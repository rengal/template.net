using System;
using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class MenuOrderType
    {
        /// <summary>
        /// Возвращает часть текста приказа, описывающую, что делает приказ и для какой ценовой категории.
        /// </summary>
        public string GetDescription(ClientPriceCategory cpc)
        {
            if (this == CHANGED)
            {
                return String.Format(Resources.MenuOrderTypeChangedDescription, GetQuotedCategoryName(cpc));
            }
            if (this == ADDED)
            {
                return String.Format(Resources.MenuOrderTypeAddedDescription, GetQuotedCategoryName(cpc));
            }
            if (this == DELETED)
            {
                return String.Format(Resources.MenuOrderTypeDeletedDescription, GetQuotedCategoryName(cpc));
            }

            return "";
        }

        private static string GetQuotedCategoryName(ClientPriceCategory cpc)
        {
            return cpc == null ? "" : string.Format("'{0}' ", cpc.NameLocal);
        }
    }
}