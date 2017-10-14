using System;
using System.Collections.Generic;
using System.Linq;
using EntityFramework.Utilities;
using Irsa.PDM.Admin.ServicesAdmin;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Filters;
using Irsa.PDM.Entities;
using Newtonsoft.Json;
using AprobacionSap = Irsa.PDM.Dtos.AprobacionSap;

namespace Irsa.PDM.Admin
{
    public class AprobacionesSapAdmin : BaseAdmin<int, Entities.AprobacionSap, Dtos.AprobacionSap, FilterAprobacionesSap>
    {               
        private readonly LogAdmin _logAdmin;
        private readonly SapAdmin _sapAdmin;

        public AprobacionesSapAdmin()
        {
            _logAdmin = new LogAdmin();
            _sapAdmin = new SapAdmin();   
        }

        #region Base

        public override AprobacionSap Create(AprobacionSap dto)
        {         
            try
            {
                LogSyncAprobacionesInit();

                var entities = PdmContext.Certificaciones.Where(e => e.Estado == EstadoCertificacion.Aceptada)
                .GroupBy(e => new { e.ProveedorNombre, e.ProveedorCodigo, e.Campania })
                .Select(e => new Entities.AprobacionSap
                {
                    Campania = e.Key.Campania,
                    ProveedorCodigo = e.Key.ProveedorCodigo,
                    ProveedorNombre = e.Key.ProveedorNombre,
                    EstadoCertificacion = EstadoAprobacionSap.Ingresada,
                    EstadoConsumo = EstadoAprobacionSap.Ingresada,
                    EstadoProvision = EstadoAprobacionSap.Ingresada,
                    MontoTotal = e.Sum(c => c.DuracionTema * c.CostoUnitario * 60)
                }).ToList();


                #region Create

                entities.ForEach(e =>
                {
                    PdmContext.AprobacionesSap.Add(e);
                });

                PdmContext.SaveChanges();

                LogSyncAprobacionesDetail(entities);

                entities.ForEach(aprobacion =>
                {
                    var certificaciones = PdmContext.Certificaciones.Where(c =>
                        c.Estado == EstadoCertificacion.Aceptada &&
                        c.Campania.Id == aprobacion.Campania.Id &&
                        c.ProveedorCodigo == aprobacion.ProveedorCodigo).ToList();


                    certificaciones.ForEach(e =>
                    {
                        e.Estado = EstadoCertificacion.Aprobada;
                        e.AprobacionSap = aprobacion;
                        e.UpdateDate = DateTime.Now;
                        e.UpdatedBy = UsuarioLogged;
                    });

                    EFBatchOperation.For(PdmContext, PdmContext.Certificaciones).UpdateAll(certificaciones, x => x.ColumnsToUpdate(                 
                        t => t.AprobacionSap, 
                        t => t.UpdateDate, 
                        t => t.UpdatedBy, 
                        t => t.Estado));

                    PdmContext.SaveChanges();
                });


                #endregion

                #region Send SAP

                _sapAdmin.CreateConsumo(entities);

                entities.ForEach(e =>
                {
                   e.EstadoConsumo = EstadoAprobacionSap.Enviada;                
                });

                PdmContext.SaveChanges();


                _sapAdmin.CreateProvision(entities);

                entities.ForEach(e =>
                {
                    e.EstadoProvision = EstadoAprobacionSap.Enviada;
                });

                PdmContext.SaveChanges();


                _sapAdmin.CreateCertificacion(entities);

                entities.ForEach(e =>
                {
                    e.EstadoCertificacion = EstadoAprobacionSap.Enviada;
                });

                PdmContext.SaveChanges();


                #endregion           

                LogSyncAprobacionesEnd();
            }
            catch (Exception e)
            {
                LogSyncAProbacionesError(e);
                throw;
            }

            return null;
        }

        public override Entities.AprobacionSap ToEntity(Dtos.AprobacionSap dto)
        {
            return null;
        }

        public override void Validate(Dtos.AprobacionSap dto)
        {
        }

        public override IQueryable GetQuery(FilterAprobacionesSap filter)
        {
            var result = PdmContext.AprobacionesSap
                        .OrderByDescending(e => e.CreateDate)                        
                        .AsQueryable();

            if (filter.Campania != null)
            {
                result = result.Where(e => e.Campania.Id == filter.Campania.Id).AsQueryable();
            }           

            if (filter.FechaDesde.HasValue)
            {
                result = result.Where(r => r.CreateDate >= filter.FechaDesde).AsQueryable();
            }

            if (filter.FechaHasta.HasValue)
            {
                var fechaHasta = filter.FechaHasta.Value.AddDays(1).AddMilliseconds(-1);
                result = result.Where(r => r.CreateDate <= fechaHasta).AsQueryable();
            }

            return result;
        }

        #endregion   
     
        public IList<Dtos.AprobacionSap> GetAprobacionesPendientes()
        {
            return PdmContext.Certificaciones.Where(e => e.Estado == EstadoCertificacion.Aceptada)
                .GroupBy(e => new { e.ProveedorNombre, e.ProveedorCodigo, e.Campania })
                .Select(e => new Dtos.AprobacionSap
                {
                    CampaniaId = e.Key.Campania.Id,
                    CampaniaNombre = e.Key.Campania.Nombre,
                    ProveedorCodigo = e.Key.ProveedorCodigo,
                    ProveedorNombre = e.Key.ProveedorNombre,
                    MontoTotal = e.Sum(c => c.DuracionTema * c.CostoUnitario * 60)
                }).ToList();

        }
    
        #region Log

        private void LogSyncAprobacionesDetail(IList<Entities.AprobacionSap> aprobaciones)
        {
            var log = new Dtos.Log
            {
                Accion = "AprobacionesSapAdmin.CreateAprobaciones",
                App = "Irsa.PDM.Web",
                CreateDate = DateTime.Now,
                Modulo = "Aprobaciones",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = string.Format("DETALLE de aprobaciones a sincronizar con sap: {0}", JsonConvert.SerializeObject(aprobaciones))
            };

            _logAdmin.Create(log);
        }      

        private void LogSyncAProbacionesError(Exception ex)
        {
            var log = new Dtos.Log
            {
                Accion = "CertificaionesAdmin.CreateAprobaciones",
                App = "Irsa.PDM.Web",
                CreateDate = DateTime.Now,
                Modulo = "Aprobaciones",
                Tipo = App.Error,
                UsuarioAccion = UsuarioLogged,
                Descripcion = "Error",
                StackTrace = GetExceptionDetail(ex)
            };

            _logAdmin.Create(log);
        }

        private void LogSyncAprobacionesInit()
        {
            var log = new Dtos.Log
            {
                Accion = "CertificaionesAdmin.CreateAprobaciones",
                App = "Irsa.PDM.Web",
                CreateDate = DateTime.Now,
                Modulo = "Aprobaciones",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = "INICIO de sincronización de aprobaciones."
            };

            _logAdmin.Create(log);
        }

        private void LogSyncAprobacionesEnd()
        {
            var log = new Dtos.Log
            {
                Accion = "CertificaionesAdmin.CreateAprobaciones",
                App = "Irsa.PDM.Web",
                CreateDate = DateTime.Now,
                Modulo = "Aprobaciones",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = "FIN de sincronización de aprobaciones."
            };

            _logAdmin.Create(log);
        }

        #endregion    
    
       
    }
}
