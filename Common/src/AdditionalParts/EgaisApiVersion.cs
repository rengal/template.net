using System.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public partial class EgaisApiVersion
    {
        public static bool TryParse(string value, out EgaisApiVersion version)
        {
            switch (value)
            {
                case "V1":
                case "WayBill":
                {
                    version = V1;
                    return true;
                }
                case "V2":
                case "WayBill_v2":
                {
                    version = V2;
                    return true;
                }
                case "V3":
                case "WayBill_v3":
                {
                    version = V3;
                    return true;
                }
                case "V4":
                case "WayBill_v4":
                {
                    version = V4;
                    return true;
                }
                default:
                {
                    version = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Пытается получить объект <see cref="EgaisApiVersion"/> по строке
        /// </summary>
        /// <returns>версия в формате <see cref="EgaisApiVersion"/> либо null</returns>
        [CanBeNull]
        public static EgaisApiVersion ParseNullable(string oldVersion)
        {
            EgaisApiVersion version;
            return TryParse(oldVersion, out version)
                ? version
                : null;
        }

        /// <summary>
        /// Возвращает самую свежую версию
        /// </summary>
        public static EgaisApiVersion LatestVersion
        {
           get { return VALUES.Last(); }
        }
    }
}
