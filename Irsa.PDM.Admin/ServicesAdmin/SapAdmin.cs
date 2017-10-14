using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Irsa.PDM.Admin.SI_PDM_Consumos_In_Request;
using Irsa.PDM.Entities;

namespace Irsa.PDM.Admin.ServicesAdmin
{
    public class SapAdmin
    {
        private static string _url = ConfigurationSettings.AppSettings["XIUrl"];
        private static string _user = ConfigurationSettings.AppSettings["XIUser"];
        private static string _pass = ConfigurationSettings.AppSettings["XIPassword"];

        public void CreateConsumo(IList<AprobacionSap> aprobaciones )
        {
            #region Init

            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => { return true; };

            var service = new SI_PDM_Consumos_In_RequestService
            {
                Credentials = new System.Net.NetworkCredential(_user, _pass),
                Url = _url                
            };

            #endregion
          
            var request = new DT_PDM_Consumos_In_Request
            {
                ZMMIM_CONMED_F001 = aprobaciones.Select(e => new DT_PDM_Consumos_In_RequestItem
                {
                    idConsume = e.Id.ToString(),
                    bank = e.Campania.IdSapDistribucion.ToString(),
                    documentHeaderText = e.Campania.Nombre,                    
                    materialNumber = e.Campania.Pautas[0].Items[0].Tarifa.Tarifario.NumeroProveedorSap,
                    plant = e.Campania.Centro.ToString(),
                    quantity = e.MontoTotal.ToString(),
                    storageLocation = e.Campania.Almacen.ToString()                    
                }).ToArray(),
                ZMMIM_CONMED_F002 = aprobaciones.Select(e => new DT_PDM_Consumos_In_RequestItem1
                {
                    idConsume = e.Id.ToString(),
                    bank = e.Campania.IdSapDistribucion.ToString(),
                    documentHeaderText = e.Campania.Nombre,
                    materialNumber = e.Campania.Pautas[0].Items[0].Tarifa.Tarifario.NumeroProveedorSap,
                    plant = e.Campania.Centro.ToString(),
                    quantity = e.MontoTotal.ToString(),
                    storageLocation = e.Campania.Almacen.ToString(),
                    orderNumber = e.Campania.Orden.ToString()
                }).ToArray(),
                ZMMIM_CONMED_F003 = aprobaciones.Select(e => new DT_PDM_Consumos_In_RequestItem2
                {
                    idConsume = e.Id.ToString(),
                    bank = e.Campania.IdSapDistribucion.ToString(),
                    documentHeaderText = e.Campania.Nombre,
                    materialNumber = e.Campania.Pautas[0].Items[0].Tarifa.Tarifario.NumeroProveedorSap,                    
                    quantity = e.MontoTotal.ToString(),                    
                    orderNumber = e.Campania.Orden.ToString(),
                    plant_D = e.Campania.CentroDestino.ToString(),
                    plant_O = e.Campania.Centro.ToString(),
                    purchasingDocumentType = "ZICV",
                    purchasingGroup = "200",
                    purchasingOrganization = "9999",
                    storageLocation_D = e.Campania.AlmacenDestino.ToString(),
                    storageLocation_O = e.Campania.Almacen.ToString()
                    
                }).ToArray(),                                                                                             
            };

            service.SI_PDM_Consumos_In_Request(request);                      
        }

        public void CreateProvision(List<AprobacionSap> entities)
        {          
        }

        public void CreateCertificacion(List<AprobacionSap> entities)
        {            
        }
    }
}
