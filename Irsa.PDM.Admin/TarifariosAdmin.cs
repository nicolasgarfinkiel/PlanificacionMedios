using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Filters;
using Irsa.PDM.Entities;
using Irsa.PDM.Repositories;
using Newtonsoft.Json;
using ServiceStack.ServiceClient.Web;
using Tarifario = Irsa.PDM.Entities.Tarifario;

namespace Irsa.PDM.Admin
{
    public class TarifariosAdmin : BaseAdmin<int, Entities.Tarifario, Dtos.Tarifario, FilterTarifarios>
    {        
        private const string GetTarifasAction = "/client?method=get-list&action=programas_a_tarifar";        
        
        #region Base

        public override Dtos.Tarifario Create(Dtos.Tarifario dto)
        {
            Validate(dto);

            var entity = ToEntity(dto);
            PdmContext.Tarifarios.Add(entity);
            PdmContext.SaveChanges();
            
            try
            {
                InitTarifario(entity);
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

        public override void Delete(int id)
        {
            var entity = default(Entities.Tarifario);

            try
            {
                new TarifasAdmin().SetValues(new FilterTarifas { TarifarioId = id }, null, null, true);
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
        
        public override Tarifario ToEntity(Dtos.Tarifario dto)
        {
            var entity = default(Tarifario);
            var vehiculo = PdmContext.Vehiculos.Single(e => e.Id == dto.Vehiculo.Id);

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
                    Vehiculo = vehiculo
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

            return result;
        }

        #endregion

        public DateTime GetFechaDesde(int vehiculolId)
        {
            var lastTarifario = PdmContext.Tarifarios.Where(e => e.Vehiculo.Id == vehiculolId).OrderByDescending(e => e.Id).FirstOrDefault();

            return lastTarifario == null ? DateTime.Now : lastTarifario.FechaHasta.AddDays(1);
        }

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

            var client = new JsonServiceClient(FcMediosTarifarioUrl);
            var tarifas = client.Get<IList<TarifaFcMedios>>(GetTarifasAction);

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

        private void InitTarifario(Tarifario entity)
        {
            var client = new JsonServiceClient(FcMediosTarifarioUrl);
            var tarifas = client.Get<IList<TarifaFcMedios>>(GetTarifasAction)
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
                    Importe = t.bruto,
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
                });
            });

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
      
        #endregion
    }
}


