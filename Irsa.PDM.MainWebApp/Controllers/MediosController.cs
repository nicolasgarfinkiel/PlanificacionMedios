using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos.Common;

namespace Irsa.PDM.MainWebApp.Controllers
{
    //[Authorize(Roles = "Administracion")]
    public class MediosController : BaseController<MediosAdmin, int, Entities.Medio, Dtos.Medio, FilterBase>
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