using System.Linq;
using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class KladrObject
    {
        /// <summary>
        /// Регион города из КЛАДР (например, "Москва (город)", "Самарская (область)")
        /// </summary>
        public string Region
        {
            get
            {
                if (Parents.Count == 0)
                    return null;

                return string.Join(", ", Parents.Select(
                    parent => string.Format(Resources.KladrCitiesCompletionModelRegionStringFormat,
                                            parent.Name, parent.Type.ToLower())));
            }
        }
    }
}