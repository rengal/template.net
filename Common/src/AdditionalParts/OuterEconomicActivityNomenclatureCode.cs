using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class OuterEconomicActivityNomenclatureCode
    {
        public override string ToString()
        {
            return $"{OuterEanCode} | {NameLocal}";
        }

        public bool Equals(OuterEconomicActivityNomenclatureCode product)
        {
            if (product == null)
            {
                return false;
            }

            return OuterEanCode.Replace(" ", string.Empty) == product.OuterEanCode.Replace(" ", string.Empty);
        }

        public bool Equals(string code)
        {
            if (code == null)
            {
                return false;
            }

            return OuterEanCode.Replace(" ", string.Empty) == code.Replace(" ", string.Empty);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as OuterEconomicActivityNomenclatureCode);
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            if (!OuterEanCode.IsNullOrEmpty())
            {
                hashCode ^= OuterEanCode.GetHashCode();
            }
            return hashCode;
        }

    }
}
