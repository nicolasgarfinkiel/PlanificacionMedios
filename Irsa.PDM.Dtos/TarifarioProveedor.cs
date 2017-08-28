using System;

namespace Irsa.PDM.Dtos
{
    public class TarifarioProveedor
    {        
        public Proveedor Proveedor { get; set; }        
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public double Importe { get; set; }
        public string Oc { get; set; }
        public string Documento { get; set; }  
    }
}
