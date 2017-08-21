using System;
using Irsa.PDM.Dtos.Common;

namespace Irsa.PDM.Dtos.Filters
{
    public class FilterCampanias : FilterBase
    {
        public string Estado { get; set; }        
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

    }
}
