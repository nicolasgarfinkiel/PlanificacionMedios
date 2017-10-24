using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Dtos.Filters;
using Irsa.PDM.Infrastructure;
using Irsa.PDM.Infrastructure.ActionResults;

namespace Irsa.PDM.MainWebApp.Controllers
{
    //[Authorize(Roles = "Administracion")]
    public class SapController : BaseController<CampaniasAdmin, int, Entities.Campania, Dtos.Campania, FilterCampanias>
    {

        public SapController()
        {        
        }

        public ActionResult Index()
        {
            return View();
        }

        public override object GetDataList()
        {
            if (PDMSession.Current.ShouldSyncCampanias) //TODO: mover llamada a  windows service
            {
                _admin.SyncCampanias();
                PDMSession.Current.LastSyncCampanias = DateTime.Now;
            }

            return new
            {               
                Estados = _admin.GetEstadosCampania(),
                DistribucionList = _admin.GetDistribucionList()
            };
        }

        public override object GetDataEdit()
        {
            return new
            {
                DistribucionList = _admin.GetDistribucionList()
            };
        }

        [HttpPost]
        public ActionResult GetPautasByFilter(FilterPautaItems filter)
        {
            var response = new PagedListResponse<PautaDetail>();

            try
            {
                response = _admin.GetPautasByFilter(filter);
            }
            catch (Exception ex)
            {
                response.Result.HasErrors = true;
                response.Result.Messages.Add(ex.Message);
            }

            return this.JsonNet(response);
        }
  
        [HttpPost]
        public ActionResult GetItemsByFilter(FilterPautaItems filter)
        {
            var response = new PagedListResponse<PautaItem>();

            try
            {
                response = _admin.GetItemsByFilter(filter);
            }
            catch (Exception ex)
            {
                response.Result.HasErrors = true;
                response.Result.Messages.Add(ex.Message);
            }

            return this.JsonNet(response);
        }

        [HttpPost]
        public ActionResult ChangeEstadoPauta(int pautaId, string estado, string motivo)
        {
            var response = new Response<string> { Result = new Result() { HasErrors = false, Messages = new List<string>() } };

            try
            {
                response.Data = _admin.ChangeEstadoPauta(pautaId, estado, motivo);
            }
            catch (Exception ex)
            {
                response.Result.HasErrors = true;
                response.Result.Messages.Add(ex.Message);
            }

            return this.JsonNet(response);
        }     

        [HttpPost]
        public ActionResult ChangeEstadoCampania(Campania campania, string estado, string motivo)
        {
            var response = new Response<string> { Result = new Result() { HasErrors = false, Messages = new List<string>() } };

            try
            {
                _admin.ChangeEstadoCampania(campania, estado, motivo);
            }
            catch (Exception ex)
            {
                response.Result.HasErrors = true;
                response.Result.Messages.Add(ex.Message);
            }

            return this.JsonNet(response);
        }

        [HttpPost]
        public ActionResult GetExcelVisualDePauta(int campaniaId)
        {
            var excelPackage = _admin.GetExcelVisualDePauta(campaniaId);            

            return new ExcelResult
            {
                ExcelPackage = excelPackage,
                FileName = string.Format("Visual_de_Pauta.xlsx")
            };
        }

        //[HttpPost]
        //public ActionResult GetPdfVisualDePauta(int campaniaId)
        //{         
        //    var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri)
        //    {
        //        Path = Url.Content("~/Content/images/logo-irsa.png"),
        //        Query = null,
        //    };

        //    var imageUrl = urlBuilder.ToString();
        //    var model = new { ImageUrl = imageUrl };

        //    var html = GetHtml("Test", model);
        //    var pdf = new  PdfGenerator(html).Generate();

        //    return new PdfResult
        //    {
        //        Content = pdf,
        //        FileName = string.Format("Visual_de_Pauta.pdf")
        //    };
        //}
    }
}