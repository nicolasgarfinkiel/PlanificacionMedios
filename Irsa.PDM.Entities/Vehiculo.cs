namespace Irsa.PDM.Entities
{
    public class Vehiculo: EntityBase
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public virtual Medio Medio { get; set; }
    }
}
