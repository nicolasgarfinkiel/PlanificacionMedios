using System;

namespace Irsa.PDM.Entities
{
    public class Certificacion : EntityBase
    {
        public string PautaEjecutadaCodigo { get; set; }
        public string PautaCodigo { get; set; }
        public int CodigoPrograma { get; set; }
        public string CodigoAviso { get; set; }              
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
        public virtual Campania Campania { get; set; }
        public virtual ConsumoSap ConsumoSap { get; set; }
        public EstadoCertificacion Estado { get; set; }

        public double CostoTotal
        {
            get { return DuracionTema * CostoUnitario * 60; }
        }
    }
}



