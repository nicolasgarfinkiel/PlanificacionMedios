using System;
using System.Collections.Generic;
using System.Web.Services;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Common;

namespace Irsa.PDM.MainWebApp
{
    /// <summary>
    /// Summary description for PdmServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PdmServices : System.Web.Services.WebService
    {
        private AprobacionesSapAdmin _admin;

        public PdmServices()
        {
            _admin = new AprobacionesSapAdmin();
        }

        [WebMethod]
        public Result ConfirmacionSap(IList<ConfirmaionSap> confirmaciones)
        {
            var result = new Result() { HasErrors = false, Messages = new List<string>() };

            try
            {
                _admin.ConfirmarAprobacion(confirmaciones);
            }
            catch (Exception ex)
            {
                result.HasErrors = true;
                result.Messages.Add(ex.Message);
            }

            return result;
        }       
    }
}
