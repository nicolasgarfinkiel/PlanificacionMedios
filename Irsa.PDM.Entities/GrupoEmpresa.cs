namespace Irsa.PDM.Entities
{
    public class GrupoEmpresa
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public int IdApp { get; set; }
        public virtual Pais Pais { get; set; }   
    }    				
}
