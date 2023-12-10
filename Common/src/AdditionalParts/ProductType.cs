using System.Collections.Generic;
using System.Linq;
using Resto.Common;
using Resto.Common.Localization;
using Resto.Common.Properties;
using Resto.Framework.Common;
using Resto.Framework.Data;
using Resto.UI.Common;

namespace Resto.Data
{
    public partial class ProductType
    {
        public string GetAlterName()
        {
            if (this == GOODS || this == DISH || this == MODIFIER)
                return this.GetLocalName();
            if (this == PREPARED)
                return Resources.ProductTypeGetAlterNameTheBlank;
            if (this == SERVICE)
                return Resources.ProductTypeGetAlterNameService;
            if (this == RATE)
                return Resources.ProductTypeGetAlterNameRate;
            return "";
        }

        public string PluralName
        {
            get
            {
                string pname = this.GetLocalName();
                if (this == GOODS)
                    pname = Resources.ProductTypePluralNameGoods;
                else if (this == DISH)
                    pname = Resources.ProductTypePluralNameDishes;
                else if (this == MODIFIER)
                    pname = Resources.ProductTypePluralNameModifiers;
                else if (this == PREPARED)
                    pname = Resources.ProductTypePluralNameBlanks;
                else if (this == SERVICE)
                    pname = Resources.ProductTypePluralNameServices;
                else if (this == RATE)
                    pname = Resources.ProductTypePluralNameRates;

                return pname;
            }
        }

        public bool MayHaveAssemblyChart
        {
            get
            {
                return this == DISH || this == PREPARED || this == MODIFIER;
            }
        }

        /// <summary>
        /// Возвращает <c>true</c>, если продукт с данным типом может иметь модификаторы.
        /// </summary>
        /// <seealso cref="Product.Modifiers"/>
        /// <seealso cref="Product.ModifierSchema"/>
        /// <seealso cref="Product.ModifierSchemaRedefinitions"/>
        public bool MayHaveModifiers
        {
            get { return this != MODIFIER; }
        }

        /// <summary>
        /// <c>true</c>, если типу продуктов может быть назначена шкала размеров.
        /// </summary>
        public bool CanHaveSize
        {
            get { return !this.In(GOODS, SERVICE, RATE); }
        }

        public static ProductType[] VALUES_NATIV
        {
            get
            {
                var list = new List<ProductType>
                {
                    GOODS,
                    DISH,
                    PREPARED,
                    SERVICE,
                    MODIFIER,
                    RATE
                };
                return list.ToArray();
            }
        }

        /// <summary>
        /// Типы продуктов, у которых могут быть тех. карты
        /// </summary>
        /// <returns></returns>
        public static ProductType[] TypesWithAssemblyChart => VALUES_NATIV
            .Where(type => type.MayHaveAssemblyChart)
            .ToArray();
    }
}