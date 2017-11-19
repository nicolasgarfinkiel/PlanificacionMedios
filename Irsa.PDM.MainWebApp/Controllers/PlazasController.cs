using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Entities;

namespace Irsa.PDM.MainWebApp.Controllers
{
    [Authorize]
    public class PlazasController : BaseController<PlazasAdmin, int, Entities.Plaza, Dtos.Plaza, FilterBase>
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
            };
        }
    }
}