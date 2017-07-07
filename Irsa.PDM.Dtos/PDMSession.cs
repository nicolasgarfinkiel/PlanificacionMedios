using System;
using Irsa.PDM.Infrastructure;

namespace Irsa.PDM.Dtos
{
    public class PDMSession : SessionInfo<PDMSession>
    {
        public Usuario Usuario { get; set; }

        public System.Web.HttpPostedFileBase File { get; set; }

        public DateTime? LastSync { get; set; }

        public bool ShouldSync
        {
            get { return !LastSync.HasValue || (DateTime.Now - LastSync.Value).Minutes > 30; }
        }
    }
}