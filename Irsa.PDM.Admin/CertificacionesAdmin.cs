using System;
using System.Collections.Generic;
using System.Linq;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Entities;
using Irsa.PDM.Repositories;
using OfficeOpenXml;
using ServiceStack.ServiceClient.Web;

namespace Irsa.PDM.Admin
{
    public class CertificacionesAdmin : BaseAdmin<int, Entities.Certificacion, Dtos.Certificacion, FilterBase>
    {
        private const string ImportUser = "Import process";
        private const string GetCertificaciones = "/client?method=create&action=pautas_controladas";

        #region Base

        public override Entities.Certificacion ToEntity(Dtos.Certificacion dto)
        {
            return null;
        }

        public override void Validate(Dtos.Certificacion dto)
        {
        }

        public override IQueryable GetQuery(FilterBase filter)
        {
            var result = PdmContext.Certificaciones
                        .OrderBy(e => e.Campania)
                        .ThenBy(e => e.PautaCodigo)
                        .ThenBy(e => e.PautaEjecutadaCodigo)
                        .AsQueryable();

            if (filter.MultiColumnSearchText != null)
            {
                var multiColumnSearchText = filter.MultiColumnSearchText.ToLower();
                result = result.Where(e =>
                    (
                        (e.Campania.Nombre.ToLower().Contains(multiColumnSearchText)) ||
                        (System.Data.Entity.SqlServer.SqlFunctions.StringConvert((decimal)e.CodigoPrograma).Contains(multiColumnSearchText)) ||
                        (e.Campania.Nombre.ToLower().Contains(multiColumnSearchText)) ||
                        (e.Espacio.ToLower().Contains(multiColumnSearchText)) ||
                        (e.PautaCodigo.ToLower().Contains(multiColumnSearchText)) ||
                        (e.PautaEjecutadaCodigo.ToLower().Contains(multiColumnSearchText)) ||
                        (e.Proveedor.ToLower().Contains(multiColumnSearchText)) ||
                        (e.Tema.ToLower().Contains(multiColumnSearchText))
                    )).AsQueryable();
            }

            return result;
        }

        #endregion

        #region Sync

        public void SyncCertificaciones()
        {                        
            var pautas = PdmContext.Pautas.Where(e => e.Estado == EstadoPauta.Aprobada ).Select(e => new {nro_pauta_aprobada = e.Codigo}).ToList();

            var client = new JsonServiceClient(FcMediosTarifarioUrl);
            var certificaciones = client.Post<IList<CertificacionFcMedios>>(GetCertificaciones, pautas).ToList();
            var campaniaNombres = certificaciones.Select(e => e.campania).Distinct().ToList();            
            var campanias = PdmContext.Campanias.Where(e => campaniaNombres.Contains(e.Nombre)).ToList();
            
            certificaciones.ForEach(c =>
            {
                var campania = campanias.SingleOrDefault(e => string.Equals(e.Nombre, c.campania) );
                if (campania == null) return;

                if (PdmContext.Certificaciones.Any(e => e.Campania.Id == campania.Id && 
                    string.Equals(e.PautaCodigo, c.nro_pauta_aprobada) &&
                    string.Equals(e.PautaEjecutadaCodigo, c.nro_pauta_ejecutada) &&
                    e.CodigoPrograma == c.cod_programa))  return;

                var certificacion = new Entities.Certificacion
                {
                    Campania = campania,
                    CodigoAviso = c.cod_aviso,
                    CodigoPrograma = c.cod_programa,
                    CostoUnitario = c.costo_unitario,
                    CreateDate = DateTime.Now,
                    CreatedBy = ImportUser,
                    Descuento1 = c.descuento_1,
                    Descuento2 = c.descuento_2,
                    Descuento3 = c.descuento_3,
                    Descuento4 = c.descuento_4,
                    Descuento5 = c.descuento_5,
                    DuracionTema = c.duracion_tema,
                    Enabled = true,
                    Espacio = c.espacio,
                    FechaAviso = c.fecha_aviso,
                    PautaCodigo = c.nro_pauta_aprobada,
                    PautaEjecutadaCodigo = c.nro_pauta_ejecutada,
                    Proveedor = c.des_proveedor,
                    Tema = c.des_tema                    
                };

                PdmContext.Certificaciones.Add(certificacion);
            });

            PdmContext.Configuration.AutoDetectChangesEnabled = false;
            PdmContext.SaveChanges();
            PdmContext.Configuration.AutoDetectChangesEnabled = true;
            PdmContext = new PDMContext();
        }

        #endregion


        public ExcelPackage GetExcel(FilterBase filter)
        {
            throw new NotImplementedException();
        }
    }
}
