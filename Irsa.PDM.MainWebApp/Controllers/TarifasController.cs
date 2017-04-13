using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos.Filters;

namespace Irsa.PDM.MainWebApp.Controllers
{
    //[Authorize(Roles = "Administracion")]
    public class TarifasController : BaseController<TarifasAdmin, int, Entities.Tarifa, Dtos.Tarifa, FilterTarifas>
    {       
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
            return new { };  
        }
    }
}