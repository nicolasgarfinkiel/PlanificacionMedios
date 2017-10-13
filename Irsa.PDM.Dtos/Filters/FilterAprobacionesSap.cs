using System;
using Irsa.PDM.Dtos.Common;

namespace Irsa.PDM.Dtos.Filters
{
    public class FilterAprobacionesSap : FilterBase
    {
        public Campania Campania { get; set; }
        public Proveedor Proveedor { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }
}
