using System.Collections.Generic;
using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Dtos.Filters;

namespace Irsa.PDM.MainWebApp.Controllers
{
    public class HomeController : Controller
    {     
        private readonly TarifariosAdmin _tarifariosAdmin;
        private readonly CampaniasAdmin _campaniasAdmin;
        private readonly CertificacionesAdmin _certificacionesAdmin;

        public HomeController()
        {           
            _tarifariosAdmin = new TarifariosAdmin();
            _campaniasAdmin = new CampaniasAdmin();
            _certificacionesAdmin = new CertificacionesAdmin();
        }

        public ActionResult Index()
        {
            var model = new Dashboard
            {
                TarifariosEditables = _tarifariosAdmin.GetByFilter(new FilterTarifarios { CurrentPage = 1, PageSize = 6, Estados = new List<string> { "Editable" } }).Data,
                TarifariosPendientesAprobacion = _tarifariosAdmin.GetByFilter(new FilterTarifarios { CurrentPage = 1, PageSize = 6, Estados = new List<string> { "PendienteAprobacion" } }).Data,
                CampaniasConInconsistencias = _campaniasAdmin.GetByFilter(new FilterCampanias { CurrentPage = 1, PageSize = 6, Estado = "InconsistenciasEnPautas" }).Data,
                CampaniasPendientesAprobacion = _campaniasAdmin.GetByFilter(new FilterCampanias { CurrentPage = 1, PageSize = 6, Estado = "Pendiente" }).Data,
                UltimasCertificaciones = _certificacionesAdmin.GetByFilter(new FilterCertificaciones{CurrentPage = 1, PageSize = 6}).Data,
            };

        //    new SapAdmin().Test();

            return View(model);
        }
    }
}