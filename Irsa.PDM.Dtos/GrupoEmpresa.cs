namespace Irsa.PDM.Dtos
{
    public class GrupoEmpresa
    {
        public int? Id { get; set; }
        public string Descripcion { get; set; }        
        public int IdApp { get; set; }
        public int PaisId { get; set; }
        public string PaisDescripcion { get; set; }   
    }    				
}
