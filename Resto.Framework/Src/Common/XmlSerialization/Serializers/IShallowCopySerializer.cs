using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal interface IShallowCopySerializer
    {
        void ShallowCopy([NotNull] object from, [NotNull] object to);
    }
}