
using System;
using System.Collections.Generic;
using Resto.Framework.Data;

namespace Resto.Data
{
    #region BarcodeCompletionData

    /// <summary>
    /// Класс для маппинга продукта и фасовки при работе со штрихкодами.
    /// </summary>
    public sealed class BarcodeCompletionData : IDeletable, IComparable
    {
        private Product product;
        private BarcodeContainer barcodeContainer;

        public Product Product
        {
            get { return product; }
            set { product = value; }
        }

        public BarcodeContainer BarcodeContainer
        {
            get { return barcodeContainer; }
            set { barcodeContainer = value; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as BarcodeCompletionData;
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            if (Product != null && (other.Product == null || other.Product.Id != Product.Id))
                return false;

            if (BarcodeContainer != null)
            {
                if (other.BarcodeContainer == null)
                    return false;
                if (BarcodeContainer.GetContainerIdOrDefault() != other.BarcodeContainer.GetContainerIdOrDefault())
                    return false;
                if (BarcodeContainer.Barcode != other.BarcodeContainer.Barcode)
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return BarcodeContainer == null ? 0 : BarcodeContainer.Barcode.GetHashCode();
        }

        public static bool operator ==(BarcodeCompletionData left, BarcodeCompletionData right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BarcodeCompletionData left, BarcodeCompletionData right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return barcodeContainer != null ? barcodeContainer.BarcodeString : string.Empty;
        }

        public bool Deleted
        {
            get { return product != null && product.Deleted; }
        }

        public int CompareTo(object obj)
        {
            return CompareTo((BarcodeCompletionData)obj);
        }

        public int CompareTo(BarcodeCompletionData other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Comparer<string>.Default.Compare(ToString(), other.ToString());
        }
    }
    #endregion BarcodeCompletionData
}
