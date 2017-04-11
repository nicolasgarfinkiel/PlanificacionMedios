using System.Linq;
using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos.Filters;

namespace Irsa.PDM.MainWebApp.Controllers
{
    //[Authorize(Roles = "Administracion")]
    public class TarifariosController : BaseController<TarifariosAdmin, int, Entities.Tarifario, Dtos.Tarifario, FilterTarifarios>
    {        
        public TarifariosController()
        {            
        }

        public ActionResult Index()
        {
            return View();
        }

        public override object GetDataList()
        {
            return new { };       
        }

        public override object GetDataEdit()
        {
            return new
            {              
            };               
        }
    }
}