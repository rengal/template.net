using Resto.Framework.Data;

namespace Resto.Data
{
    partial class EgaisOrganizationEntity : IDeletable
    {
        /// <summary>
        /// ИНН, в соответствии с версией информации об организации
        /// </summary>
        public string Inn
        {
            get
            {
                return orgInfoV2 != null
                    ? orgInfoV2.Inn
                    : orgInfoV1 != null
                    ? orgInfoV1.Inn
                    : string.Empty;
            }
        }

        /// <summary>
        /// КПП, в соответствии с версией информации об организации
        /// </summary>
        public string Kpp
        {
            get
            {
                return orgInfoV2 != null
                    ? orgInfoV2.Kpp
                    : orgInfoV1 != null
                    ? orgInfoV1.Kpp
                    : string.Empty;
            }
        }

        /// <summary>
        /// Краткое наименование, в соответствии с версией информации об организации
        /// </summary>
        public string ShortName
        {
            get
            {
                return orgInfoV2 != null
                    ? orgInfoV2.ShortName
                    : orgInfoV1 != null
                    ? orgInfoV1.ShortName
                    : string.Empty;
            }
        }

        /// <summary>
        /// Адрес, в соответствии с версией информации об организации
        /// </summary>
        public EgaisAddress Address
        {
            get
            {
                return orgInfoV2 != null
                    ? orgInfoV2.Address
                    : orgInfoV1 != null
                    ? orgInfoV1.Address
                    : new EgaisAddress();
            }
        }
    }
}
