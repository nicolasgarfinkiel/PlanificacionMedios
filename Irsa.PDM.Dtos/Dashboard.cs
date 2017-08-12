using System.Collections.Generic;

namespace Irsa.PDM.Dtos
{
    public class Dashboard
    {
        public IList<Tarifario> TarifariosEditables { get; set; }
        public IList<Tarifario> TarifariosPendientesAprobacion { get; set; }
        public IList<Campania> CampaniasConInconsistencias { get; set; }
        public IList<Campania> CampaniasPendientesAprobacion { get; set; }
        public IList<Certificacion> UltimasCertificaciones { get; set; }
    }
}
