using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos.Common;

namespace Irsa.PDM.MainWebApp.Controllers
{
    [Authorize]
    public class VehiculosController : BaseController<VehiculosAdmin, int, Entities.Vehiculo, Dtos.Vehiculo, FilterBase>
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