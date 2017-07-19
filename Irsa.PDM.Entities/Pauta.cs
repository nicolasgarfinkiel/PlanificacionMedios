using System;
using System.Collections.Generic;

namespace Irsa.PDM.Entities
{
    public class Pauta : EntityBase
    {
        public string Codigo { get; set; }
        public EstadoPauta Estado { get; set; }
        public virtual Campania Campania { get; set; }
        public virtual IList<PautaItem> Items { get; set; }

        public DateTime? FechaCierre { get; set; }
    }
}
