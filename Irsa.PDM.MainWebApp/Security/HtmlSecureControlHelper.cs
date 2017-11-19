using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Security;
using Irsa.PDM.Infrastructure;

namespace Irsa.PDM.MainWebApp.Security
{
    public static partial class HtmlHelpers
    {
        private static Dictionary<SecureControlType, SecureControl> _controls;

        public static HtmlString SecureControl(this HtmlHelper htmlHelper, SecureControlType controlType, object model = null)
        {            
            if (_controls == null)
            {                
                InitializeControls(htmlHelper);
            }

            var secureControl = _controls[controlType];
            var userRoles = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);

            var content = userRoles.Intersect(secureControl.AllowedRoles).Any() ?
                     RazorEngineHelper.Render(@"SecuredControls\" + secureControl.SecuredPartial, model) : 
                     string.IsNullOrEmpty(secureControl.DefaultPartial) ? 
                     string.Empty :
                     RazorEngineHelper.Render(@"DefaultControls\" + secureControl.DefaultPartial, model);


            return new HtmlString(content);
        }

        private static void InitializeControls(HtmlHelper htmlHelper)
        {            
            _controls = new Dictionary<SecureControlType, SecureControl>
            {
                {SecureControlType.MediosCreate, new SecureControl{AllowedRoles = new List<string>{"medios_create"}, SecuredPartial = "BaseCreate" , DefaultPartial = string.Empty}},                
                {SecureControlType.MediosSave, new SecureControl{AllowedRoles = new List<string>{"medios_create", "medios_edit"}, SecuredPartial = "BaseSave" , DefaultPartial = string.Empty}},

                {SecureControlType.PlazasCreate, new SecureControl{AllowedRoles = new List<string>{"plazas_create"}, SecuredPartial = "BaseCreate" , DefaultPartial = string.Empty}},                
                {SecureControlType.PlazasSave, new SecureControl{AllowedRoles = new List<string>{"plazas_create", "plazas_edit"}, SecuredPartial = "BaseSave" , DefaultPartial = string.Empty}},

                {SecureControlType.VehiculosCreate, new SecureControl{AllowedRoles = new List<string>{"vehiculos_create"}, SecuredPartial = "BaseCreate" , DefaultPartial = string.Empty}},                
                {SecureControlType.VehiculosSave, new SecureControl{AllowedRoles = new List<string>{"vehiculos_create", "vehiculos_edit"}, SecuredPartial = "BaseSave" , DefaultPartial = string.Empty}},

                {SecureControlType.ProveedoresCreate, new SecureControl{AllowedRoles = new List<string>{"proveedores_create"}, SecuredPartial = "BaseCreate" , DefaultPartial = string.Empty}},                
                {SecureControlType.ProveedoresSave, new SecureControl{AllowedRoles = new List<string>{"proveedores_create", "proveedores_edit"}, SecuredPartial = "BaseSave" , DefaultPartial = string.Empty}},

                {SecureControlType.TarifariosListDashboard, new SecureControl{AllowedRoles = new List<string>{"tarifarios_list"}, SecuredPartial = "TarifariosListDashboard" , DefaultPartial = string.Empty}},
                {SecureControlType.TarifariosCreate, new SecureControl{AllowedRoles = new List<string>{"tarifarios_create"}, SecuredPartial = "TarifariosCreate" , DefaultPartial = string.Empty}},
                {SecureControlType.TarifariosCreatePorProveedor, new SecureControl{AllowedRoles = new List<string>{"tarifarios_create_por_proveedor"}, SecuredPartial = "TarifariosCreatePorProveedor" , DefaultPartial = string.Empty}},
                {SecureControlType.TarifariosEdit, new SecureControl{AllowedRoles = new List<string>{"tarifarios_edit"}, SecuredPartial = "TarifariosEdit" , DefaultPartial = string.Empty}},
                {SecureControlType.TarifariosEditDashboard, new SecureControl{AllowedRoles = new List<string>{"tarifarios_edit"}, SecuredPartial = "TarifariosEditDashboard" , DefaultPartial = string.Empty}},
                                
                {SecureControlType.CampaniasListDashboard, new SecureControl{AllowedRoles = new List<string>{"campanias_list"}, SecuredPartial = "CampaniasListDashboard" , DefaultPartial = string.Empty}},
                {SecureControlType.CampaniasEditDashboard, new SecureControl{AllowedRoles = new List<string>{"campanias_edit"}, SecuredPartial = "CampaniasEditDashboard" , DefaultPartial = string.Empty}},

                {SecureControlType.CertificacionesListDashboard, new SecureControl{AllowedRoles = new List<string>{"certificaciones_list"}, SecuredPartial = "CertificacionesListDashboard" , DefaultPartial = string.Empty}},
               
        };
      }
    }
}
