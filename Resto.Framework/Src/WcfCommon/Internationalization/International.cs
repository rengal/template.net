using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Resto.Framework.WcfCommon.Internationalization
{
    [DataContract(Name=WSI18N.ElementNames.International, Namespace=WSI18N.NamespaceURI)]  
    public class International
    {
        [DataMember(Name=WSI18N.ElementNames.Locale)]
        public string Locale { get; set; }

        [DataMember(Name = WSI18N.ElementNames.TZ)]
        public string Tz { get; set; }

        [DataMember(Name=WSI18N.ElementNames.Preferences)]
        public List<Preferences> Preferences { get; set; }

        public static International GetHeaderFromIncomeMessage()
        {
            var headers = OperationContext.Current.IncomingMessageHeaders;

            return headers.UnderstoodHeaders
                .Where(uheader => uheader.Name == WSI18N.ElementNames.International && uheader.Namespace == WSI18N.NamespaceURI)
                .Select(uheader => headers.GetHeader<International>(uheader.Name, uheader.Namespace))
                .FirstOrDefault();
        }
    }

    
}
