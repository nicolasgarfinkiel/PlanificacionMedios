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

            var request = new DT_PDM_Consumos_In_Request
            {
                ZMMIM_CONMED_F001 = new List<DT_PDM_Consumos_In_RequestItem>
                {
                    new DT_PDM_Consumos_In_RequestItem
                    {
                        bank = "",
                        documentHeaderText = "",
                        idConsume  = "",
                        materialNumber = "",
                        plant = "",
                        quantity = "",
                        storageLocation = ""
                    }
                }.ToArray(),
                ZMMIM_CONMED_F002 = new List<DT_PDM_Consumos_In_RequestItem1>
                {
                    new DT_PDM_Consumos_In_RequestItem1
                    {
                        bank = "",
                        documentHeaderText = "",
                        idConsume  = "",
                        materialNumber = "",
                        plant = "",
                        quantity = "",
                        storageLocation = "",
                        orderNumber = ""
                    }
                }.ToArray(),
                ZMMIM_CONMED_F003 = new List<DT_PDM_Consumos_In_RequestItem2>
                {
                    new DT_PDM_Consumos_In_RequestItem2
                    {
                        bank = "",
                        documentHeaderText = "",
                        idConsume  = "",
                        materialNumber = "",                        
                        quantity = "",
                        orderNumber = "",
                        plant_D = "",
                        
                        
                    }
                }.ToArray(), 
               
            };

           service.SI_PDM_Consumos_In_Request(new DT_PDM_Consumos_In_Request());

          
            
        }    
    }
}
