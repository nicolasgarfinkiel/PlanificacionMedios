namespace Irsa.PDM.Entities
{
    public class Tarifa: EntityBase
    { 
        public int CodigoPrograma{ get; set; }
        public virtual Tarifario Tarifario { get; set; }
        public virtual Medio Medio { get; set; }
        public virtual Plaza Plaza { get; set; }
        public virtual Vehiculo Vehiculo { get; set; }
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

        public double ImporteOld { get; set; }
    }
}
