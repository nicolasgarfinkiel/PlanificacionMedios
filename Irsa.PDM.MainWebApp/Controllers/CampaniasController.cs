using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos.Common;

namespace Irsa.PDM.MainWebApp.Controllers
{
    //[Authorize(Roles = "Administracion")]
    public class CampaniasController : BaseController<CampaniasAdmin, int, Entities.Campania, Dtos.Campania, FilterBase>
    {
        
        public CampaniasController()
        {        
        }

        public ActionResult Index()
        {
            return View();
        }

        public override object GetDataList()
        {            
            _admin.SyncCampanias(); //TODO: mover llamada a  windows service

            return new
            {               
            };
        }

        public override object GetDataEdit()
        {
            return new
            {                
            };
        }     
    }
}