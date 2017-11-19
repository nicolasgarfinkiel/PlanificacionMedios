using System;
using System.IO;
using System.Web;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Irsa.PDM.Infrastructure
{
    public class RazorEngineHelper
    {
        public static string Render(string template, object model)
        {
            string result;
            using (var cacheProvidxer = new InvalidatingCachingProvider())
            {
                var config = new TemplateServiceConfiguration
                {
                    CachingProvider = cacheProvidxer,                    
                    BaseTemplateType = typeof(WebApiTemplateBase<>)
                };

                using (var service = RazorEngineService.Create(config))
                {
                    result = service.RunCompile(GetTemplate(template), Guid.NewGuid().ToString(), null, model);
                    cacheProvidxer.InvalidateAll();
                }
            }
            return result;
        }

        public static string GetTemplate(string templateName)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var fileName = string.Format("{0}.cshtml", templateName);
            var path = Path.Combine(basePath, "Views", "Shared", fileName);

            var template = templateName;
            if (System.IO.File.Exists(path))
            {
                template = System.IO.File.ReadAllText(path);
            }

            return template;
        }        
      
    }

    public abstract class WebApiTemplateBase<T> : TemplateBase<T>
    {
        protected WebApiTemplateBase()
        {
            Url = new UrlHelper();
        }

        public UrlHelper Url;
    }

    public class UrlHelper
    {
        public string Content(string content)
        {
            return VirtualPathUtility.ToAbsolute(content);
        }
    }
}