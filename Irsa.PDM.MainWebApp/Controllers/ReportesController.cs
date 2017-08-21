using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Infrastructure.ActionResults;
using System;

namespace Irsa.PDM.MainWebApp.Controllers
{
    //[Authorize(Roles = "Administracion")]
    public class ReportesController : Controller
    {
        private readonly CampaniasAdmin _campaniasAdmin;

        public ReportesController()
        {
            _campaniasAdmin = new CampaniasAdmin();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetExcelReporteDePauta(int pautaId)
        {
            var excelPackage = _campaniasAdmin.GetExcelReporteDePauta(pautaId);

            return new ExcelResult
            {
                ExcelPackage = excelPackage,
                FileName = string.Format("Reporte_de_pauta_{0}.xlsx", DateTime.Now.ToString("dd_MM_yyyy"))
            };
        }

        [HttpPost]
        public ActionResult GetExcelVisualDePauta(int pautaId)
        {
            var excelPackage = _campaniasAdmin.GetExcelVisualDePauta(pautaId);

            return new ExcelResult
            {
                ExcelPackage = excelPackage,
                FileName = string.Format("Reporte_visual_de_pauta_{0}.xlsx", DateTime.Now.ToString("dd_MM_yyyy"))
            };
        }
    }
}