﻿using System;

namespace Irsa.PDM.Entities
{
    public class PautaItem : EntityBase
    {
        public int CodigoPrograma { get; set; }
        public string CodigoAviso { get; set; }
        public virtual Pauta Pauta { get; set; }
        public virtual Tarifa Tarifa { get; set; }
        public int ProveedorCodigo { get; set; }
        public string Proveedor { get; set; }
        public string Producto { get; set; }
        public DateTime? FechaAviso { get; set; }
        public string Espacio { get; set; }
        public string Tema { get; set; }
        public double DuracionTema { get; set; }
        public double CostoUnitario { get; set; }
        public double Descuento1 { get; set; }
        public double Descuento2 { get; set; }
        public double Descuento3 { get; set; }
        public double Descuento4 { get; set; }
        public double Descuento5 { get; set; }
        public double Rtg1 { get; set; }
        public double Rtg2 { get; set; }
        public double Rtg3 { get; set; }
        public double Cpr1 { get; set; }
        public double Cpr2 { get; set; }
        public double Cpr3 { get; set; }

        public bool DiferenciaEnMontoTarifas { get; set; }

        public double CostoTotal
        {
            get { return DuracionTema*CostoUnitario*60; }
        }
    }
}
