using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Filters;
using Irsa.PDM.Entities;
using Irsa.PDM.Repositories;
using Newtonsoft.Json;
using ServiceStack.Common.Extensions;
using Tarifario = Irsa.PDM.Entities.Tarifario;

namespace Irsa.PDM.Admin
{
    public class TarifariosAdmin : BaseAdmin<int, Entities.Tarifario, Dtos.Tarifario, FilterTarifarios>
    {        
        private const string GetTarifasAction = "/client?method=get-list&action=programas_a_tarifar";        
        private readonly LogAdmin LogAdmin;

        public TarifariosAdmin()
        {
            LogAdmin = new LogAdmin();   
        }
        
        #region Base

        public override Dtos.Tarifario Create(Dtos.Tarifario dto)
        {
            Validate(dto);

            var entity = ToEntity(dto);
            PdmContext.Tarifarios.Add(entity);
            PdmContext.SaveChanges();
            
            try
            {             
                InitTarifario(entity, dto.Importe, dto.OrdenDeCompra);
                SaveFile(entity.Id);
            }
            catch (Exception ex)
            {
                PdmContext.Tarifarios.Remove(entity);
                PdmContext.SaveChanges();
                LogCreateError(dto, ex);
                throw;
            }

            var lastTarifario = PdmContext.Tarifarios.Where(e => e.Estado != EstadoTarifario.Eliminado &&
                e.Vehiculo.Id == dto.Vehiculo.Id && e.Id != entity.Id)
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();

            if (lastTarifario != null)
            {
                lastTarifario.Estado = EstadoTarifario.Cerrado;
                lastTarifario.UpdateDate = DateTime.Now;
                lastTarifario.UpdatedBy = UsuarioLogged;
            }

            PdmContext.SaveChanges();
            dto = Mapper.Map<Tarifario, Dtos.Tarifario>(entity);
            LogCreateInfo(dto);

            return dto;
        }

        public override Dtos.Tarifario Update(Dtos.Tarifario dto)
        {
            Validate(dto);
            var entity = ToEntity(dto);
            PdmContext.SaveChanges();
            SaveFile(entity.Id);
            return Mapper.Map<Entities.Tarifario, Dtos.Tarifario>(entity);
        }      

        public override void Delete(int id)
        {
            var entity = default(Entities.Tarifario);

            try
            {          
                entity = PdmContext.Tarifarios.Single(c => c.Id == id);

                entity.Estado = EstadoTarifario.Eliminado;
                entity.UpdatedBy = UsuarioLogged;
                entity.UpdateDate = DateTime.Now;                
                PdmContext.SaveChanges();

                LogDeleteInfo(entity);
            }
            catch (Exception ex)
            {
                LogDeleteError(entity, ex);
                throw;
            }            
        }

        private void SaveFile(int entityId)
        {
            if (PDMSession.Current.File == null) return;

            var path = String.Format(@"{0}\Documents\Tarifarios\{1}", AppDomain.CurrentDomain.BaseDirectory, entityId);
            var file = string.Format(@"{0}\{1}", path, PDMSession.Current.File.FileName);

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);   
            }

            Directory.CreateDirectory(path);            
            PDMSession.Current.File.SaveAs(file);
        }      
        
        public override Tarifario ToEntity(Dtos.Tarifario dto)
        {
            var entity = default(Tarifario);
            var vehiculo = PdmContext.Vehiculos.Single(e => e.Id == dto.Vehiculo.Id);
            var tipoOeracion = (TipoOperacion) Enum.Parse(typeof (TipoOperacion), dto.TipoOperacion);

            if (!dto.Id.HasValue)
            {
                entity = new Tarifario
                {
                    CreateDate = DateTime.Now,
                    CreatedBy = UsuarioLogged,
                    Enabled = true,
                    Estado = EstadoTarifario.Editable,
                    FechaDesde = dto.FechaDesde,
                    FechaHasta = dto.FechaHasta,
                    Vehiculo = vehiculo,
                    NumeroProveedorSap = dto.NumeroProveedorSap,
                    Documento = dto.Documento,
                    TipoOperacion = tipoOeracion
                };
            }
            else
            {
                entity = PdmContext.Tarifarios.Single(c => c.Id == dto.Id.Value);

                entity.UpdateDate = DateTime.Now;
                entity.UpdatedBy = UsuarioLogged;
                entity.FechaDesde = dto.FechaDesde;
                entity.FechaHasta = dto.FechaHasta;
                entity.Vehiculo = vehiculo;
                entity.NumeroProveedorSap = dto.NumeroProveedorSap;
                entity.Documento = dto.Documento;
                entity.TipoOperacion = tipoOeracion;
            }

            return entity;
        }

        public override void Validate(Dtos.Tarifario dto)
        {
            if (dto.FechaHasta <= dto.FechaDesde)
            {
                throw new Exception("La fecha hasta debe ser mayor a la fecha desde");
            }

            var entity = PdmContext.Tarifarios.FirstOrDefault(e => e.FechaDesde == dto.FechaDesde && e.Vehiculo.Id == dto.Vehiculo.Id && e.Estado != EstadoTarifario.Eliminado);

            if (entity != null && entity.Id != dto.Id)
            {
                throw new Exception("Ya existe otro tarifario con misma fecha desde");
            }

        }

        public override IQueryable GetQuery(FilterTarifarios filter)
        {
            var result = PdmContext.Tarifarios
                .Where(e => e.Estado != EstadoTarifario.Eliminado)
                .OrderByDescending(r => r.Id).AsQueryable();


            if (filter.FechaDesde.HasValue)
            {
                result = result.Where(r => r.FechaDesde >= filter.FechaDesde).AsQueryable();
            }

            if (filter.FechaHasta.HasValue)
            {
                var fechaHasta = filter.FechaHasta.Value.AddDays(1).AddMilliseconds(-1);
                result = result.Where(r => r.FechaDesde <= fechaHasta).AsQueryable();
            }

            if (filter.Vehiculo != null)
            {
                result = result.Where(r => r.Vehiculo.Id == filter.Vehiculo.Id).AsQueryable();
            }

            if (filter.Estados != null && filter.Estados.Any())
            {
                var estados = filter.Estados.Select(e => (EstadoTarifario)Enum.Parse(typeof(EstadoTarifario), e)).ToList();
                result = result.Where(r => estados.Contains(r.Estado)).AsQueryable();
            }

            return result;
        }

        #endregion

        public void Aprobar(int tarifarioId)
        {
            var entity = default(Entities.Tarifario);

            try
            {                
                entity = PdmContext.Tarifarios.Single(c => c.Id == tarifarioId);

                entity.Estado = EstadoTarifario.Aprobado;
                entity.UpdatedBy = UsuarioLogged;
                entity.UpdateDate = DateTime.Now;   

                entity.Tarifas.ForEach(t =>
                {
                    t.Estado = EstadoTarifa.Aprobada;
                    t.UpdatedBy = UsuarioLogged;
                    t.UpdateDate = DateTime.Now;   
                });

                var tarifasFcMedios = entity.Tarifas
                    .Select(t => new TarifaFcMediosUpdate
                    {
                        cod_programa = t.CodigoPrograma,
                        bruto = t.Importe,
                        descuento_1 = 0,
                        descuento_2 = 0,
                        descuento_3 = 0,
                        descuento_4 = 0,
                        descuento_5 = 0,
                        fecha_tarifa = entity.FechaDesde.ToString("yyyy-MM-dd 00:00:00"),
                    }).ToList();

                new TarifasAdmin().SyncTarifas(tarifasFcMedios);

                PdmContext.SaveChanges();
                LogAprobarInfo(entity);
            }
            catch (Exception ex)
            {
                LogAprobarError(entity, ex);
                throw;
            }                        
        }        

        public DateTime GetFechaDesde(int vehiculolId)
        {
            var lastTarifario = PdmContext.Tarifarios.Where(e => e.Vehiculo.Id == vehiculolId && e.Estado != EstadoTarifario.Eliminado).OrderByDescending(e => e.Id).FirstOrDefault();

            return lastTarifario == null ? DateTime.Now : lastTarifario.FechaHasta.AddDays(1);
        }

        #region Proveedor

        public DateTime GetFechaDesdeProveedor(int proveedorId)
        {            
            var proveedor = PdmContext.Proveedores.Single(e => e.Id == proveedorId);
            var vehioculosId = proveedor.Vehiculos.Select(e => e.Id).ToList();
            var lastTarifario = PdmContext.Tarifarios.Where(e => vehioculosId.Contains(e.Vehiculo.Id)  && e.Estado != EstadoTarifario.Eliminado).OrderByDescending(e => e.FechaHasta).FirstOrDefault();

            return lastTarifario == null ? DateTime.Now : lastTarifario.FechaHasta.AddDays(1);
        }

        public void CreateTarifariosProveedor(TarifarioProveedor tarifarioProveedor)
        {
            if (tarifarioProveedor.FechaHasta <= tarifarioProveedor.FechaDesde)
            {
                throw new Exception("La fecha hasta debe ser mayor a la fecha desde");
            }

            tarifarioProveedor.Proveedor.Vehiculos.ForEach(e =>
            {
                var dto = new Dtos.Tarifario
                {
                    Documento = tarifarioProveedor.Documento,
                    FechaDesde = tarifarioProveedor.FechaDesde,
                    FechaHasta = tarifarioProveedor.FechaHasta,
                    NumeroProveedorSap = tarifarioProveedor.Proveedor.NumeroProveedorSap,
                    Vehiculo = e,
                    Importe = tarifarioProveedor.Importe,
                    OrdenDeCompra = tarifarioProveedor.Oc,
                    TipoOperacion = tarifarioProveedor.TipoOperacion
                };

                Create(dto);                
            });

        }

        #endregion

        #region Sync

        public void SyncTablasBasicas()
        {
            var serviceSync = PdmContext.ServiceSyncs.FirstOrDefault();

            if (serviceSync != null && !serviceSync.MustSync) return;

            if (serviceSync == null)
            {
                serviceSync = new ServiceSync
                {
                    CreateDate = DateTime.Now,
                    Enabled = true,
                    CreatedBy = UsuarioLogged
                };

                PdmContext.ServiceSyncs.Add(serviceSync);
            }

            serviceSync.LastBaseTablesSync = DateTime.Now;
            
            var tarifas = FCMediosClient.Get<IList<TarifaFcMedios>>(GetTarifasAction);

            #region Base

            var actualMedios = PdmContext.Medios.ToList();
            var actualPlazas = PdmContext.Plazas.ToList();
            var actualVehiculos = PdmContext.Vehiculos.ToList();

            var serviceMedios = tarifas.Select(e => new { Codigo = e.cod_medio, Descripcion = e.des_medio }).Distinct().ToList();
            var servicePlazas = tarifas.Select(e => new { Codigo = e.cod_plaza, Descripcion = e.des_plaza }).Distinct().ToList();
            var serviceVehiculos = tarifas.Select(e => new { Codigo = e.cod_vehiculo, Descripcion = e.des_vehiculo }).Distinct().ToList();

            #region Medios

            serviceMedios.ForEach(t =>
            {
                var medio = actualMedios.FirstOrDefault(e => e.Codigo == t.Codigo);

                if (medio == null)
                {
                    medio = new Entities.Medio
                    {
                        Codigo = t.Codigo,
                        Descripcion = t.Descripcion,
                        Nombre = t.Descripcion,
                        CreateDate = DateTime.Now,
                        CreatedBy = App.ImportUser,
                        Enabled = true
                    };

                    PdmContext.Medios.Add(medio);
                }
                else
                {
                    medio.Descripcion = t.Descripcion;
                    medio.UpdateDate = DateTime.Now;
                    medio.UpdatedBy = App.ImportUser;
                }
            });

            #endregion

            #region Plazas

            servicePlazas.ForEach(t =>
            {
                var plaza = actualPlazas.FirstOrDefault(e => e.Codigo == t.Codigo);

                if (plaza == null)
                {
                    plaza = new Entities.Plaza
                    {
                        Codigo = t.Codigo,
                        Descripcion = t.Descripcion,
                        CreateDate = DateTime.Now,
                        CreatedBy = App.ImportUser,
                        Enabled = true
                    };

                    PdmContext.Plazas.Add(plaza);
                }
                else
                {
                    plaza.Descripcion = t.Descripcion;
                    plaza.UpdateDate = DateTime.Now;
                    plaza.UpdatedBy = App.ImportUser;
                }
            });

            #endregion

            #region Vehiculos

            serviceVehiculos.ForEach(t =>
            {
                var vehiculo = actualVehiculos.FirstOrDefault(e => e.Codigo == t.Codigo);

                if (vehiculo == null)
                {
                    vehiculo = new Entities.Vehiculo
                    {
                        Codigo = t.Codigo,
                        Descripcion = t.Descripcion,
                        CreateDate = DateTime.Now,
                        CreatedBy = App.ImportUser,
                        Enabled = true,
                        Nombre = t.Descripcion
                    };

                    PdmContext.Vehiculos.Add(vehiculo);
                }
                else
                {
                    vehiculo.Nombre = t.Descripcion;
                    vehiculo.Descripcion = t.Descripcion;
                    vehiculo.UpdateDate = DateTime.Now;
                    vehiculo.UpdatedBy = App.ImportUser;
                }
            });

            #endregion

            #endregion

            PdmContext.Configuration.AutoDetectChangesEnabled = false;
            PdmContext.SaveChanges();
            PdmContext.Configuration.AutoDetectChangesEnabled = true;
            PdmContext = new PDMContext();
        }

        private void InitTarifario(Tarifario entity, double? importe = null, string oi = null)
        {            
            var tarifas = FCMediosClient.Get<IList<TarifaFcMedios>>(GetTarifasAction)
                          .Where(e => e.cod_vehiculo == entity.Vehiculo.Codigo).ToList();

            var actualMedios = PdmContext.Medios.ToList();
            var actualPlazas = PdmContext.Plazas.ToList();
            var actualVehiculos = PdmContext.Vehiculos.ToList();

            #region Tarifas

            var toAdd = new List<Entities.Tarifa>();

            tarifas.ForEach(t =>
            {
                var medio = actualMedios.Single(e => e.Codigo == t.cod_medio);
                var plaza = actualPlazas.Single(e => e.Codigo == t.cod_plaza);
                var vehiculo = actualVehiculos.Single(e => e.Codigo == t.cod_vehiculo);

                toAdd.Add(new Entities.Tarifa
                {
                    CodigoPrograma = t.cod_programa,
                    Descripcion = t.espacio,
                    HoraDesde = t.hora_inicio,
                    HoraHasta = t.hora_fin,
                    Importe = importe ?? t.bruto,
                    OrdenDeCompra = oi,
                    ImporteOld = t.bruto,
                    Lunes = t.Lunes,
                    Martes = t.Martes,
                    Miercoles = t.Miercoles,
                    Jueves = t.Jueves,
                    Viernes = t.Viernes,
                    Sabado = t.Sabado,
                    Domingo = t.Domingo,
                    Medio = medio,
                    Plaza = plaza,
                    Tarifario = entity,
                    Vehiculo = vehiculo,
                    CreateDate = DateTime.Now,
                    CreatedBy = App.ImportUser,
                    Enabled = true,
                    Estado = t.bruto > 0 ? EstadoTarifa.PendienteAprobacion : EstadoTarifa.SinTarifaAsociada
                });
            });

            entity.Estado = toAdd.All(tt => tt.Estado == EstadoTarifa.PendienteAprobacion)
                 ? EstadoTarifario.PendienteAprobacion
                 : EstadoTarifario.Editable;

            PdmContext.Configuration.AutoDetectChangesEnabled = false;
            toAdd.ForEach(e => PdmContext.Tarifas.Add(e));

            PdmContext.SaveChanges();
            PdmContext.Configuration.AutoDetectChangesEnabled = true;
            PdmContext = new PDMContext();

            #endregion
        }

        #endregion

        #region Logs

        private void LogCreateInfo(Dtos.Tarifario dto)
        {
            var log = new Dtos.Log
            {
                Accion = "TarifarioAdmin.CreateTarifario",
                App = "Irsa.PDM.Web",
                CreateDate = DateTime.Now,
                Modulo = "Tarifarios",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = string.Format("Nuevo tarifario. ID: {0}", dto.Id)
            };

            LogAdmin.Create(log);
        }

        private void LogCreateError(Dtos.Tarifario dto, Exception ex)
        {                   
            var log = new Dtos.Log
            {
                Accion = "TarifarioAdmin.CreateTarifario",
                App = "Irsa.PDM.Web",
                CreateDate = DateTime.Now,
                Modulo = "Tarifarios",
                Tipo = App.Error,
                UsuarioAccion = UsuarioLogged,
                Descripcion = JsonConvert.SerializeObject(dto),
                StackTrace = GetExceptionDetail(ex)
            };

            LogAdmin.Create(log);
        }

        private void LogDeleteInfo(Tarifario entity)
        {
            var log = new Dtos.Log
            {
                Accion = "TarifarioAdmin.DeleteTarifario",
                App = "Irsa.PDM.Web",
                CreateDate = DateTime.Now,
                Modulo = "Tarifarios",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = string.Format("Tarifario dado de baja. ID: {0}", entity.Id)
            };

            LogAdmin.Create(log);
        }

        private void LogDeleteError(Tarifario entity, Exception ex)
        {           
            var log = new Dtos.Log
            {
                Accion = "TarifarioAdmin.DeleteTarifario",
                App = "Irsa.PDM.Web",
                CreateDate = DateTime.Now,
                Modulo = "Tarifarios",
                Tipo = App.Error,
                UsuarioAccion = UsuarioLogged,
                Descripcion = string.Format("Tarifario. ID: {0}", entity.Id),
                StackTrace = GetExceptionDetail(ex)
            };

            LogAdmin.Create(log);
        }
        
        private void LogAprobarInfo(Tarifario entity)
        {
            var log = new Dtos.Log
            {
                Accion = "TarifarioAdmin.AprobarTarifario",
                App = "Irsa.PDM.Web",
                CreateDate = DateTime.Now,
                Modulo = "Tarifarios",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = string.Format("Tarifario aprobado. ID: {0}", entity.Id)
            };

            LogAdmin.Create(log);           
        }

        private void LogAprobarError(Tarifario entity, Exception ex)
        {
            var log = new Dtos.Log
            {
                Accion = "TarifarioAdmin.AprobarTarifario",
                App = "Irsa.PDM.Web",
                CreateDate = DateTime.Now,
                Modulo = "Tarifarios",
                Tipo = App.Error,
                UsuarioAccion = UsuarioLogged,
                Descripcion = string.Format("Tarifario. ID: {0}", entity.Id),
                StackTrace = GetExceptionDetail(ex)
            };

            LogAdmin.Create(log);
        }

      
        #endregion
        
    }
}


