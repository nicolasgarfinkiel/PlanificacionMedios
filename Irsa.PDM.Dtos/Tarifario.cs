using System;

namespace Irsa.PDM.Dtos
{
    public class Tarifario
    {
        public int? Id { get; set; }
        public Vehiculo Vehiculo { get; set; }        
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Estado { get; set; }
    }
}
