using System;
using System.Linq;
using Irsa.PDM.Dtos.Filters;
using Irsa.PDM.Entities;

namespace Irsa.PDM.Admin
{
    public class TarifasAdmin : BaseAdmin<int, Entities.Tarifa, Dtos.Tarifa, FilterTarifas>
    {
        #region Base

        public override Tarifa ToEntity(Dtos.Tarifa dto)
        {
            var entity = default(Tarifa);
            var medio = PdmContext.Medios.Single(e => e.Id == dto.Medio.Id);
            var plaza = PdmContext.Plazas.Single(e => e.Id == dto.Plaza.Id);
            var vehiculo = PdmContext.Vehiculos.Single(e => e.Id == dto.Vehiculo.Id);

            if (!dto.Id.HasValue)
            {
                var tarifario = PdmContext.Tarifario.Single(t => t.Id == dto.IdTarifario);

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
                    Medio = medio,
                    OrdenDeCompra = dto.OrdenDeCompra,
                    Plaza = plaza,
                    Vehiculo = vehiculo,
                    Importe = dto.Importe
                };
            }
            else
            {
                entity = PdmContext.Tarifas.Single(c => c.Id == dto.Id.Value);

                entity.Descripcion = dto.Descripcion;
                entity.UpdateDate = DateTime.Now;
                entity.UpdatedBy = UsuarioLogged;
                entity.Descripcion = dto.Descripcion;
                entity.HoraDesde = dto.HoraDesde;
                entity.HoraHasta = dto.HoraHasta;
                entity.Lunes = dto.Lunes;
                entity.Martes = dto.Martes;
                entity.Miercoles = dto.Miercoles;
                entity.Jueves = dto.Jueves;
                entity.Viernes = dto.Viernes;
                entity.Sabado = dto.Sabado;
                entity.Domingo = dto.Domingo;
                entity.Medio = medio;
                entity.OrdenDeCompra = dto.OrdenDeCompra;
                entity.Plaza = plaza;
                entity.Vehiculo = vehiculo;
                entity.Importe = dto.Importe;
            }

            return entity;
        }

        public override void Validate(Dtos.Tarifa dto)
        {
        }

        public override IQueryable GetQuery(FilterTarifas filter)
        {
            var result = PdmContext.Tarifas.Where(t => t.Tarifario.Id == filter.TarifarioId).OrderBy(r => r.Medio.Nombre).AsQueryable();

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
                    (r.Plaza != null && r.Plaza.Nombre.ToLower().Contains(filter.MultiColumnSearchText)) ||
                    (r.Vehiculo != null && r.Vehiculo.Nombre.ToLower().Contains(filter.MultiColumnSearchText)) ||
                    (r.Descripcion != null && r.Descripcion.ToLower().Contains(filter.MultiColumnSearchText))).AsQueryable();
            }

            return result;
        }

        #endregion

    }
}
