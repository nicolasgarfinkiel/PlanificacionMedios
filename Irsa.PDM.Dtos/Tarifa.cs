using System;

namespace Irsa.PDM.Dtos
{
    public class Tarifa
    {
        public int CodigoPrograma { get; set; }
        public int? Id { get; set; }
        public int IdTarifario { get; set; }
        public string MedioNombre { get; set; }
        public string PlazaCodigo { get; set; }
        public string VehiculoNombre { get; set; }
        public int? HoraDesde { get; set; }
        public int? HoraHasta { get; set; }
        public bool Lunes { get; set; }
        public bool Martes { get; set; }
        public bool Miercoles { get; set; }
        public bool Jueves { get; set; }
        public bool Viernes { get; set; }
        public bool Sabado { get; set; }
        public bool Domingo { get; set; }
        public string OrdenDeCompra { get; set; }
        public string Descripcion { get; set; }
        public double Importe { get; set; }
        public bool Nueva { get; set; }

        public string HoraDesdeFormatted
        {
            get
            {
                if (!HoraDesde.HasValue) return null;

                var hora =  string.Format("{0}{1}", new String('0', 4 - HoraDesde.Value.ToString().Length), HoraDesde);

                return string.Format("{0}:{1}", hora.Substring(0, 2), hora.Substring(2,2));
            }
        }

        public string HoraHastaFormatted
        {
            get
            {
                if (!HoraHasta.HasValue) return null;

                var hora = string.Format("{0}{1}", new String('0', 4 - HoraHasta.Value.ToString().Length), HoraHasta);

                return string.Format("{0}:{1}", hora.Substring(0, 2), hora.Substring(2, 2));
            }
        }
    }
}
