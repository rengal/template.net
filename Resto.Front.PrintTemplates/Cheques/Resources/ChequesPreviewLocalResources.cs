using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Cheques.Resources
{
    public partial class ChequesPreviewLocalResources
    {
        [CanBeNull]
        public static string TryGetResource([NotNull] string resourceCode)
        {
            if (resourceCode == null)
                throw new ArgumentNullException(nameof(resourceCode));

            return Localizer.TryGetStringFromResources(resourceCode);
        }
    }
}