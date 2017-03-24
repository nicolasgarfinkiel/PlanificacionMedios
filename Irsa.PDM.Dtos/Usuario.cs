using System.Collections.Generic;

namespace Irsa.PDM.Dtos
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public IList<string> Roles { get; set; }
    }
}
