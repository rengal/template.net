using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Threading;

namespace Resto.Framework.WcfCommon.Internationalization
{
    public sealed class InternationalizationMessageInspector : IClientMessageInspector, IDispatchMessageInspector
    {
        private readonly string locale;
        private readonly string timeZone;

        public InternationalizationMessageInspector()
        {
        }

        public InternationalizationMessageInspector(string locale)
        {
            this.locale = locale;
        }

        public InternationalizationMessageInspector(string locale, string timeZone)
        {
            this.locale = locale;
            this.timeZone = timeZone;
        }

        #region IClientMessageInspector Members

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var internationalHeader = new International
                                          {
                                              Locale = !string.IsNullOrEmpty(locale) 
                                                ? locale
                                                : Thread.CurrentThread.CurrentUICulture.Name,
                                          };


            if (!string.IsNullOrEmpty(timeZone))
                internationalHeader.Tz = timeZone;

            var header = MessageHeader.CreateHeader(WSI18N.ElementNames.International, WSI18N.NamespaceURI, internationalHeader);
            request.Headers.Add(header);
            return null;
        }

        #endregion

        #region IDispatchMessageInspector Members
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var index = request.Headers.FindHeader(WSI18N.ElementNames.International, WSI18N.NamespaceURI);
            if (index >= 0)
            {
                request.Headers.UnderstoodHeaders.Add(request.Headers[index]);
                var header = request.Headers.GetHeader<International>(index);
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(header.Locale);
            }
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        #endregion
    }
}
