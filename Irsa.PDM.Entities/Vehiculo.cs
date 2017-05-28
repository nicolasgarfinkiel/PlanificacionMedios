using System.Collections.Generic;

namespace Irsa.PDM.Entities
{
    public class Vehiculo: EntityBase
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public virtual IList<Proveedor> Proveedores { get; set; }        
    }
}
