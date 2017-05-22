using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace Irsa.PDM.Entities
{
    public class Campania : EntityBase
    {
        public string Nombre { get; set; }
        public EstadoCampania Estado { get; set; }
        public virtual IList<Pauta> Pautas { get; set; }        
    }
}
