using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class MenuTemplate
    {
        public string FullCaption
        {
            get
            {
                var caption = Resources.MenuTemplateFullCaption;
                if (!Name.IsNullOrWhiteSpace())
                {
                    caption += ": " + Name;
                }
                return caption;
            }
        }
    }
}
