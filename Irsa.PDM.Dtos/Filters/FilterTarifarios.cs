using System;
using System.Collections.Generic;
using Irsa.PDM.Dtos.Common;

namespace Irsa.PDM.Dtos.Filters
{
    public class FilterTarifarios : FilterBase
    {
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public Vehiculo Vehiculo { get; set; }
        public IList<string> Estados { get; set; }

  }
}
