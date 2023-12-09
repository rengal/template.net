using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Barcodes
{
    /// <summary>
    ///Двумерный матричный штрихкод,в котором представлены марки ЕГАИС и табачной продукции.
    /// </summary>
    public class BarcodeDataMatrix : IBarcode, IEquatable<BarcodeDataMatrix>
    {
        [NotNull]
        private readonly string barcodeString = string.Empty;
        #region Ctor

        /// <summary>
        /// Пустой конструктор для корректной работы десериализатора
        /// </summary>
        [UsedImplicitly]
        private BarcodeDataMatrix()
        {

        }

        /// <summary>
        /// Создаёт экземпляр штрихкода по строке <paramref name="barcodeString"/>
        /// </summary>
        /// <param name="barcodeString">Строка с цифрами штрихкода</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="barcodeString"/><c> == null</c>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="barcodeString"/> не является допустимой строкой для штрихкода DataMatrix
        /// (см. <see cref="IsEgaisMark"/>)
        /// </exception>
        /// <seealso cref="IsMark()"/>
        private protected BarcodeDataMatrix([NotNull] string barcodeString)
        {
            if (barcodeString == null)
                throw new ArgumentNullException(nameof(barcodeString));
            
            this.barcodeString = barcodeString;
        }

        public static BarcodeDataMatrix CreateEgaisBarcode([NotNull] string barcodeString)
        {
            if (barcodeString == null)
                throw new ArgumentNullException(nameof(barcodeString));
            if (!IsEgaisMark(barcodeString))
                throw new ArgumentOutOfRangeException(nameof(barcodeString), barcodeString, @"Invalid barcode string");

            return new BarcodeDataMatrix(barcodeString);
        }

        private static BarcodeDataMatrix CreateDairyProductBarcode([NotNull] string barcodeString, bool needInsertSeparator)
        {
            if (barcodeString == null)
                throw new ArgumentNullException(nameof(barcodeString));
            
            if (needInsertSeparator)
            {
                barcodeString = barcodeString.Insert(24, CommonConstants.SeparatorMark);
            }
            return new BarcodeDataMatrix(barcodeString);
        }

        private static BarcodeDataMatrix CreateWeightedDairyProductBarcode([NotNull] string barcodeString, bool needInsertSeparator)
        {
            if (barcodeString == null)
                throw new ArgumentNullException(nameof(barcodeString));
            
            if (needInsertSeparator)
            {
                barcodeString = barcodeString.Insert(24, CommonConstants.SeparatorMark);
                barcodeString = barcodeString.Insert(31, CommonConstants.SeparatorMark);
            }
            return new BarcodeDataMatrix(barcodeString);
        }

        private static BarcodeDataMatrix CreateBottledWaterBarcode([NotNull] string barcodeString, bool needInsertSeparator)
        {
            if (barcodeString == null)
                throw new ArgumentNullException(nameof(barcodeString));
            
            if (needInsertSeparator)
            {
                barcodeString = barcodeString.Insert(31, CommonConstants.SeparatorMark);
            }
            return new BarcodeDataMatrix(barcodeString);
        }

        private static BarcodeDataMatrix CreateTobaccoBarcode([NotNull] string barcodeString)
        {
            if (barcodeString == null)
                throw new ArgumentNullException(nameof(barcodeString));
            
            return new BarcodeDataMatrix(barcodeString);
        }

        private static BarcodeDataMatrix CreateTobaccoBlockBarcode([NotNull] string barcodeString, bool needInsertSeparator)
        {
            if (barcodeString == null)
                throw new ArgumentNullException(nameof(barcodeString));
            
            if (needInsertSeparator)
            {
                barcodeString = barcodeString.Insert(25, CommonConstants.SeparatorMark);
                barcodeString = barcodeString.Insert(36, CommonConstants.SeparatorMark);
            }
            
            return new BarcodeDataMatrix(barcodeString);
        }

        public static bool TryParseDataMatrixBarcode(string barcodeString, out BarcodeDataMatrix barcode)
        {
            if (barcodeString == null)
                throw new ArgumentNullException(nameof(barcodeString));

            string serial = null;
            string price = null;
            barcode = null;
            var GSchar = CommonConstants.SeparatorMark[0];

            if (BarcodeDataMatrix.IsEgaisMark(barcodeString))
            {
                barcode = BarcodeDataMatrix.CreateEgaisBarcode(barcodeString);

                return true;
            }
            if (BarcodeDataMatrix.IsDairyProductMark(barcodeString, out var needInsertSeparator, out var gtin, out serial))
            {
                barcode = BarcodeDataMatrix.CreateDairyProductBarcode(barcodeString, needInsertSeparator);
                barcode.Gtin = gtin;
                barcode.CommodityCode = GetCommodityMark(gtin, serial, null);

                return true;
            }
            if (BarcodeDataMatrix.IsWeightedDairyProductMark(barcodeString, out needInsertSeparator, out gtin, out serial, out var weight))
            {
                barcode = BarcodeDataMatrix.CreateWeightedDairyProductBarcode(barcodeString, needInsertSeparator);
                barcode.Gtin = gtin;
                barcode.Weight = weight;
                barcode.CommodityCode = GetCommodityMark(gtin, serial, null);

                return true;
            }
            if (BarcodeDataMatrix.IsBottledWaterMark(barcodeString, out needInsertSeparator, out gtin, out serial))
            {
                barcode = BarcodeDataMatrix.CreateBottledWaterBarcode(barcodeString, needInsertSeparator);
                barcode.Gtin = gtin;
                barcode.CommodityCode = GetCommodityMark(gtin, serial, null);

                return true;
            }
            if (BarcodeDataMatrix.IsTobaccoBlockMark(barcodeString, out needInsertSeparator, out gtin, out serial, out price))
            {
                barcode = BarcodeDataMatrix.CreateTobaccoBlockBarcode(barcodeString, needInsertSeparator);
                barcode.Gtin = gtin;
                barcode.CommodityCode = GetCommodityMark(gtin, serial, price);

                return true;
            }
            if (BarcodeDataMatrix.IsTobaccoMark(barcodeString, out gtin, out serial))
            {
                barcode = BarcodeDataMatrix.CreateTobaccoBarcode(barcodeString);
                barcode.Gtin = gtin;
                barcode.CommodityCode = GetCommodityMarkTobacco(gtin, serial);
                return true;
            }
            if (IsDataMatrixGSMark(barcodeString, out gtin, out serial))
            {
                var commodityCode = GetCommodityMark(gtin, serial, null);

                barcode = new BarcodeDataMatrix(barcodeString)
                {
                    Gtin = gtin,
                    CommodityCode = commodityCode
                };

                return true;
            }

            return false;
        }

        #endregion

        #region Props

        public string BarcodeString => barcodeString;

        public string Gtin { get; set; }

        [CanBeNull]
        public string Weight { get; set; }

        public string CommodityCode { get; set; }

        #endregion

        #region Methods
        /// <summary>
        /// ЕГАИС: Формат штрихового кода, наносимого АО "Гознак" на марки, выпускаемые после 01.07.2018.
        /// </summary>
        public static bool IsEgaisMark(string mark)
        {
            return mark.Length == 150;
        }
        public bool IsEgaisMark()
        {
            return IsEgaisMark(barcodeString);
        }

        public static bool IsMark(string mark)
        {
            return IsTobaccoBlockMark(mark, out _, out _, out _, out _)
                   || IsTobaccoMark(mark, out _, out _)
                   || IsBottledWaterMark(mark, out _, out _, out _)
                   || IsDairyProductMark(mark, out _, out _, out _)
                   || IsWeightedDairyProductMark(mark, out _, out _, out _, out _)
                   || IsDataMatrixGSMark(mark, out _, out _);
        }

        public bool IsMark()
        {
            return IsMark(barcodeString);
        }

        private static bool CheckMarkPrefix(string mark, out string gtin)
        {
            gtin = null;
            if (mark[0] != '0')
                return false;

            if (mark[1] != '1')
                return false;

            for (var i = 2; i < CommonConstants.LengthOfGtinMark + 2; i++)
            {
                //Gtin
                if (!char.IsDigit(mark[i]))
                    return false;
            }

            if (mark[CommonConstants.LengthOfGtinMark + 2] != '2')
                return false;

            if (mark[CommonConstants.LengthOfGtinMark + 3] != '1')
                return false;

            gtin = mark.Substring(2, CommonConstants.LengthOfGtinMark);

            return true;
        }

        public static bool IsDairyProductMark(string mark, out bool needInsertSeparator, out string gtin, out string serial)
        {
            var GSchar = CommonConstants.SeparatorMark[0];
            bool containsGS = mark.Any(c => c == GSchar);
            needInsertSeparator = false;
            gtin = null;
            serial = null;

            if ((mark.Length != CommonConstants.DairyProductMarkLength && containsGS) ||
                (mark.Length != CommonConstants.DairyProductMarkLength - 1 && !containsGS))
                return false;

            if (!CheckMarkPrefix(mark, out var gtinValue)) return false;

            int index = CommonConstants.LengthOfGtinMark + 4;
            var serialValue = mark.Substring(index, CommonConstants.LengthOfSerialDairyProduct);
            index += CommonConstants.LengthOfSerialDairyProduct;

            if (containsGS)
            {
                if (mark[index] != GSchar)
                    return false;

                index++;
            }

            if (mark[index] != '9')
                return false;

            index++;
            if (mark[index] != '3')
                return false;

            if (!containsGS)
            {
                needInsertSeparator = true;
            }

            gtin = gtinValue;
            serial = serialValue;
            return true;
        }

        public static bool IsWeightedDairyProductMark(string mark, out bool needInsertSeparator, out string gtin, out string serial, out string weight)
        {
            var GSchar = CommonConstants.SeparatorMark[0];
            bool containsGS = mark.Any(c => c == GSchar);
            needInsertSeparator = false;
            gtin = null;
            serial = null;
            weight = null;

            if ((mark.Length != CommonConstants.WeightedDairyProductMarkLength && containsGS) ||
                (mark.Length != CommonConstants.WeightedDairyProductMarkLength - 2 && !containsGS))
                return false;

            if (!CheckMarkPrefix(mark, out var gtinValue)) return false;

            int index = CommonConstants.LengthOfGtinMark + 4;
            var serialValue = mark.Substring(index, CommonConstants.LengthOfSerialWeightedDairyProduct);
            index += CommonConstants.LengthOfSerialWeightedDairyProduct;

            if (containsGS)
            {
                if (mark[index] !=
                    GSchar)
                    return false;

                index++;
            }

            if (mark[index] != '9')
                return false;

            index++;
            if (mark[index] != '3')
                return false;

            index += 5;

            if (containsGS)
            {
                if (mark[index] != GSchar)
                    return false;

                index++;
            }

            if (mark[index] != '3')
                return false;

            index++;
            if (mark[index] != '1')
                return false;

            index++;
            if (mark[index] != '0')
                return false;

            index++;
            if (mark[index] != '3')
                return false;
            if (!containsGS)
            {
                needInsertSeparator = true;
            }
            index++;
            var weightValue = mark.Substring(index, CommonConstants.LengthOfWeightDairyProduct);

            serial = serialValue;
            gtin = gtinValue;
            weight = weightValue;
            return true;
        }

        public static bool IsBottledWaterMark(string mark, out bool needInsertSeparator, out string gtin, out string serial)
        {
            var GSchar = CommonConstants.SeparatorMark[0];
            bool containsGS = mark.Any(c => c == GSchar);
            needInsertSeparator = false;
            gtin = null;
            serial = null;

            if ((mark.Length != CommonConstants.BottledWaterMarkLength && containsGS) ||
                (mark.Length != CommonConstants.BottledWaterMarkLength - 1 && !containsGS))
                return false;

            if (!CheckMarkPrefix(mark, out var gtinValue)) return false;

            int index = CommonConstants.LengthOfGtinMark + 4;
            var serialValue = mark.Substring(index, CommonConstants.LengthOfSerialBottledWater);
            index += CommonConstants.LengthOfSerialBottledWater;

            if (containsGS)
            {
                if (mark[index] != GSchar)
                    return false;

                index++;
            }

            if (mark[index] != '9')
                return false;

            index++;
            if (mark[index] != '3')
                return false;

            if(!containsGS)
            {
                needInsertSeparator = true;
            }

            gtin = gtinValue;
            serial = serialValue;
            return true;
        }

        public static bool IsTobaccoMark(string mark, out string gtin, out string serial)
        {
            serial = null;
            gtin = null;
            if (mark.Length != CommonConstants.TobaccoMarkLength)
                return false;

            for (var i = 0; i < CommonConstants.LengthOfGtinMark; i++)
            {
                //Gtin
                if (!char.IsDigit(mark[i]))
                    return false;
            }

            gtin = mark.Substring(0, CommonConstants.LengthOfGtinMark);
            serial = mark.Substring(CommonConstants.LengthOfGtinMark, CommonConstants.LengthOfAdditionalPartTobaccoMark);

            return true;
        }

        public static bool IsTobaccoBlockMark(string mark, out bool needInsertSeparator, out string gtin, out string serial, out string price)
        {
            var GSchar = CommonConstants.SeparatorMark[0];
            bool containsGS = mark.Any(c => c == GSchar);
            needInsertSeparator = false;
            serial = null;
            gtin = null;
            price = null;

            if ((mark.Length < CommonConstants.TobaccoBlockMarkLength && containsGS) ||
                (mark.Length < CommonConstants.TobaccoBlockMarkLength - 2 && !containsGS))
                return false;

            if (!CheckMarkPrefix(mark, out var gtinValue)) return false;

            int index = CommonConstants.LengthOfGtinMark + 4;
            var serialValue = mark.Substring(index, CommonConstants.LengthOfSerialTobaccoMark);
            index += CommonConstants.LengthOfSerialTobaccoMark;

            if (containsGS)
            {
                if (mark[index] != GSchar)
                    return false;

                index++;
            }

            if (mark[index] != '8')
                return false;
            index++;
            if (mark[index] != '0')
                return false;
            index++;
            if (mark[index] != '0')
                return false;
            index++;
            if (mark[index] != '5')
                return false;

            string priceValueSubstring = mark.Substring(index + 1, CommonConstants.LengthOfPriceTobaccoMark);
            if (!priceValueSubstring.All(char.IsDigit))
                return false;
            index = index + CommonConstants.LengthOfPriceTobaccoMark + 1;

            if(containsGS)
            {
                if (mark[index] != GSchar)
                    return false;

                index++;
            }

            if (mark[index] != '9')
                return false;
            index++;
            if (mark[index] != '3')
                return false;

            if (!containsGS)
                needInsertSeparator = true;

            serial = serialValue;
            gtin = gtinValue;
            price = priceValueSubstring;

            return true;
        }

        public static bool IsDataMatrixGSMark(string mark, out string gtin, out string serial)
        {
            var GSchar = CommonConstants.SeparatorMark[0];
            bool containsGS = mark.Any(c => c == GSchar);
            serial = null;
            gtin = null;

            if (mark.Length < CommonConstants.MinLengthOfTobaccoMark || mark.Length > CommonConstants.MaxMarkLength)
                return false;

            if (!CheckMarkPrefix(mark, out var gtinValue)) return false;

            if (!containsGS)
                return false;

            var startSerialTegIndex = CommonConstants.CodeOfGtinMark.Length + CommonConstants.LengthOfGtinMark + CommonConstants.CodeOfSerialTobaccoMark.Length;
            var endSerialIndex = mark.IndexOf(CommonConstants.SeparatorMark, startSerialTegIndex + CommonConstants.CodeOfSerialTobaccoMark.Length, StringComparison.OrdinalIgnoreCase);
            var lengthSerial = endSerialIndex - startSerialTegIndex;

            serial = mark.Substring(startSerialTegIndex, lengthSerial);
            gtin = gtinValue;

            return true;
        }

        [NotNull]
        private static string GetCommodityMark([NotNull] string gtin, [NotNull] string serial, [CanBeNull] string price)
        {
            if (gtin == null)
                throw new AbandonedMutexException(nameof(gtin));

            if (serial == null)
                throw new ArgumentNullException(nameof(serial));

            var convertGtin = ConvertToHexString(gtin);
            var convertSerial = ConvertToAscii(serial);
            string convertPrice = string.Empty;
            if (!string.IsNullOrEmpty(price))
                convertPrice = ConvertToAscii(price);

            return string.Join(string.Empty, CommonConstants.CodeOfTobaccoMark, convertGtin, convertSerial, convertPrice);
        }

        private static string GetCommodityMarkTobacco([NotNull] string gtin, [NotNull] string serial)
        {
            if (gtin == null)
                throw new AbandonedMutexException(nameof(gtin));

            if (serial == null)
                throw new ArgumentNullException(nameof(serial));

            var convertGtin = ConvertToHexString(gtin);
            var convertSerial = ConvertToAscii(serial);
            while (convertSerial.Length < 26)
            {
                //дополняем знаками "20" в конце (пробелами справа) до 13 байт.
                convertSerial += "20";
            }
            return string.Join(string.Empty, CommonConstants.CodeOfTobaccoMark, convertGtin, convertSerial);
        }

        private static string ConvertToHexString(string gtin)
        {
            return Convert.ToInt64(gtin).ToString("X", CultureInfo.InvariantCulture).PadLeft(12, '0');
        }

        private static string ConvertToAscii(string barcode)
        {
            return string.Join(string.Empty, Encoding.ASCII.GetBytes(barcode).Select(c => $"{Convert.ToInt32(c):X2}"));
        }

        public override string ToString()
        {
            return barcodeString;
        }
        #endregion

        #region Equality Members
        public bool Equals(BarcodeDataMatrix other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Equals(other.barcodeString, barcodeString);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(BarcodeDataMatrix))
                return false;
            return Equals((BarcodeDataMatrix)obj);
        }

        public override int GetHashCode()
        {
            return barcodeString.GetHashCode();
        }

        public static bool operator ==(BarcodeDataMatrix left, BarcodeDataMatrix right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BarcodeDataMatrix left, BarcodeDataMatrix right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}
