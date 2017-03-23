using System.Collections.Generic;

namespace Irsa.PDM.Dtos
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public IList<Empresa> Empresas { get; set; }
        public Empresa CurrentEmpresa { get; set; }

        public string CurrentEmpresaDescripcion
        {
            get { return CurrentEmpresa == null ? "Empresa" : CurrentEmpresa.Descripcion; }

        }

        public string CurrentEmpresaLabelCuit
        {
            get
            {
                if (CurrentEmpresa == null) return string.Empty;

                var result = string.Empty;
                var pais = CurrentEmpresa.GrupoEmpresa.PaisDescripcion.ToUpper();

                switch (pais)
                {
                    case "ARGENTINA":
                        result = "Cuit";
                        break;
                    case "BOLIVIA":
                        result = "Nit";
                        break;
                    case "PARAGUAY":
                        result = "Ruc";
                        break;
                }


                return result;
            }

        }
    }
}
