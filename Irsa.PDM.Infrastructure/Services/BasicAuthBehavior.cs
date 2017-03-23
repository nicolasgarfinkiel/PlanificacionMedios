using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Irsa.PDM.Infrastructure.Services
{
    public class BasicAuthBehavior : IEndpointBehavior
    {
        public string Password { get; set; }
        public string Username { get; set; }

        public BasicAuthBehavior(string username, string password)
        {
            Username = username;
            Password = password;
        }
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {           
            var inspector = new BasicAuthInspector(Username, Password);
            clientRuntime.MessageInspectors.Add(inspector);
        }
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) { }
        public void Validate(ServiceEndpoint endpoint) { }
    }

    public class BasicAuthInspector : IClientMessageInspector
    {
        public string Password { get; set; }
        public string Username { get; set; }

        public BasicAuthInspector(string username, string password)
        {
            Username = username;
            Password = password;
        }
        public void AfterReceiveReply(ref Message reply, object correlationState) { }
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            
            // we add the headers manually rather than using credentials 
            // due to proxying issues, and with the 101-continue http verb 
            var authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(Username + ":" + Password));
            var messageProperty = new HttpRequestMessageProperty();
            messageProperty.Headers.Add("Authorization", "Basic " + authInfo);
            request.Properties[HttpRequestMessageProperty.Name] = messageProperty;
            return null;
        }
    }
}