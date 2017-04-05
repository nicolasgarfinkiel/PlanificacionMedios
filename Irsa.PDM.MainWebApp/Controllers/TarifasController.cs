using System.Linq;
using System.Web.Mvc;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos.Filters;

namespace Irsa.PDM.MainWebApp.Controllers
{
    //[Authorize(Roles = "Administracion")]
    public class TarifasController : BaseController<TarifasAdmin, int, Entities.Tarifa, Dtos.Tarifa, FilterTarifas>
    {
        private readonly MediosAdmin _mediosAdmin;
        private readonly PlazasAdmin _plazasAdmin;
        private readonly VehiculosAdmin _vehiculosAdmin;

        public TarifasController()
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
                Plazas = _plazasAdmin.GetAll().OrderBy(e => e.Nombre),
                Vehiculos = _vehiculosAdmin.GetAll().OrderBy(e => e.Nombre),
            };               
        }
    }
}