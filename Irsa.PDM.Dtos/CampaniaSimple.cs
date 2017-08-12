using System;
using System.Collections.Generic;

namespace Irsa.PDM.Dtos
{
    public class CampaniaSimple
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public DateTime CreateDate { get; set; }        
    }
}
