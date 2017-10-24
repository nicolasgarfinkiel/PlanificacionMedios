using System;
using System.Collections.Generic;
using System.Linq;
using EntityFramework.Utilities;
using Irsa.PDM.Admin.ServicesAdmin;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Filters;
using Irsa.PDM.Entities;
using Newtonsoft.Json;
using ServiceStack.Common.Extensions;
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
                .ToList()
                .Select(e => new Entities.AprobacionSap
                {
                    Campania = e.Key.Campania,
                    ProveedorCodigo = e.Key.ProveedorCodigo,
                    ProveedorNombre = e.Key.ProveedorNombre,
                    EstadoCertificacion = EstadoAprobacionSap.Ingresada,
                    EstadoConsumo = EstadoAprobacionSap.Ingresada,
                    EstadoProvision = EstadoAprobacionSap.Ingresada,
                    MontoTotal = e.Sum(c => c.DuracionTema * c.CostoUnitario * 60),
                    CreateDate = DateTime.Now,
                    CreatedBy = UsuarioLogged
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

        public void ConfirmarAprobacion(IList<ConfirmaionSap> confirmaciones)
        {
            var aprobacionesSapId = confirmaciones.Select(e => e.IdOrigen).ToList();
            var aprobaciones = PdmContext.AprobacionesSap.Where(e => aprobacionesSapId.Contains(e.Id)).ToList();
            var aprobacionesId = aprobaciones.Select(e => e.Id).ToList();
            var diff = aprobacionesSapId.Except(aprobacionesId).ToList();

            #region Validation

            if (confirmaciones.Any(e => e.IdOrigen == 0))
            {
                throw new Exception(string.Format("El campo idOrigen es obligatorio "));
            }

            if (confirmaciones.Any(e => string.IsNullOrEmpty(e.Resultado)))
            {
                throw new Exception(string.Format("El campo resultado es obligatorio "));
            }

            if (confirmaciones.Any(e => !e.Metodo.HasValue))
            {
                throw new Exception(string.Format("El campo metodo es obligatorio "));
            }      

            if (diff.Any())
            {
                throw new Exception(string.Format("Los siguientes idOrigen no exiten en el sistema: {0}", string.Join(", ", diff)));
            }            

            if (confirmaciones.Any(e => !string.Equals(e.Resultado, "Confirmada") && !string.Equals(e.Resultado, "Error")))
            {
                throw new Exception(string.Format("Revise el valor ingresado en el campo resultado. Los valores posibles son: Confirmada y Error"));
            }

            if (confirmaciones.Any(e => e.Metodo != MetodoSap.Certificacion && e.Metodo != MetodoSap.Consumo && e.Metodo != MetodoSap.Provision ))
            {
                throw new Exception(string.Format("Revise el valor ingresado en el campo metodo. Los valores posibles son: Certificacion, Consumo y Provision"));
            }

            #endregion

            confirmaciones.ForEach(confirmacion =>
            {
                var aprobacion = aprobaciones.Single(e => e.Id == confirmacion.IdOrigen);
                aprobacion.MensajeSap = confirmacion.Mensaneje;

                switch (confirmacion.Metodo.Value)
                {
                        case MetodoSap.Certificacion:
                        aprobacion.IdReferenciaCertificacion = confirmacion.IdSap;
                        aprobacion.FechaConfirmacionCertificacion = DateTime.Now;
                        aprobacion.EstadoCertificacion = string.Equals(confirmacion.Resultado, "Confirmada") ? EstadoAprobacionSap.Confirmada : EstadoAprobacionSap.Error;
                        break;
                        case MetodoSap.Consumo:
                        aprobacion.IdReferenciaConsumo = confirmacion.IdSap;
                        aprobacion.FechaConfirmacionConsumo = DateTime.Now;
                        aprobacion.EstadoConsumo = string.Equals(confirmacion.Resultado, "Confirmada") ? EstadoAprobacionSap.Confirmada : EstadoAprobacionSap.Error;
                        break;
                        case MetodoSap.Provision:
                        aprobacion.IdReferenciaProvision = confirmacion.IdSap;
                        aprobacion.FechaConfirmacionProvision = DateTime.Now;
                        aprobacion.EstadoProvision = string.Equals(confirmacion.Resultado, "Confirmada") ? EstadoAprobacionSap.Confirmada : EstadoAprobacionSap.Error;
                        break;
                }

            });

            PdmContext.SaveChanges();
        }
    
        #region Log

        private void LogSyncAprobacionesDetail(IList<Entities.AprobacionSap> aprobaciones)
        {
            var data = aprobaciones.Select(e => new AprobacionSap
            {
                CampaniaId = e.Campania.Id,
                CampaniaNombre = e.Campania.Nombre,
                ProveedorCodigo = e.ProveedorCodigo,
                ProveedorNombre = e.ProveedorNombre,
                MontoTotal = e.MontoTotal
                
            }).ToList();

            var log = new Dtos.Log
            {
                Accion = "AprobacionesSapAdmin.CreateAprobaciones",
                App = "Irsa.PDM.Web",
                CreateDate = DateTime.Now,
                Modulo = "Aprobaciones",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = string.Format("DETALLE de aprobaciones a sincronizar con sap: {0}", JsonConvert.SerializeObject(data))
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
