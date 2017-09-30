using System;
using System.Collections.Generic;

namespace Irsa.PDM.Dtos
{
    public class Campania
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public DateTime CreateDate { get; set; }
        public IList<Pauta> Pautas { get; set; }
        public int? Centro { get; set; }
        public int? Almacen { get; set; }
        public int? Orden { get; set; }
        public int? CentroDestino { get; set; }
        public int? AlmacenDestino { get; set; }
        public int? IdSapDistribucion { get; set; }
    }
}
