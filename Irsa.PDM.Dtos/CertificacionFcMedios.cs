using System;

namespace Irsa.PDM.Dtos
{
    public class CertificacionFcMedios
    {
        public string nro_pauta_ejecutada { get; set; }
        public string nro_pauta_aprobada { get; set; }
        public int cod_programa { get; set; }
        public int cod_vehiculo { get; set; }
        public string cod_aviso { get; set; }
        public int cod_campania { get; set; }
        public string des_campania { get; set; }
        public string des_proveedor { get; set; }
        public DateTime fecha_aviso { get; set; }
        public string espacio { get; set; }
        public string des_tema { get; set; }
        public int duracion_tema { get; set; }
        public double costo_unitario { get; set; }
        public double descuento_1 { get; set; }
        public double descuento_2 { get; set; }
        public double descuento_3 { get; set; }
        public double descuento_4 { get; set; }
        public double descuento_5 { get; set; }
    }
}
