using System;
using System.Collections.Generic;

namespace Irsa.PDM.Entities
{
    public class AprobacionSap : EntityBase
    {
        public EstadoAprobacionSap EstadoConsumo { get; set; }
        public EstadoAprobacionSap EstadoProvision { get; set; }
        public EstadoAprobacionSap EstadoCertificacion { get; set; }

        public virtual Campania Campania { get; set; }
        public int ProveedorCodigo { get; set; }
        public string ProveedorNombre { get; set; }
        public double MontoTotal { get; set; }
        public string MensajeSap { get; set; }
        public string IdReferenciaConsumo { get; set; }
        public string IdReferenciaProvision { get; set; }
        public string IdReferenciaCertificacion { get; set; }
        public DateTime? FechaConfirmacionConsumo { get; set; }
        public DateTime? FechaConfirmacionProvision { get; set; }
        public DateTime? FechaConfirmacionCertificacion { get; set; }
    }
}
