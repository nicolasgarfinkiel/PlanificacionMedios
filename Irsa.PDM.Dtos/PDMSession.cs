using System;
using Irsa.PDM.Infrastructure;

namespace Irsa.PDM.Dtos
{
    public class PDMSession : SessionInfo<PDMSession>
    {
        public Usuario Usuario { get; set; }

        public System.Web.HttpPostedFileBase File { get; set; }

        public DateTime? LastSyncCampanias { get; set; } //TODO: quitar cuando se desarrolle windows service
        public DateTime? LastSyncCertificaciones { get; set; } //TODO: quitar cuando se desarrolle windows service

        public bool ShouldSyncCampanias //TODO: quitar cuando se desarrolle windows service
        {
            get
            {
                return !LastSyncCampanias.HasValue || (DateTime.Now - LastSyncCampanias.Value).Minutes > 30;
            }
        }

        public bool ShouldSyncCertificaciones //TODO: quitar cuando se desarrolle windows service
        {
            get
            {
                return !LastSyncCertificaciones.HasValue || (DateTime.Now - LastSyncCertificaciones.Value).Minutes > 30;
            }
        }
    }
}