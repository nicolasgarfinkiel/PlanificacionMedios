using System;
using System.Collections.Generic;

namespace Irsa.PDM.Dtos
{
    public class TarifarioEdit
    {
        public int? Id { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public IList<Tarifa> Tarifas { get; set; }
        public bool Editable { get; set; }
    }
}
