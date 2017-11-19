using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Entities;

namespace Irsa.PDM.MainWebApp.Controllers
{
    [Authorize]
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
            return new
            {
                TipoEspacioList = _admin.GetTipoEspacioList()
            };
        }
    }
}