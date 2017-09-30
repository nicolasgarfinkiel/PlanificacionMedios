using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace Irsa.PDM.Entities
{
    public class Campania : EntityBase
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public EstadoCampania Estado { get; set; }
        public virtual IList<Pauta> Pautas { get; set; }
        public DateTime? FechaCierre { get; set; }
        public int? Centro { get; set; }
        public int? Almacen { get; set; }
        public int? Orden { get; set; }
        public int? CentroDestino { get; set; }
        public int? AlmacenDestino { get; set; }
        public int? IdSapDistribucion { get; set; }
    }
}
