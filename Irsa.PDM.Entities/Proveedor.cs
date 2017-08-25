using System.Collections.Generic;

namespace Irsa.PDM.Entities
{
    public class Proveedor : EntityBase
    {
        public string Nombre { get; set; }
        public string NumeroProveedorSap { get; set; }      
        public virtual IList<Vehiculo> Vehiculos { get; set; }        
    }
}
