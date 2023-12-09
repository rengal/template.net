using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Resto.Framework.WcfCommon.Internationalization
{
    public sealed class InternationalizationBehavior : IServiceBehavior, IEndpointBehavior
    {
        private readonly string locale;

        public InternationalizationBehavior()
        {
        }

        public InternationalizationBehavior(string locale)
        {
            this.locale = locale;
        }

        #region Implementation of IServiceBehavior

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            ApplyInternationalizationMessageInspector(serviceHostBase);
        }

        public static void ApplyInternationalizationMessageInspector(ServiceHostBase serviceHostBase)
        {
            foreach (var endpointDispatcher in serviceHostBase.ChannelDispatchers
                .OfType<ChannelDispatcher>()
                .SelectMany(channelDispatcher => channelDispatcher.Endpoints))
            {
                endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new InternationalizationMessageInspector());
            }
        }

        #endregion

        #region Implementation of IEndpointBehavior

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new InternationalizationMessageInspector());
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new InternationalizationMessageInspector(locale));
        }

        #endregion
    }
}
