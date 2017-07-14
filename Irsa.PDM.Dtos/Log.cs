using System;

namespace Irsa.PDM.Dtos
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public string App { get; set; }
        public string Modulo { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }        
        public string UsuarioAccion { get; set; }
        public string Accion { get; set; }
        public string StackTrace { get; set; }
    }
}
