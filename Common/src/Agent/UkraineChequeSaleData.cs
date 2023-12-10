using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    /// <summary>
    /// Дополнительные данные о товаре в соответствии с законодательством Украины
    /// </summary>
    public sealed class UkraineChequeSaleData
    {
        public UkraineChequeSaleData()
        {
        }

        public UkraineChequeSaleData(List<string> markingCodes, string containerBarcode)
        {
            MarkingCodes = markingCodes;
            ContainerBarcode = containerBarcode;
        }

        /// <summary>
        /// Штрихкоды акцизных марок
        /// </summary>
        public List<string> MarkingCodes { get; set; }

        /// <summary>
        /// Отсканированный при добавлении товара штрихкод
        /// либо штрихкод фасовки по умолчанию
        /// </summary>
        [CanBeNull]
        public string ContainerBarcode { get; set; }
    }
}