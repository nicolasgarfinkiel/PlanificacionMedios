using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Filters;
using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;
using Tarifa = Irsa.PDM.Entities.Tarifa;

namespace Irsa.PDM.Admin
{
    public class TarifasAdmin : BaseAdmin<int, Entities.Tarifa, Dtos.Tarifa, FilterTarifas>
    {
        private const string PostTarifasAction = "/client?method=create&action=programas_tarifados";
        private const string ErrorMessage = "Se actualizaron satisfactoriamente las tarifas.";

        #region Base

        public override Dtos.Tarifa Update(Dtos.Tarifa dto)
        {
            var entity = ToEntity(dto);

            #region Sync

            var tarifasFcMedios = new List<TarifaFcMediosUpdate>
            {
                new TarifaFcMediosUpdate
                {
                    cod_programa = entity.CodigoPrograma,
                    fecha_tarifa = entity.Tarifario.FechaDesde.ToString("yyyy-dd-MM 00:00:00"),
                    bruto = entity.Importe,
                    descuento_1 = 0,
                    descuento_2 = 0,
                    descuento_3 = 0,
                    descuento_4 = 0,
                    descuento_5 = 0
                }
            };

            var json = string.Join(",", tarifasFcMedios.Select(e => string.Format("{{\"cod_programa\":{0},\"bruto\":{1},\"descuento_1\":{2},\"descuento_2\":{3},\"descuento_3\":{4},\"descuento_4\":{5},\"descuento_5\":{6},\"fecha_tarifa\":\"{7}\"}}",
               e.cod_programa, e.bruto, e.descuento_1, e.descuento_2, e.descuento_3, e.descuento_4, e.descuento_5, e.fecha_tarifa)).ToList());

            json = string.Format("[{0}]", json);

            var result = string.Format("{0}{1}", FcMediosTarifarioUrl, PostTarifasAction).PostJsonToUrl(json);

            if (!string.Equals(result, ErrorMessage))
            {
                throw new Exception(string.Format("Error en la sincronización con FC Medios: {0}", result));
            }

            #endregion

            PdmContext.SaveChanges();

            return Mapper.Map<Entities.Tarifa, Dtos.Tarifa>(entity);
        }

        public override Tarifa ToEntity(Dtos.Tarifa dto)
        {
            var entity = default(Tarifa);
            //var medio = PdmContext.Medios.Single(e => e.Id == dto.Medio.Id);
            //var plaza = PdmContext.Plazas.Single(e => e.Id == dto.Plaza.Id);
            //var vehiculo = PdmContext.Vehiculos.Single(e => e.Id == dto.Vehiculo.Id);

            if (!dto.Id.HasValue)
            {
                var tarifario = PdmContext.Tarifarios.Single(t => t.Id == dto.IdTarifario);

                entity = new Tarifa
                {
                    Tarifario = tarifario,
                    CreateDate = DateTime.Now,
                    CreatedBy = UsuarioLogged,
                    Enabled = true,
                    Descripcion = dto.Descripcion,
                    HoraDesde = dto.HoraDesde,
                    HoraHasta = dto.HoraHasta,
                    Lunes = dto.Lunes,
                    Martes = dto.Martes,
                    Miercoles = dto.Miercoles,
                    Jueves = dto.Jueves,
                    Viernes = dto.Viernes,
                    Sabado = dto.Sabado,
                    Domingo = dto.Domingo,
                    //  Medio = medio,
                    OrdenDeCompra = dto.OrdenDeCompra,
                    // Plaza = plaza,
                    //Vehiculo = vehiculo,
                    Importe = dto.Importe
                };
            }
            else
            {
                entity = PdmContext.Tarifas.Single(c => c.Id == dto.Id.Value);

                //    entity.Descripcion = dto.Descripcion;
                entity.UpdateDate = DateTime.Now;
                entity.UpdatedBy = UsuarioLogged;
                //entity.Descripcion = dto.Descripcion;
                //entity.HoraDesde = dto.HoraDesde;
                //entity.HoraHasta = dto.HoraHasta;
                //entity.Lunes = dto.Lunes;
                //entity.Martes = dto.Martes;
                //entity.Miercoles = dto.Miercoles;
                //entity.Jueves = dto.Jueves;
                //entity.Viernes = dto.Viernes;
                //entity.Sabado = dto.Sabado;
                //entity.Domingo = dto.Domingo;
                //entity.Medio = medio;
                entity.OrdenDeCompra = dto.OrdenDeCompra;
                //entity.Plaza = plaza;
                //entity.Vehiculo = vehiculo;
                entity.Importe = dto.Importe;
            }

            return entity;
        }

        public override void Validate(Dtos.Tarifa dto)
        {
        }

        public override IQueryable GetQuery(FilterTarifas filter)
        {
            var result = PdmContext
                        .Tarifas.Include(t => t.Vehiculo)
                        .Include(t => t.Medio)
                        .Include(t => t.Plaza)
                        .Where(t => t.Tarifario.Id == filter.TarifarioId).OrderBy(r => r.Medio.Nombre)
                        .AsQueryable();

            if (filter.Medios != null && filter.Medios.Any())
            {
                result = result.Where(r => filter.Medios.Contains(r.Medio.Id)).AsQueryable();
            }

            if (filter.Plazas != null && filter.Plazas.Any())
            {
                result = result.Where(r => filter.Plazas.Contains(r.Plaza.Id)).AsQueryable();
            }

            if (filter.Vehiculos != null && filter.Vehiculos.Any())
            {
                result = result.Where(r => filter.Vehiculos.Contains(r.Vehiculo.Id)).AsQueryable();
            }

            if (filter.HoraDesde.HasValue)
            {
                result = result.Where(r => r.HoraDesde == filter.HoraDesde).AsQueryable();
            }

            if (filter.HoraHasta.HasValue)
            {
                result = result.Where(r => r.HoraHasta == filter.HoraHasta).AsQueryable();
            }

            if (filter.HoraHasta.HasValue)
            {
                result = result.Where(r => r.HoraHasta == filter.HoraHasta).AsQueryable();
            }

            if (filter.Dias != null)
            {
                result = result.Where(r =>
                        (r.Lunes && filter.Lunes) ||
                        (r.Martes && filter.Martes) ||
                        (r.Miercoles && filter.Miercoles) ||
                        (r.Jueves && filter.Jueves) ||
                        (r.Viernes && filter.Viernes) ||
                        (r.Sabado && filter.Sabado) ||
                        (r.Domingo && filter.Domingo)
                        ).AsQueryable();
            }


            if (!string.IsNullOrEmpty(filter.MultiColumnSearchText))
            {
                filter.MultiColumnSearchText = filter.MultiColumnSearchText.ToLower();

                result = result.Where(r =>
                    (r.Medio != null && r.Medio.Nombre.ToLower().Contains(filter.MultiColumnSearchText)) ||
                    (r.Plaza != null && r.Plaza.Codigo.ToLower().Contains(filter.MultiColumnSearchText)) ||
                    (r.Vehiculo != null && r.Vehiculo.Nombre.ToLower().Contains(filter.MultiColumnSearchText)) ||
                    (r.Descripcion != null && r.Descripcion.ToLower().Contains(filter.MultiColumnSearchText))).AsQueryable();
            }

            return result;
        }

        #endregion

        public void SetValues(FilterTarifas filter, double? importe = null, int? oc = null, bool takeOld = false)
        {
            var tarifas = GetQuery(filter).OfType<Tarifa>().ToList();
            var tarifario = PdmContext.Tarifarios.Single(e => e.Id == filter.TarifarioId);
            var tarifasFcMedios = new List<TarifaFcMediosUpdate>();

            tarifas.ForEach(t =>
            {
                if (importe.HasValue)
                {
                    t.Importe = importe.Value;
                }

                if (oc.HasValue)
                {
                    t.OrdenDeCompra = oc.Value.ToString();
                }

                if (takeOld)
                {
                    t.Importe = t.ImporteOld;
                }

                t.UpdateDate = DateTime.Now;
                t.UpdatedBy = UsuarioLogged;

                if (t.Importe > 0)
                {
                    tarifasFcMedios.Add(new TarifaFcMediosUpdate
                    {
                        cod_programa = t.CodigoPrograma,
                        bruto = t.Importe,
                        descuento_1 = 0,
                        descuento_2 = 0,
                        descuento_3 = 0,
                        descuento_4 = 0,
                        descuento_5 = 0,
                        fecha_tarifa = tarifario.FechaDesde.ToString("yyyy-MM-dd 00:00:00"),
                    });
                }
            });

            #region Sync

            var json = string.Join(",", tarifasFcMedios.Select(e => string.Format("{{\"cod_programa\":{0},\"bruto\":{1},\"descuento_1\":{2},\"descuento_2\":{3},\"descuento_3\":{4},\"descuento_4\":{5},\"descuento_5\":{6},\"fecha_tarifa\":\"{7}\"}}",
                e.cod_programa, e.bruto, e.descuento_1, e.descuento_2, e.descuento_3, e.descuento_4, e.descuento_5, e.fecha_tarifa)).ToList());

            json = string.Format("[{0}]", json);

            var result = string.Format("{0}{1}", FcMediosTarifarioUrl, PostTarifasAction).PostJsonToUrl(json);

            if (!string.Equals(result, ErrorMessage))
            {
                throw new Exception(string.Format("Error en la sincronización con FC Medios: {0}", result));
            }

            #endregion

            PdmContext.BulkSaveChanges();
        }
    }
}

