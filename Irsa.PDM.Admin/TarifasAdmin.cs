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
            var medio = PdmContext.Medios.Single(e => e.Id == dto.MedioId);
            var plaza = PdmContext.Plazas.Single(e => e.Id == dto.PlazaId);
            var vehiculo = PdmContext.Vehiculos.Single(e => e.Id == dto.VehiculoId);

            if (!dto.Id.HasValue)
            {
                entity = new Tarifa
                {                 
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
                    Vehiculo = vehiculo                     
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
            }

            return entity;
        }

        public override void Validate(Dtos.Tarifa dto)
        {          
        }

        public override IQueryable GetQuery(FilterTarifas filter)
        {
            var result = PdmContext.Tarifas.OrderBy(r => r.Medio.Nombre).AsQueryable();

            if (filter.Medios!= null && filter.Medios.Any())
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
                if (filter.Lunes.HasValue)
                {
                    result = result.Where(r => r.Lunes == filter.Lunes).AsQueryable();
                }

                if (filter.Martes.HasValue)
                {
                    result = result.Where(r => r.Lunes == filter.Martes).AsQueryable();
                }

                if (filter.Miercoles.HasValue)
                {
                    result = result.Where(r => r.Lunes == filter.Miercoles).AsQueryable();
                }

                if (filter.Jueves.HasValue)
                {
                    result = result.Where(r => r.Lunes == filter.Jueves).AsQueryable();
                }

                if (filter.Viernes.HasValue)
                {
                    result = result.Where(r => r.Lunes == filter.Viernes).AsQueryable();
                }

                if (filter.Sabado.HasValue)
                {
                    result = result.Where(r => r.Lunes == filter.Sabado).AsQueryable();
                }

                if (filter.Domingo.HasValue)
                {
                    result = result.Where(r => r.Lunes == filter.Domingo).AsQueryable();
                }
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
