using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Entities;
using Irsa.PDM.Repositories;
using Newtonsoft.Json;
using OfficeOpenXml;
using ServiceStack.ServiceClient.Web;

namespace Irsa.PDM.Admin
{
    public class CertificacionesAdmin : BaseAdmin<int, Entities.Certificacion, Dtos.Certificacion, FilterBase>
    {        
        private const string GetCertificaciones = "/client?method=create&action=pautas_controladas";
        private readonly LogAdmin LogAdmin;

        public CertificacionesAdmin()
        {
            LogAdmin = new LogAdmin();   
        }

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
                        .OrderByDescending(e => e.CreateDate)                        
                        .AsQueryable();

            if (filter.MultiColumnSearchText != null)
            {
                var multiColumnSearchText = filter.MultiColumnSearchText.ToLower();
                result = result.Where(e =>
                    (
                        (e.Campania != null && e.Campania.Nombre.ToLower().Contains(multiColumnSearchText)) ||
                        (e.Campania != null && System.Data.Entity.SqlServer.SqlFunctions.StringConvert((decimal)e.Campania.Codigo).Contains(multiColumnSearchText)) ||
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
            LogSyncCertificacionesInit();

            try
            {                
                var pautas = PdmContext.Pautas.Where(e => e.Estado == EstadoPauta.Aprobada ).Select(e => new {nro_pauta_aprobada = e.Codigo}).ToList();
               
                var certificaciones = FCMediosClient.Post<IList<CertificacionFcMedios>>(GetCertificaciones, pautas).ToList();
                var campaniasCodigos = certificaciones.Select(e => e.cod_campania).Distinct().ToList();            
                var campanias = PdmContext.Campanias.Where(e => campaniasCodigos.Contains(e.Codigo)).ToList();

                LogSyncCertificacionesDetail(certificaciones);
            
                certificaciones.ForEach(c =>
                {                  
                    var campania = campanias.SingleOrDefault(e => c.cod_campania == e.Codigo );

                    var certificacion = PdmContext.Certificaciones.FirstOrDefault(e => 
                                        string.Equals(e.PautaCodigo, c.nro_pauta_aprobada) &&
                                        string.Equals(e.PautaEjecutadaCodigo, c.nro_pauta_ejecutada) &&
                                        e.Campania.Codigo == c.cod_campania &&
                                        e.CodigoPrograma == c.cod_programa);

                    if (certificacion != null && certificacion.Estado == EstadoCertificacion.Aceptada) return;

                    if (certificacion == null)
                    {
                        certificacion = new Entities.Certificacion
                        {
                            Campania = campania,
                            CodigoAviso = c.cod_aviso,
                            CodigoPrograma = c.cod_programa,
                            CostoUnitario = c.costo_unitario,
                            CreateDate = DateTime.Now,
                            CreatedBy = App.ImportUser,
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
                            Tema = c.des_tema,
                            Producto = c.des_producto
                        };
                    }

                    var estado = EstadoCertificacion.Aceptada;
                    var pautaItem = PdmContext.PautasItem.FirstOrDefault(i =>
                                    i.CodigoPrograma == c.cod_programa &&
                                    i.Pauta.Codigo == c.nro_pauta_aprobada &&
                                    i.Pauta.Campania.Id == campania.Id);

                    if (campania == null)
                    {
                        estado = EstadoCertificacion.CampaniaNoRegistrada;
                    }
                    else if (campania.Estado == EstadoCampania.Cerrada)
                    {
                        estado = EstadoCertificacion.CampaniaCerrada;
                    }
                    else if (pautaItem == null)
                    {
                        estado = EstadoCertificacion.ProgramaNoPautado;
                    }    
                    else if (campania.Estado != EstadoCampania.Cerrada && campania.Estado != EstadoCampania.Aprobada)
                    {
                        estado = EstadoCertificacion.CampaniaNoAprobada;
                    }                                   

                    certificacion.Estado = estado;

                    if (certificacion.Id == 0)
                    {                                           
                        PdmContext.Certificaciones.Add(certificacion);    
                    }

                    if (pautaItem == null) return;

                    var codProgramas = pautaItem.Pauta.Items.Select(e => e.CodigoPrograma).ToList();

                    if (codProgramas.All(cp => PdmContext.Certificaciones.Any(e => e.CodigoPrograma == cp && e.Estado == EstadoCertificacion.Aceptada)))
                    {
                        pautaItem.Pauta.Estado = EstadoPauta.Cerrada;
                        pautaItem.Pauta.FechaCierre = DateTime.Now;
                    }

                    if (pautaItem.Pauta.Campania.Pautas.All(e => e.Estado == EstadoPauta.Cerrada))
                    {
                        pautaItem.Pauta.Campania.Estado = EstadoCampania.Cerrada;
                        pautaItem.Pauta.Campania.FechaCierre = DateTime.Now;
                    }                                                                         
                });

                //PdmContext.Configuration.AutoDetectChangesEnabled = false;
                PdmContext.SaveChanges();
                //PdmContext.Configuration.AutoDetectChangesEnabled = true;
                //PdmContext = new PDMContext();
            }
            catch (Exception ex)
            {
                LogSyncCertificacionesError(ex);
                LogSyncCertificacionesEnd();
                throw;
            }

            LogSyncCertificacionesEnd();
        }       

        #endregion

        public ExcelPackage GetExcel(FilterBase filter)
        {
            var template = new FileInfo(String.Format(@"{0}\Reports\Rpt_Certificaciones.xlsx", AppDomain.CurrentDomain.BaseDirectory));
            var pck = new ExcelPackage(template, true);
            var ws = pck.Workbook.Worksheets[1];
            var row = 8;

            filter.CurrentPage = 1;
            filter.PageSize = 99999999;
            var data = GetByFilter(filter).Data;

            foreach (var item in data)
            {
                row++;
                ws.Cells[row, 1].Value = item.CampaniaCodigo;
                ws.Cells[row, 2].Value = item.CampaniaNombre;
                ws.Cells[row, 3].Value = item.PautaCodigo;
                ws.Cells[row, 4].Value = item.PautaEjecutadaCodigo;
                ws.Cells[row, 5].Value = item.CodigoPrograma;
                ws.Cells[row, 6].Value = item.Proveedor;
                ws.Cells[row, 7].Value = item.Producto;
                ws.Cells[row, 8].Value = item.Espacio;
                ws.Cells[row, 9].Value = item.CodigoAviso;
                ws.Cells[row, 10].Value =  item.FechaAviso.HasValue ? item.FechaAviso.Value.ToString("dd/MM/yyyy") : string.Empty;
                ws.Cells[row, 11].Value = item.CostoUnitario;
                ws.Cells[row, 12].Value = item.DuracionTema;
                ws.Cells[row, 13].Value = item.CostoTotal;
                ws.Cells[row, 14].Value = item.Estado;
            }

            return pck;
        }

        #region Log

        private void LogSyncCertificacionesDetail(List<CertificacionFcMedios> certificaciones)
        {
            var log = new Dtos.Log
            {
                Accion = "CertificaionesAdmin.SyncCertificaciones",
                App = "Irsa.PDM.WindowsService",
                CreateDate = DateTime.Now,
                Modulo = "Certificaciones",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = string.Format("DETALLE de certificaciones a ingresar: {0}", JsonConvert.SerializeObject(certificaciones))
            };

            LogAdmin.Create(log);
        }      

        private void LogSyncCertificacionesError(Exception ex)
        {
            var log = new Dtos.Log
            {
                Accion = "CertificaionesAdmin.SyncCertificaciones",
                App = "Irsa.PDM.WindowsService",
                CreateDate = DateTime.Now,
                Modulo = "Certificaciones",
                Tipo = App.Error,
                UsuarioAccion = UsuarioLogged,
                Descripcion = "Error",
                StackTrace = GetExceptionDetail(ex)
            };

            LogAdmin.Create(log);
        }

        private void LogSyncCertificacionesInit()
        {
            var log = new Dtos.Log
            {
                Accion = "CertificaionesAdmin.SyncCertificaciones",
                App = "Irsa.PDM.WindowsService",
                CreateDate = DateTime.Now,
                Modulo = "Certificaciones",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = "INICIO de sincronización de certificaciones."
            };

            LogAdmin.Create(log);
        }

        private void LogSyncCertificacionesEnd()
        {
            var log = new Dtos.Log
            {
                Accion = "CertificaionesAdmin.SyncCertificaciones",
                App = "Irsa.PDM.WindowsService",
                CreateDate = DateTime.Now,
                Modulo = "Certificaciones",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = "FIN de sincronización de certificaciones."
            };

            LogAdmin.Create(log);
        }

        #endregion
    }
}
