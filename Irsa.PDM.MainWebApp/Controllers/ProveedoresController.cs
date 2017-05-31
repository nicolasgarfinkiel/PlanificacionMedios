using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos.Common;

namespace Irsa.PDM.MainWebApp.Controllers
{
    //[Authorize(Roles = "Administracion")]
    public class ProveedoresController : BaseController<ProveedoresAdmin, int, Entities.Proveedor, Dtos.Proveedor, FilterBase>
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
            return new
            {   
                Vehiculos  = new VehiculosAdmin().GetByFilter(new FilterBase{CurrentPage = 1, PageSize = 99999}).Data
            };
        }
    }
}