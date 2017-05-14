using System;

namespace Irsa.PDM.Dtos
{
    public class TarifaFcMedios
    {
        public int cod_programa { get; set; }
        public string cod_medio { get; set; }
        public string des_medio { get; set; }
        public string cod_plaza { get; set; }
        public string des_plaza { get; set; }
        public int cod_vehiculo { get; set; }
        public string des_vehiculo { get; set; }
        public string espacio { get; set; }
        public string dia_lunes { get; set; }
        public string dia_martes { get; set; }
        public string dia_miercoles { get; set; }
        public string dia_jueves { get; set; }
        public string dia_viernes { get; set; }
        public string dia_sabado { get; set; }
        public string dia_domingo { get; set; }
        public int hora_inicio { get; set; }
        public int hora_fin { get; set; }
        public double bruto { get; set; }    

        public DateTime fecha_tarifa { get; set; }

        public bool Lunes
        {
            get { return string.Equals(dia_lunes, "S"); }
        }

        public bool Martes
        {
            get { return string.Equals(dia_martes, "S"); }
        }

        public bool Miercoles
        {
            get { return string.Equals(dia_miercoles, "S"); }
        }

        public bool Jueves
        {
            get { return string.Equals(dia_jueves, "S"); }
        }

        public bool Viernes
        {
            get { return string.Equals(dia_viernes, "S"); }
        }

        public bool Sabado
        {
            get { return string.Equals(dia_sabado, "S"); }
        }

        public bool Domingo
        {
            get { return string.Equals(dia_domingo, "S"); }
        }
      
    }
}
