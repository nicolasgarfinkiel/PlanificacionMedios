using System;
using Irsa.PDM.Dtos.Common;

namespace Irsa.PDM.Dtos.Filters
{
    public class FilterTarifarios : FilterBase
    {
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public Vehiculo Vehiculo { get; set; }

    }
}
