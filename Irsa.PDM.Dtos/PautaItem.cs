namespace Irsa.PDM.Dtos
{
    public class PautaItem
    {
        public int CodigoPrograma { get; set; }   
        public string Proveedor { get; set; }      
        public string Espacio { get; set; }
        public string Tema { get; set; }      
        public double CostoUnitario { get; set; }
        public int DuracionTema { get; set; }
        public string Producto { get; set; }
        public double Descuento1 { get; set; }
        public double Descuento2 { get; set; }
        public double Descuento3 { get; set; }
        public double Descuento4 { get; set; }
        public double Descuento5 { get; set; }
        public Tarifa Tarifa { get; set; }
        public bool DiferenciaEnMontoTarifas { get; set; }
        public double CostoTotal { get; set; }
    }
}
