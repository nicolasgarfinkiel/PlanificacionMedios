namespace Irsa.PDM.Dtos
{
    public class Tarifa
    {
        public int? Id { get; set; }
        public int IdTarifario { get; set; }
        public string MedioNombre { get; set; }
        public string PlazaCodigo { get; set; }
        public string VehiculoNombre { get; set; }
        public int? HoraDesde { get; set; }
        public int? HoraHasta { get; set; }
        public bool Lunes { get; set; }
        public bool Martes { get; set; }
        public bool Miercoles { get; set; }
        public bool Jueves { get; set; }
        public bool Viernes { get; set; }
        public bool Sabado { get; set; }
        public bool Domingo { get; set; }
        public string OrdenDeCompra { get; set; }
        public string Descripcion { get; set; }
        public double Importe { get; set; }
    }
}
