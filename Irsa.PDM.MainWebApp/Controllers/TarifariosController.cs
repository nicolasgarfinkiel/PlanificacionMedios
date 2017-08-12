using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Antlr.Runtime.Tree;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Dtos.Filters;
using Irsa.PDM.Infrastructure;
using OfficeOpenXml.ConditionalFormatting;

namespace Irsa.PDM.MainWebApp.Controllers
{
    //[Authorize(Roles = "Administracion")]
    public class TarifariosController : BaseController<TarifariosAdmin, int, Entities.Tarifario, Dtos.Tarifario, FilterTarifarios>
    {
        private readonly MediosAdmin _mediosAdmin;
        private readonly PlazasAdmin _plazasAdmin;
        private readonly VehiculosAdmin _vehiculosAdmin;
        private readonly TarifasAdmin _tarifasAdmin;

        public TarifariosController()
        {
            _mediosAdmin = new MediosAdmin();
            _plazasAdmin = new PlazasAdmin();
            _vehiculosAdmin = new VehiculosAdmin();
            _tarifasAdmin = new TarifasAdmin();
        }

        public ActionResult Index()
        {            
            return View();
        }

        public override object GetDataList()
        {
            _admin.SyncTablasBasicas();

            return new
            {
                Vehiculos = _vehiculosAdmin.GetAll().OrderBy(e => e.Nombre)
            };
        }

        public override object GetDataEdit()
        {
            return new
            {
                Medios = _mediosAdmin.GetAll().OrderBy(e => e.Nombre),
                Plazas = _plazasAdmin.GetAll().OrderBy(e => e.Codigo),
                Vehiculos = _vehiculosAdmin.GetAll().OrderBy(e => e.Nombre),
                Estados = _tarifasAdmin.GetEstadosList()
            };
        }

        [HttpPost]
        public ActionResult GetFechaDesde(int vehiculolId)
        {
            var response = new Response<DateTime> { Result = new Result() { HasErrors = false, Messages = new List<string>() } };

            try
            {
                response.Data = _admin.GetFechaDesde(vehiculolId);                    
            }
            catch (Exception ex)
            {
                response.Result.HasErrors = true;
                response.Result.Messages.Add(ex.Message);
            }

            return this.JsonNet(response);
        }

        [HttpPost]
        public ActionResult Aprobar(int tarifarioId)
        {
            var response = new Response<int> { Result = new Result() { HasErrors = false, Messages = new List<string>() } };

            try
            {
                _admin.Aprobar(tarifarioId);
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