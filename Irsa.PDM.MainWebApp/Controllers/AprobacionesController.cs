using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos.Filters;

namespace Irsa.PDM.MainWebApp.Controllers
{
    //[Authorize(Roles = "Administracion")]
    public class AprobacionesController : BaseController<AprobacionesSapAdmin, int, Entities.AprobacionSap, Dtos.AprobacionSap, FilterAprobacionesSap>
    {       
        public AprobacionesController()
        {            
        }
        public ActionResult Index()
        {
            return View();
        }        

        public override object GetDataList()
        {          
            return new
            {
                //Estados = _admin.GetEstadosCertificaciones(),
            };
        }

        public override object GetDataEdit()
        {
            return new
            {
                AprobacionesPendientes = _admin.GetAprobacionesPendientes()
            };
        }
    }
}