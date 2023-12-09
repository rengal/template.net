using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Data
{
    public interface IEntitiesProviderFactory
    {
        [NotNull]
        IEntitiesProvider Create();
    }
}