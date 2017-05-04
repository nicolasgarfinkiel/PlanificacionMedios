using System;
using System.Collections.Generic;

namespace Irsa.PDM.Entities
{
    public class Tarifario : EntityBase
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public virtual Vehiculo Vehiculo { get; set; }
        public virtual IList<Tarifa> Tarifas { get; set; }
    }
}
