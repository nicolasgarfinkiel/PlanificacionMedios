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
                    Description = "Alta Solicitud"
                },
                new Permission
                {
                    Description = "Visualizacion Solicitud"
                },
                new Permission
                {
                    Description = "Administracion"
                },
                new Permission
                {
                    Description = "Alta PDF Lotes Cartas de Porte"
                },
                new Permission
                {
                    Description = "Alta Solicitud"
                },
                new Permission
                {
                    Description = "Reservas"
                },
                new Permission
                {
                    Description = "Bandeja de Salida"
                },
                new Permission
                {
                    Description = "Confirmar Arribo"
                },
                new Permission
                {
                    Description = "Reportes"
                },
                new Permission
                {
                    Description = "Imprimir Solicitud"
                },
                new Permission
                {
                    Description = "Anular Solicitud"
                },
            };
        }
    }
}
