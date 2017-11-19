using System.Collections.Generic;
using System.Web.Services;

namespace Irsa.PDM.SecurityServiceTest
{
    /// <summary>
    /// Summary description for Security
    /// </summary>
    [WebService(Namespace = "http://framework.irsa.com.ar/WebServices/Security/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Security : System.Web.Services.WebService
    {
        [WebMethod]
        public UserLogonByNameResult UserLogonByName(string ntUserName, int idApplication)
        {
            return new UserLogonByNameResult();
        }

        [WebMethod]
        public List<Group> GroupsListPerUser(UserLogonByNameResult user, int idApplication)
        {
            return new List<Group>
            {
                new Group
                {
                    
                }
            };
        }

        [WebMethod]
        public List<Permission> PermissionListPerGroup(Group group)
        {            
            return new List<Permission>
            {
                new Permission
                {
                    Description = "medios_list"
                },
                new Permission
                {
                    Description = "medios_create"
                },
                new Permission
                {
                    Description = "medios_edit"
                },
                new Permission
                {
                    Description = "plazas_list"
                },
                 new Permission
                {
                    Description = "plazas_create"
                },
                 new Permission
                {
                    Description = "plazas_edit"
                },
                new Permission
                {
                    Description = "vehiculos_list"
                },
                new Permission
                {
                    Description = "vehiculos_create"
                },
                new Permission
                {
                    Description = "vehiculos_edit"
                },
                new Permission
                {
                    Description = "proveedores_list"
                },
                new Permission
                {
                    Description = "proveedores_create"
                },
                new Permission
                {
                    Description = "proveedores_edit"
                },
                new Permission
                {
                    Description = "tarifarios_list"
                },    
                new Permission
                {
                    Description = "tarifarios_create"
                }, 
                new Permission
                {
                    Description = "tarifarios_create_por_proveedor"
                }, 
                new Permission
                {
                    Description = "tarifarios_edit"
                },      
                new Permission
                {
                    Description = "tarifarios_delete"
                },    
                new Permission
                {
                    Description = "tarifarios_admin"
                },    
                new Permission
                {
                    Description = "tarifarios_aprobacion"
                },                                    
                new Permission
                {
                    Description = "campanias_list"
                },
               new Permission
                {
                    Description = "campanias_create"
                },
                new Permission
                {
                    Description = "campanias_edit"
                },
                new Permission
                {
                    Description = "campanias_reports"
                },                
                new Permission
                {
                    Description = "certificaciones_list"
                },
                new Permission
                {
                    Description = "certificaciones_aprobaciones"
                },
               
            };
        }
    }
}
