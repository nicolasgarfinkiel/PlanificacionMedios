namespace Irsa.PDM.Dtos
{
    public class ConfirmaionSap
    {
        public int IdOrigen { get; set; }
        public string Resultado { get; set; }
        public string Mensaneje { get; set; }
        public string IdSap { get; set; }
        public MetodoSap? Metodo { get; set; }
    }
}
