using System.Collections.Generic;

namespace Irsa.PDM.Dtos
{
    public class Proveedor
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }

        public IList<Vehiculo> Vehiculos { get; set; }
    }
}
