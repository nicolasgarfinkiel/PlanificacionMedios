using System;
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
                var config = new TemplateServiceConfiguration { CachingProvider = cacheProvidxer };
                using (var service = RazorEngineService.Create(config))
                {
                    result = service.RunCompile(template, Guid.NewGuid().ToString(), null, model);
                    cacheProvidxer.InvalidateAll();
                }
            }
            return result;
        }
      
    }
}