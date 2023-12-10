using Resto.Common.Properties;

namespace Resto.Data
{
    partial class DocumentValidationResult
    {
        public string StatusText
        {
            get
            {
                return Valid
                           ? Resources.DocumentValidationResultValidCaption
                           : Warning
                                 ? Resources.DocumentValidationResultWarningCaption
                                 : Resources.DocumentValidationResultErrorCaption;
            }
        }
    }
}