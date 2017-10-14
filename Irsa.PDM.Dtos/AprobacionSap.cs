using System.Collections.Generic;

namespace Irsa.PDM.Dtos
{
    public class AprobacionSap
    {
        public int Id { get; set; }
        public string EstadoConsumo { get; set; }
        public string EstadoProvision { get; set; }
        public string EstadoCertificacion { get; set; }
        public int CampaniaId  { get; set; }
        public string CampaniaNombre { get; set; }
        public int ProveedorCodigo { get; set; }
        public string ProveedorNombre { get; set; }
        public double MontoTotal { get; set; }
    }
}
