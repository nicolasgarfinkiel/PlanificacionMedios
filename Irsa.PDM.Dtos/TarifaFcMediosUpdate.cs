using System;

namespace Irsa.PDM.Dtos
{
    public class TarifaFcMediosUpdate
    {
        public int cod_programa { get; set; }      
        public double bruto { get; set; }
        public string fecha_tarifa { get; set; }
        public double descuento_1 { get; set; }
        public double descuento_2 { get; set; }
        public double descuento_3 { get; set; }
        public double descuento_4 { get; set; }
        public double descuento_5 { get; set; }
    }
}
