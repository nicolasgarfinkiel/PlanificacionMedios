namespace Irsa.PDM.Dtos
{
    public class Vehiculo
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Medio Medio { get; set; }
        public int Codigo { get; set; }
    }
}
