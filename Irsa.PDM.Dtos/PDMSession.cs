using Irsa.PDM.Infrastructure;

namespace Irsa.PDM.Dtos
{
    public class PDMSession : SessionInfo<PDMSession>
    {
        public Usuario Usuario { get; set; }

        public System.Web.HttpPostedFileBase File { get; set; }
    }
}