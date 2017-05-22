using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Dtos.Filters;
using Irsa.PDM.Infrastructure;

namespace Irsa.PDM.MainWebApp.Controllers
{
    //[Authorize(Roles = "Administracion")]
    public class CampaniasController : BaseController<CampaniasAdmin, int, Entities.Campania, Dtos.Campania, FilterBase>
    {
        
        public CampaniasController()
        {        
        }

        public ActionResult Index()
        {
            return View();
        }

        public override object GetDataList()
        {            
            _admin.SyncCampanias(); //TODO: mover llamada a  windows service

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
              response.Data =  _admin.ChangeEstadoPauta(pautaId, estado, motivo);
            }
            catch (Exception ex)
            {
                response.Result.HasErrors = true;
                response.Result.Messages.Add(ex.Message);
            }

            return this.JsonNet(response);
        }     
    }
}