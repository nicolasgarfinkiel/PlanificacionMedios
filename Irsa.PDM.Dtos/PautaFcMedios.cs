﻿using System;

namespace Irsa.PDM.Dtos
{
    public class PautaFcMedios
    {
        public string nro_pauta { get; set; }
        public int cod_programa { get; set; }
        public int cod_vehiculo { get; set; }
        public string des_vehiculo { get; set; }
        public string cod_aviso { get; set; }
        public string cod_medio { get; set; }
        public string des_medio { get; set; }
        public string cod_plaza { get; set; }
        public string des_plaza { get; set; }
        public int cod_campania { get; set; }
        public string des_campania { get; set; }
        public int cod_proveedor { get; set; }
        public string des_proveedor { get; set; }
        public string des_producto { get; set; }
        public DateTime fecha_aviso { get; set; }
        public string espacio { get; set; }
        public string des_tema { get; set; }
        public double duracion_tema { get; set; }
        public int hora_inicio { get; set; }
        public int hora_fin { get; set; }
        public double costo_unitario { get; set; }
        public double descuento_1 { get; set; }
        public double descuento_2 { get; set; }
        public double descuento_3 { get; set; }
        public double descuento_4 { get; set; }
        public double descuento_5 { get; set; }

        public double rtg_1 { get; set; }
        public double rtg_2 { get; set; }
        public double rtg_3 { get; set; }
        public double cpr_1 { get; set; }
        public double cpr_2 { get; set; }
        public double cpr_3 { get; set; }
    }
}
