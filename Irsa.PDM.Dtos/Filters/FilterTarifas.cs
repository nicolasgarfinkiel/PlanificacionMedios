using System.Collections;
using System.Collections.Generic;
using Irsa.PDM.Dtos.Common;

namespace Irsa.PDM.Dtos.Filters
{
    public class FilterTarifas : FilterBase
    {
        public IList<int> Medios { get; set; }
        public IList<int> Plazas { get; set; }
        public IList<int> Vehiculos { get; set; }
        public int? HoraDesde { get; set; }
        public int? HoraHasta { get; set; }
        public IList<string> Dias { get; set; }
        public string OrdenDeCompra { get; set; }

        public bool Lunes
        {
            get { return Dias != null && Dias.Contains("Lunes"); }
        }

        public bool Martes
        {
            get { return Dias != null && Dias.Contains("Martes"); }
        }

        public bool Miercoles
        {
            get { return Dias != null && Dias.Contains("Miercoles"); }
        }

        public bool Jueves
        {
            get { return Dias != null && Dias.Contains("Jueves"); }
        }

        public bool Viernes
        {
            get { return Dias != null && Dias.Contains("Viernes"); }
        }

        public bool Sabado
        {
            get { return Dias != null && Dias.Contains("Sabado"); }
        }

        public bool Domingo
        {
            get { return Dias != null && Dias.Contains("Domingo"); }
        }

    }
}
