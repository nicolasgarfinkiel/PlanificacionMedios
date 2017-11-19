using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Dtos.Filters;
using Irsa.PDM.Infrastructure;

namespace Irsa.PDM.MainWebApp.Controllers
{
    [Authorize]
    public class TarifasController : BaseController<TarifasAdmin, int, Entities.Tarifa, Dtos.Tarifa, FilterTarifas>
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

        [HttpPost]
        public ActionResult SetValues(FilterTarifas filter, double? importe, int? oc)
        {
            var response = new Response<int> { Result = new Result() { HasErrors = false, Messages = new List<string>() } };

            try
            {
                _admin.SetValues(filter, importe, oc);
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