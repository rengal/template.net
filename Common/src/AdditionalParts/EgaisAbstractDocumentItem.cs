using System;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class EgaisAbstractDocumentItem : IWithEmptyCheck
    {
        private AlcoholClass alcoholClass;

        #region Properties

        /// <summary>
        /// Так как при добавлении через UI нового айтема <see cref="ProductInfo"/> может быть null, 
        /// реализована данная заглушка чтобы избежать предупреждений решарпера
        /// </summary>
        public EgaisProductInfo NullableProductInfo
        {
            get { return ProductInfo; }
            set
            {
                if (value == null)
                {
                    return;
                }
                ProductInfo = value;
            }
        }

        /// <summary>
        /// Полное наименование продукта в системе ЕГАИС
        /// </summary>
        public string EgaisProductFullName
        {
            get { return NullableProductInfo == null ? null : ProductInfo.FullName; }
            set
            {
                if (NullableProductInfo == null)
                {
                    ProductInfo = new EgaisProductInfo();
                }
                ProductInfo.FullName = value;
            }
        }

        /// <summary>
        /// Краткое наименование продукта в системе ЕГАИС
        /// </summary>
        public string EgaisProductShortName
        {
            get { return NullableProductInfo == null ? null : ProductInfo.ShortName; }
            set
            {
                if (NullableProductInfo == null)
                {
                    ProductInfo = new EgaisProductInfo();
                }
                ProductInfo.ShortName = value;
            }
        }

        /// <summary>
        /// Уникальный идетнификатор продукта в системе ЕГАИС
        /// </summary>
        public string EgaisAlcCode
        {
            get { return NullableProductInfo == null ? null : ProductInfo.AlcCode; }
            set
            {
                if (NullableProductInfo == null)
                {
                    ProductInfo = new EgaisProductInfo();
                }
                ProductInfo.AlcCode = value;
            }
        }

        /// <summary>
        /// Емкость фасовки
        /// </summary>
        public decimal? Capacity
        {
            get { return NullableProductInfo == null ? null : ProductInfo.Capacity; }
            set
            {
                if (NullableProductInfo == null)
                {
                    ProductInfo = new EgaisProductInfo();
                }
                ProductInfo.Capacity = value;
            }
        }

        /// <summary>
        /// Крепость алкоголя
        /// </summary>
        public decimal? AlcVolume
        {
            get { return NullableProductInfo == null ? null : ProductInfo.AlcVolume; }
        }

        /// <summary>
        /// Признак фасованного товара.
        /// Берется как есть из <see cref="ProductInfo"/>.
        /// Для более корректного расчета необходимо использовать метод EgaisDocumentHelper.IsPackedDocumentItem
        /// </summary>
        public bool? Packed
        {
            get
            {
                return NullableProductInfo != null ? ProductInfo.Packed : null;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (NullableProductInfo == null)
                {
                    ProductInfo = new EgaisProductInfo();
                }
                ProductInfo.Packed = value;
            }
        }

        /// <summary>
        /// Производитель товара
        /// </summary>
        [CanBeNull]
        public EgaisOrganizationInfo Producer
        {
            get { return NullableProductInfo != null ? ProductInfo.Producer : null; }
            set
            {
                if (NullableProductInfo == null)
                {
                    ProductInfo = new EgaisProductInfo();
                }

                ProductInfo.Producer = value;
            }
        }

        /// <summary>
        /// Импортер товара
        /// </summary>
        [CanBeNull]
        public EgaisOrganizationInfo Importer
        {
            get { return NullableProductInfo != null ? ProductInfo.Importer : null; }
            set
            {
                if (NullableProductInfo == null)
                {
                    ProductInfo = new EgaisProductInfo();
                }

                ProductInfo.Importer = value;
            }
        }

        /// <summary>
        /// Нет информации по производителю либо она не полна.
        /// true в случае если не определен <see cref="EgaisAbstractDocumentItem.ProductInfo"/> 
        /// или не определено его свойство ProductInfo.Producer 
        /// или в свойстве ProductInfo.Producer не определен ИНН
        /// </summary>
        public bool IsProducerInfoNotFullOrEmppty
        {
            get
            {
                return Producer == null || !Producer.IsCorrect;
            }
        }

        /// <summary>
        /// Нет информации по импортеру.
        /// true в случае если не определен <see cref="EgaisAbstractDocumentItem.ProductInfo"/> 
        /// или не определено его свойство ProductInfo.Importer 
        /// или в свойстве ProductInfo.Importer не определен ИНН
        /// </summary>
        public bool IsImporterInfoNotFullOrEmpty
        {
            get
            {
                return Importer == null || !Importer.IsCorrect;
            }
        }


        /// <summary>
        /// true - в айтеме есть информация о производителе товара, но в системе отсутствует такой контрагент
        /// </summary>
        public bool IsNativeProducerNotExist
        {
            get { return Producer != null && Producer.Сounteragent == null; }
        }

        /// <summary>
        /// true - в айтеме есть информация о импортере товара, но в системе отсутствует такой контрагент
        /// </summary>
        public bool IsNativeImporterNotExist
        {
            get { return Importer != null && Importer.Сounteragent == null; }
        }


        /// <summary>
        /// true - в айтеме есть информация по производителю и в системе есть соответствующий контрагент, 
        /// но этот контрагент отсутствует в списке производителей/импортеров поля <see cref="Product"/>
        /// </summary>
        public bool IsProducerNotAssignedForProduct
        {
            get
            {
                return Producer != null && !IsNativeProducerNotExist && Product != null &&
                       (Product.Producers == null || !Product.Producers.Contains(Producer.Сounteragent));
            }
        }

        /// <summary>
        /// true - в айтеме есть информация по импортеру и в системе есть соответствующий контрагент, 
        /// но этот контрагент отсутствует в списке производителей/импортеров поля <see cref="Product"/>
        /// </summary>
        public bool IsImporterNotAssignedForProduct
        {
            get
            {
                return Importer != null && !IsNativeImporterNotExist && Product != null &&
                       (Product.Producers == null || !Product.Producers.Contains(Importer.Сounteragent));
            }
        }

        /// <summary>
        /// Класс алкогольной продукции
        /// </summary>
        public AlcoholClass AlcoholClass
        {
            get
            {
                if (NullableProductInfo == null)
                {
                    return null;
                }
                if (alcoholClass == null || alcoholClass.Code != ProductInfo.ProductVCode)
                {
                    alcoholClass = EntityManager.INSTANCE.GetAll<AlcoholClass>()
                        .FirstOrDefault(alcClass => alcClass.Code == ProductInfo.ProductVCode);
                }
                return alcoholClass;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (NullableProductInfo == null)
                {
                    ProductInfo = new EgaisProductInfo();
                }

                alcoholClass = value;
                ProductInfo.ProductVCode = value.Code;
            }
        }

        /// <summary>
        /// Текстовое представление свойства <see cref="AlcoholClass"/>
        /// </summary>
        public string AlcoholClassText
        {
            get
            {
                return AlcoholClass != null
                    ? string.Format("({0}) {1}", AlcoholClass.Code, AlcoholClass.NameLocal)
                    : null;
            }
        }

        public virtual bool IsCorrect
        {
            get
            {
                return NullableProductInfo != null &&
                       (ProductInfo.Producer != null || ProductInfo.Importer != null) &&
                       !EgaisAlcCode.IsNullOrWhiteSpace() && AlcoholClass != null;
            }
        }

        public virtual bool IsEmptyRow
        {
            get { return false; }
        }

        public bool IsProductABeer
        {
            get
            {
                return AlcoholClass != null && AlcoholClass.AlcoholClassGroup != null &&
                       Equals(AlcoholClass.AlcoholClassGroup.Type, AlcoholType.BEER);
            }
        }

        public virtual EgaisAbstractDocumentItem ConvertItemTo<T>(bool? packed)
            where T : EgaisAbstractDocumentItem, new()
        {
            var item = new T();
            item.Id = Guid.NewGuid();
            item.Num = Num;
            item.SupplierProduct = SupplierProduct;
            item.AmountUnit = AmountUnit;
            item.ContainerId = ContainerId;
            item.ProductInfo = ProductInfo;
            item.Product = Product;
            item.Amount = Amount;
            item.Packed = packed;

            return item;
        }

        #endregion
    }
}
