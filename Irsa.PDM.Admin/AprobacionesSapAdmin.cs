using System;
using System.Collections.Generic;
using System.Linq;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Filters;
using Irsa.PDM.Entities;
using Newtonsoft.Json;

namespace Irsa.PDM.Admin
{
    public class AprobacionesSapAdmin : BaseAdmin<int, Entities.AprobacionSap, Dtos.AprobacionSap, FilterAprobacionesSap>
    {               
        private readonly LogAdmin LogAdmin;

        public AprobacionesSapAdmin()
        {
            LogAdmin = new LogAdmin();   
        }

        #region Base

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
                    MontoTotal = e.Sum(c => c.CostoTotal)
                }).ToList();

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
