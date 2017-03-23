using System;
using System.Net;
using System.ServiceModel;
using System.Text;

namespace Irsa.PDM.Infrastructure.Services
{

	public static class WSHelpers
	{

		private static BasicHttpBinding _httpBinding;

		public static BasicHttpBinding HttpBinding
		{
			get
			{

				if(_httpBinding != null) return _httpBinding;

				_httpBinding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
				_httpBinding.MaxReceivedMessageSize = int.MaxValue;
				_httpBinding.ReceiveTimeout = TimeSpan.MaxValue;
				_httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

				return _httpBinding;

			}
		}

		public static T CallService<T>(string url, string message, NetworkCredential credentials) where T : class
		{
			var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
			request.ContentType = "text/xml; charset=utf-8";
			request.Method = "POST";

			request.Credentials = credentials;
			request.Headers["SOAPAction"] = "http://sap.com/xi/WebService/soap1.1";
            message = string.Format(@"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'><s:Body xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>{0}</s:Body></s:Envelope>", message);

			using (var requestStream = request.GetRequestStream())
			{
				var buffer = Encoding.UTF8.GetBytes(message);
				requestStream.Write(buffer, 0, message.Length);
			}

			using (var webResponse = (HttpWebResponse)request.GetResponse())
			using (var responseStream = webResponse.GetResponseStream())
			{
				return responseStream.DeserializeXML<T>();
			}

		}
	}
}
