using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Formatting = System.Xml.Formatting;

namespace Irsa.PDM.Infrastructure
{
    public class JsonNetResult : JsonResult
    {
        public JsonSerializerSettings SerializerSettings { get; set; }

        public JsonNetResult()
            : base()
        {
            // create serializer settings
            this.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,

#if DEBUG
                Formatting = (Newtonsoft.Json.Formatting) Formatting.Indented,
#endif
            };

            // setup default serializer settings
            this.SerializerSettings.Converters.Add(new IsoDateTimeConverter());
            this.SerializerSettings.Converters.Add(new StringEnumConverter());
        }

        public override void ExecuteResult(ControllerContext context)
        {
            // verify we have a contrex
            if (context == null)
                throw new ArgumentNullException("context");

            // get the current http context response
            var response = context.HttpContext.Response;
            // set content type of response
            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
            // set content encoding of response
            if (ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;

            // verify this response has data
            if (this.Data == null)
                return;

            // serialize the object to JSON using JSON.Net
            String JSONText = JsonConvert.SerializeObject(Data, SerializerSettings);


            // write the response
            response.Write(JSONText);
        }
    }
}
