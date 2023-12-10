using System.Reflection;
using Resto.Framework.Localization;

namespace Resto.Common.Localization
{
    public static class RestoServerJournalEventsResourcesExtensions
    {
        private static readonly object syncObj = new object();

        private static Localizer localizer;
        private static Localizer Localizer
        {
            get
            {
                if (localizer == null)
                {
                    lock (syncObj)
                    {
                        if (localizer == null)
                        {
                            localizer = Localizer.Create("RestoEventsResources.resx",
                                "Resto.Common.Resources.RestoEventsResources",
                                Assembly.GetExecutingAssembly());
                        }
                    }
                }
                return localizer;
            }
        }

        public static string GetLocalName(this string key)
        {
            return Localizer.GetStringFromResources(key);
        }


    }
}
