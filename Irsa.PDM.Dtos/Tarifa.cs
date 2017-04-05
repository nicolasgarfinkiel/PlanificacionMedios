namespace Irsa.PDM.Dtos
{
    public class Tarifa
    {
        public int? Id { get; set; }
        public int MedioId { get; set; }
        public int PlazaId { get; set; }
        public int VehiculoId { get; set; }
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
    }
}
