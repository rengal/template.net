using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization
{
    internal interface IRootEntityFactory<out TRootEntityBaseType> where TRootEntityBaseType : Entity, IRootEntity
    {
        [NotNull]
        TRootEntityBaseType Create();
    }
}