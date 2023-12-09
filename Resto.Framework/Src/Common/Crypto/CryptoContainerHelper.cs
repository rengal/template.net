using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Resto.Framework.Properties;

namespace Resto.Framework.Common.Crypto
{
    /// <summary>
    /// Делегат для метода записи в криптоконтейнер информации, определяемой вызывающей стороной
    /// </summary>
    /// <param name="writer"></param>
    public delegate void WriteMetadataDelegate(XmlWriter writer);

    /// <summary>
    /// Вспомогательтный класс для обработки криптоконтейнеров. Криптоконтейнер - XML-файл специального
    /// формата, содержащий, помимо зашифрованных данных, вспомогательную информацию для определения правил
    /// обработки и контроля целостности данных.
    /// </summary>
    public static class CryptoContainerHelper
    {
        #region Общие методы и свойства
        /// <summary>
        /// Текущая версия формата
        /// </summary>
        private const int ContainerVersion = 1;

        /// <summary>
        /// Маска форматирования для даты и времени
        /// </summary>
        private const string DateTimeFormat = "dd.MM.yyyy HH:mm:ss";

        /// <summary>
        /// Записать XmlAttribute
        /// </summary>
        /// <param name="writer">XmlWriter, осуществляющий запись</param>
        /// <param name="attributeName">Название атрибута</param>
        /// <param name="attributeValue">Значение атрибута</param>
        private static void WriteXmlAttribute(XmlWriter writer, string attributeName, string attributeValue)
        {
            writer.WriteStartAttribute(attributeName);
            writer.WriteString(attributeValue);
            writer.WriteEndAttribute();
        }

        #endregion

        #region Запись
        /// <summary>
        /// Записать секцию настроек криптографии
        /// </summary>
        /// <param name="writer">XmlWriter</param>
        /// <param name="crypter">Активный модуль шифрования</param>
        private static void WriteCryptoSection(XmlWriter writer, CrypterBase crypter)
        {
            // в зависимости от используемого модуля пишем его тип
            // TODO вынести это в CrypterBase
            writer.WriteStartElement(CryptoContainerXmlConsts.CryptoNode);
            switch (crypter.Algorithm)
            {
                case CryptoAlgorithmType.DES:
                    WriteXmlAttribute(writer, CryptoXmlConsts.cryptoAlgorithm, CryptoXmlConsts.cryptoAlgorithmDES);
                    break;
                case CryptoAlgorithmType.None:
                    WriteXmlAttribute(writer, CryptoXmlConsts.cryptoAlgorithm, CryptoXmlConsts.cryptoAlgorithmNone);
                    break;
            }
            // пишем специфические данные модуля (хэш ключа и т.п.)
            crypter.WriteContainerInfo(writer);

            writer.WriteEndElement();
        }

        /// <summary>
        /// Записать секцию данных, определяемых вызывающей стороной
        /// </summary>
        /// <param name="writer">XmlWriter</param>
        /// <param name="metadataWriter">Метод записи</param>
        private static void WriteMetadataSection(XmlWriter writer, WriteMetadataDelegate metadataWriter)
        {
            writer.WriteStartElement(CryptoContainerXmlConsts.MetadataNode);
            metadataWriter(writer);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Записать секцию данных
        /// </summary>
        /// <param name="crypter">Активный модуль шифрования</param>
        /// <param name="writer">XmlWriter</param>
        /// <param name="data">исходные данные</param>
        private static void WriteDataSection(CrypterBase crypter, XmlWriter writer, byte[] data)
        {
            writer.WriteStartElement(CryptoContainerXmlConsts.DataNode);
            // шифруем данные
            var cryptedData = crypter.EncryptData(data);
            // пишем размер зашифрованных данных
            WriteXmlAttribute(writer, CryptoContainerXmlConsts.DataSize, cryptedData.Length.ToString());
            // пишем котрольную сумму зашифрованных данных
            long crc32 = Crc32Helper.CalcCrc32(cryptedData, 0, cryptedData.Length);
            WriteXmlAttribute(writer, CryptoContainerXmlConsts.DataCrc32, crc32.ToString());
            // пишем зашифрованные данные в формате Base64
            writer.WriteCData(Convert.ToBase64String(cryptedData, Base64FormattingOptions.InsertLineBreaks));
            writer.WriteEndElement();
        }

        /// <summary>
        /// Создать криптоконтейнер
        /// </summary>
        /// <param name="crypter">Активный модуль шифрования</param>
        /// <param name="sourceData">Исхожные данные</param>
        /// <param name="error">Текст ошибки</param>
        /// <param name="containerFileName">Полный путь к файлу</param>
        /// <param name="ownerType">Тип объекта, которому принадлежат данные (необязательный)</param>
        /// <param name="metadataWriter">Метод записи данных, определяемых вызывающей стороной</param>
        /// <returns>true - криптоконтейнер сформирован / false - произошла ошибка</returns>
        public static bool WriteContainer(CrypterBase crypter, byte[] sourceData, out string error,
            string containerFileName, Type ownerType, WriteMetadataDelegate metadataWriter)
        {
            error = null;
            // модуль шифрования должен быть задан
            if (crypter == null)
            {
                error = "Crypter must be defined.";
                return false;
            }
            // исходные данные должны быть заданы
            if (sourceData == null || sourceData.Length == 0)
            {
                error = "Source data must be present.";
                return false;
            }
            // если существует файл с заданным именем..
            if (File.Exists(containerFileName))
            {
                try
                {
                    // .. пытаемся удалить
                    File.Delete(containerFileName);
                }
                catch (Exception e)
                {
                    error = $"Can not delete existing container '{containerFileName}': {e.Message}";
                    return false;
                }
            }
            // инициализируем XmlWriter
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                CheckCharacters = false,
                CloseOutput = true,
                Indent = true,
                IndentChars = "\t",
                OmitXmlDeclaration = false
            };

            using (var writer = XmlWriter.Create(containerFileName, settings))
            {
                try
                {
                    writer.WriteStartDocument();
                    // корневой узел
                    writer.WriteStartElement(CryptoContainerXmlConsts.RootNode);
                    // текущая версия формата
                    WriteXmlAttribute(writer, CryptoContainerXmlConsts.Version, ContainerVersion.ToString());
                    // если задан владелец данных - пишем и его
                    if (ownerType != null)
                    {
                        WriteXmlAttribute(writer, CryptoContainerXmlConsts.OwnerType, ownerType.Name);
                    }
                    // дата создания криптоконтейнера
                    WriteXmlAttribute(writer, CryptoContainerXmlConsts.Created, DateTime.Now.ToString(DateTimeFormat, null));
                    // настройки модуля шифрования
                    WriteCryptoSection(writer, crypter);
                    // если задан делегат польовательских данных - вызываем
                    if (metadataWriter != null)
                    {
                        WriteMetadataSection(writer, metadataWriter);
                    }
                    // шифруем и пишем данные
                    WriteDataSection(crypter, writer, sourceData);

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();

                    return true;
                }
                catch (Exception e)
                {
                    writer.Close();
                    // в случае неудачи - удаляем частично сформированный файл
                    File.Delete(containerFileName);
                    error = "Unexpected error has been occured: " + e.Message;
                    return false;
                }
            }
        }
        #endregion

        #region Чтение

        /// <summary>
        /// Прочитать заголовок криптоконтейнера
        /// </summary>
        /// <param name="reader">XmlReader</param>
        /// <param name="info">Информация о контейнере</param>
        /// <param name="error">Текст ошибки</param>
        /// <returns>true/false</returns>
        private static bool ReadHeader(XmlReader reader, CryptoContainerInfo info, out string error)
        {
            error = null;
            if (reader.Name != CryptoContainerXmlConsts.RootNode)
            {
                error = Resources.CryptoContainerHelperInvalidFileFormat;
                return false;
            }

            #region Версия
            if (!reader.MoveToAttribute(CryptoContainerXmlConsts.Version))
            {
                error = string.Format(Resources.CryptoContainerHelperInvalidFileFormatAttributeNotFound, CryptoContainerXmlConsts.Version);
                return false;
            }

            if (!int.TryParse(reader.Value, out var ver))
            {
                error = string.Format(Resources.CryptoContainerHelperInvalidFileFormatCanNotInterpretValue, reader.Value);
                return false;
            }

            if (ver != ContainerVersion)
            {
                error = string.Format(Resources.CryptoContainerHelperUnknownContainerVersion, reader.Value);
                return false;
            }
            #endregion

            #region OnerType
            if (reader.MoveToAttribute(CryptoContainerXmlConsts.OwnerType))
            {
                info.OwnerTypeStr = reader.Value;
            }
            #endregion

            #region DateCreated
            if (!reader.MoveToAttribute(CryptoContainerXmlConsts.Created))
            {
                error = string.Format(Resources.CryptoContainerHelperInvalidFileFormatAttributeNotFound, CryptoContainerXmlConsts.Created);
                return false;
            }

            if (!DateTime.TryParseExact(reader.Value, DateTimeFormat, null, DateTimeStyles.None, out var created))
            {
                error = string.Format(Resources.CryptoContainerHelperInvalidFileFormatCanNotInterpretValue, reader.Value);
                return false;
            }
            info.Created = created;
            #endregion

            return true;
        }

        /// <summary>
        /// Прочитать секцию настроек модуля шифрования
        /// </summary>
        /// <param name="reader">XmlReader</param>
        /// <param name="info">Информация о контейнере</param>
        /// <param name="error">Текст ошибки</param>
        /// <returns>true/false</returns>
        private static bool ReadCryptoSection(XmlReader reader, CryptoContainerInfo info, out string error)
        {
            error = null;
            if (!reader.MoveToAttribute(CryptoContainerXmlConsts.Algorithm))
            {
                error = String.Format(Resources.CryptoContainerHelperInvalidFileFormatAttributeNotFound, CryptoContainerXmlConsts.Algorithm);
                return false;
            }

            var algStr = reader.Value;
            switch (algStr)
            {
                case CryptoXmlConsts.cryptoAlgorithmDES:
                    info.Algorithm = CryptoAlgorithmType.DES;
                    break;
                case CryptoXmlConsts.cryptoAlgorithmNone:
                    info.Algorithm = CryptoAlgorithmType.None;
                    return true;
                default:
                    error = String.Format(Resources.CryptoContainerHelperUnsupportedAttributeValue,
                        algStr, CryptoContainerXmlConsts.Algorithm);
                    return false;
            }

            reader.Read();
            info.CryptoSettingsStr = reader.ReadOuterXml();
            return true;
        }

        /// <summary>
        /// Прочитать секцию данных, определямых вызывающей стороной
        /// </summary>
        /// <param name="reader">XmlReader</param>
        /// <param name="info">Информация о контейнере</param>
        /// <param name="error">Текст ошибки</param>
        /// <returns>true/false</returns>
        private static bool ReadMetadataSection(XmlReader reader, CryptoContainerInfo info, out string error)
        {
            error = null;
            using (var rdr = reader.ReadSubtree())
            {
                if (rdr.Read())
                    info.MetadataStr = rdr.ReadOuterXml();
            }
            return true;
        }

        /// <summary>
        /// Прочитать секцию данных
        /// </summary>
        /// <param name="reader">XmlReader</param>
        /// <param name="info">Информация о контейнере</param>
        /// <param name="error">Текст ошибки</param>
        /// <returns>true/false</returns>
        private static bool ReadDataSection(XmlReader reader, CryptoContainerInfo info, out string error)
        {
            error = null;
            if (!reader.MoveToAttribute(CryptoContainerXmlConsts.DataSize))
            {
                error = String.Format(Resources.CryptoContainerHelperInvalidFileFormatAttributeNotFound, CryptoContainerXmlConsts.DataSize);
                return false;
            }

            if (!int.TryParse(reader.Value, out var tempInt))
            {
                error = string.Format(Resources.CryptoContainerHelperInvalidFileFormatCanNotInterpretValue, reader.Value);
                return false;
            }
            info.Length = tempInt;

            if (!reader.MoveToAttribute(CryptoContainerXmlConsts.DataCrc32))
            {
                error = string.Format(Resources.CryptoContainerHelperInvalidFileFormatAttributeNotFound, CryptoContainerXmlConsts.DataCrc32);
                return false;
            }

            if (!uint.TryParse(reader.Value, out var tempUint))
            {
                error = string.Format(Resources.CryptoContainerHelperInvalidFileFormatCanNotInterpretValue, reader.Value);
                return false;
            }
            info.Crc32 = tempUint;

            reader.MoveToElement();
            reader.Read();
            if (reader.NodeType != XmlNodeType.CDATA)
            {
                error = Resources.CryptoContainerHelperDataSectionNotFound;
                return false;
            }
            var dataStr = reader.ReadContentAsString();

            info.Data = Convert.FromBase64String(dataStr);

            return true;
        }

        /// <summary>
        /// Прочитать файл криптоконтейнера и создать экземпяр CryptoContainerInfo
        /// </summary>
        /// <param name="containerFileName">Полный путь и имя файла</param>
        /// <param name="info">CryptoContainerInfo со всеми данными контейнера</param>
        /// <param name="error">Текст ошибки</param>
        /// <returns>true - криптоконтейнер прочитан успешно / false - возникла ошибка</returns>
        public static bool ReadContainer(string containerFileName, out CryptoContainerInfo info, out string error)
        {
            // создаем экземпяр CryptoContainerInfo
            info = new CryptoContainerInfo();
            try
            {
                // инициализируем XmlReader
                var settings = new XmlReaderSettings
                {
                    CheckCharacters = false,
                    CloseInput = true,
                    IgnoreComments = true,
                    IgnoreWhitespace = true
                };
                // читаем Xml из файла
                using (var inputStream = File.OpenRead(containerFileName))
                {
                    using (var reader = XmlReader.Create(inputStream, settings))
                    {
                        reader.MoveToElement();
                        reader.MoveToContent();
                        if (!ReadHeader(reader, info, out error))
                            return false;
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    switch (reader.Name)
                                    {
                                        case CryptoContainerXmlConsts.CryptoNode:
                                            if (!ReadCryptoSection(reader, info, out error))
                                                return false;
                                            break;
                                        case CryptoContainerXmlConsts.MetadataNode:
                                            if (!ReadMetadataSection(reader, info, out error))
                                                return false;
                                            break;
                                        case CryptoContainerXmlConsts.DataNode:
                                            if (!ReadDataSection(reader, info, out error))
                                                return false;
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                }
                // проверяем размер и контрольную сумму
                return info.ValidateData(out error);
            }
            catch (XmlException xmlException)
            {
                error = string.Format(Resources.CryptoContainerHelperInvalidXMLFormat, xmlException.Message);
                return false;
            }
            catch (Exception e)
            {
                error = string.Format(Resources.CryptoContainerHelperFileProcessingError, e.Message);
                return false;
            }
        }
        #endregion
    }
}