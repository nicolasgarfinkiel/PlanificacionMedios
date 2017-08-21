namespace Irsa.PDM.Dtos
{
    public class PautaDetail
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Estado { get; set; }
        public Campania Campania { get; set; }
    }
}
