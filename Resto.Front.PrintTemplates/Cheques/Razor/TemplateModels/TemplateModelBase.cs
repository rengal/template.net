using System.Collections.Generic;
using System.Text.RegularExpressions;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;
using Resto.Front.Localization.Resources;
using Resto.Front.PrintTemplates.Cheques.Resources;

namespace Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels
{
    internal abstract class TemplateModelBase : Entity
    {
        private static readonly Regex ResourceIdPattern = new Regex(@"{\$([a-zA-Z\d]+)}", RegexOptions.Compiled);

        [CanBeNull, Transient]
        private Dictionary<string, string> fieldToValue;

        [CanBeNull]
        [ContractAnnotation("value:null => null; value:notnull => notnull")]
        protected string GetLocalizedValue([CanBeNull] string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (fieldToValue == null)
                fieldToValue = new Dictionary<string, string>();

            if (fieldToValue.TryGetValue(value, out var localizedString))
                return localizedString;

            localizedString = value;
            foreach (Match match in ResourceIdPattern.Matches(value))
            {
                var localizedResource = ChequesPreviewLocalResources.TryGetResource(match.Groups[1].Value);
                if (localizedResource != null)
                    localizedString = localizedString.Replace(match.Value, localizedResource);
            }

            fieldToValue.Add(value, localizedString);
            return localizedString;
        }
    }
}