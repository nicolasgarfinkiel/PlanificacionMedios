using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Dtos.Filters;
using Irsa.PDM.Infrastructure;

namespace Irsa.PDM.MainWebApp.Controllers
{
    //[Authorize(Roles = "Administracion")]
    public class TarifariosController : BaseController<TarifariosAdmin, int, Entities.Tarifario, Dtos.Tarifario, FilterTarifarios>
    {
        private readonly MediosAdmin _mediosAdmin;
        private readonly PlazasAdmin _plazasAdmin;
        private readonly VehiculosAdmin _vehiculosAdmin;

        public TarifariosController()
        {
            _mediosAdmin = new MediosAdmin();
            _plazasAdmin = new PlazasAdmin();
            _vehiculosAdmin = new VehiculosAdmin();
        }

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
                Medios = _mediosAdmin.GetAll().OrderBy(e => e.Nombre),
                Plazas = _plazasAdmin.GetAll().OrderBy(e => e.Codigo),
                Vehiculos = _vehiculosAdmin.GetAll().OrderBy(e => e.Nombre),
            };
        }

           [HttpPost]
        public ActionResult GetFechaDesde()
        {
            var response = new Response<DateTime> { Result = new Result() { HasErrors = false, Messages = new List<string>() } };

            try
            {
                response.Data = _admin.GetFechaDesde();                    
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