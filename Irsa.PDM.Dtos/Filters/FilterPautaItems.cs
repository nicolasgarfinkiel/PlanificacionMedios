using System;
using Irsa.PDM.Dtos.Common;

namespace Irsa.PDM.Dtos.Filters
{
    public class FilterPautaItems : FilterBase
    {
        public int CampaniaCodigo { get; set; }
        public int? PautaId { get; set; }
        public string PautaCodigo { get; set; }

    }
}
