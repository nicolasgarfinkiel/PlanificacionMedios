using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Dtos.Filters;
using Irsa.PDM.Infrastructure;
using Irsa.PDM.Infrastructure.ActionResults;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace Irsa.PDM.MainWebApp.Controllers
{
    //[Authorize(Roles = "Administracion")]
    public class CertificacionesController : BaseController<CertificacionesAdmin, int, Entities.Certificacion, Dtos.Certificacion, FilterBase>
    {

        public CertificacionesController()
        {        
        }

        public ActionResult Index()
        {
            return View();
        }

        public override object GetDataList()
        {
            if (PDMSession.Current.ShouldSyncCertificaciones) //TODO: mover llamada a  windows service
            {
                _admin.SyncCertificaciones();
                PDMSession.Current.LastSyncCertificaciones = DateTime.Now;
            }

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
         
        [HttpPost]
        public ActionResult GetExcel(FilterBase filter)
        {
            var excelPackage = _admin.GetExcel(filter);            

            return new ExcelResult
            {
                ExcelPackage = excelPackage,
                FileName = string.Format("Certificaciones_{0}.xlsx", DateTime.Now.ToString("dd_MM_yyyy"))
            };
        }
    }
}