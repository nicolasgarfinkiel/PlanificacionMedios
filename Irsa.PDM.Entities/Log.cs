namespace Irsa.PDM.Entities
{
    public class Log: EntityBase
    {
        public string App { get; set; }
        public string Modulo { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public string Accion { get; set; }
        public string UsuarioAccion { get; set; }
        public string StackTrace { get; set; }
    }
}
