using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Reports.TemplateModels
{
    public interface ISettings
    {
        [NotNull]
        object GetValue([NotNull] string name);
    }
}