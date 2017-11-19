using System.Collections.Generic;

namespace Irsa.PDM.MainWebApp.Security
{
    public class SecureControl
    {        
        public IList<string> AllowedRoles  { get; set; }        
        public string SecuredPartial { get; set; }
        public string DefaultPartial { get; set; }
    }
}