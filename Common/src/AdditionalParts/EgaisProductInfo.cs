using Resto.Framework.Common;
using Resto.Framework.Common.XmlSerialization;

namespace Resto.Data
{
    public partial class EgaisProductInfo
    {
        #region Methods

        public bool Equals(EgaisProductInfo productInfo)
        {
            if (productInfo == null)
            {
                return false;
            }

            return AlcCode == productInfo.AlcCode;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EgaisProductInfo);
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            if (!AlcCode.IsNullOrEmpty())
            {
                hashCode ^= AlcCode.GetHashCode();
            }
            return hashCode;
        }

        public EgaisProductInfo DeepClone()
        {
            return Serializer.DeepClone(this);
        }

        #endregion
    }
}
