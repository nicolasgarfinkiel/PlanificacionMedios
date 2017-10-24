using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Dtos.Filters;
using Irsa.PDM.Infrastructure;

namespace Irsa.PDM.MainWebApp.Controllers
{
    //[Authorize(Roles = "Administracion")]
    public class AprobacionesController : BaseController<AprobacionesSapAdmin, int, Entities.AprobacionSap, Dtos.AprobacionSap, FilterAprobacionesSap>
    {       
        public AprobacionesController()
        {            
        }
        public ActionResult Index()
        {
            return View();
        }        

        public override object GetDataList()
        {          
            return new
            {
                //Estados = _admin.GetEstadosCertificaciones(),
            };
        }

        public override object GetDataEdit()
        {
            return new
            {
                AprobacionesPendientes = _admin.GetAprobacionesPendientes()
            };
        }

        [HttpPost]
        public ActionResult ConfirmacionSap(IList<ConfirmaionSap> confirmaciones )
        {
            var result =  new Result() { HasErrors = false, Messages = new List<string>() } ;

            try
            {
                _admin.ConfirmarAprobacion(confirmaciones);
            }
            catch (Exception ex)
            {
                result.HasErrors = true;
                result.Messages.Add(ex.Message);
            }

            return this.JsonNet(result);
        }       
    }
}