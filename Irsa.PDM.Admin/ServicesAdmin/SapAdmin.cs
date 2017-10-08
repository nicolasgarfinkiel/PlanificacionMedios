using System.Collections.Generic;
using System.Configuration;
using Irsa.PDM.Admin.SI_PDM_Consumos_In_Request;


namespace Cresud.CDP.Admin.ServicesAdmin
{
    public class SapAdmin
    {
        private static string _url = ConfigurationSettings.AppSettings["XIUrl"];
        private static string _user = ConfigurationSettings.AppSettings["XIUser"];
        private static string _pass = ConfigurationSettings.AppSettings["XIPassword"];

        public void Test()
        {
            #region Init

            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => { return true; };

            var service = new SI_PDM_Consumos_In_RequestService
            {
                Credentials = new System.Net.NetworkCredential(_user, _pass),
                Url = _url                
            };

//            service.clie

            #endregion
            

           service.SI_PDM_Consumos_In_Request(new DT_PDM_Consumos_In_Request());

           service.SI_PDM_Consumos_In_RequestCompleted += (sender, args) =>
           {
               var j = 0;
           };
            
        }    
    }
}
