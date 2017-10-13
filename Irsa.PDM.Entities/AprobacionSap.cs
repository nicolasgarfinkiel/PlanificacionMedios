using System.Collections.Generic;

namespace Irsa.PDM.Entities
{
    public class AprobacionSap : EntityBase
    {
        public EstadoAprobacionSap EstadoConsumo { get; set; }
        public EstadoAprobacionSap EstadoProvision { get; set; }
        public EstadoAprobacionSap EstadoCertificacion { get; set; }

        public virtual Campania Campania { get; set; }
        public virtual Proveedor Proveedor { get; set; }
        public double MontoTotal { get; set; }
    }
}
