using System;
using System.Linq;
using Irsa.PDM.Dtos.Filters;
using Irsa.PDM.Entities;

namespace Irsa.PDM.Admin
{
    public class TarifariosAdmin : BaseAdmin<int, Entities.Tarifario, Dtos.Tarifario, FilterTarifarios>
    {
        #region Base

        public override Tarifario ToEntity(Dtos.Tarifario dto)
        {
            var entity = default(Tarifario);
          
            if (!dto.Id.HasValue)
            {
                entity = new Tarifario
                {
                    CreateDate = DateTime.Now,
                    CreatedBy = UsuarioLogged,
                    Enabled = true,
                    FechaDesde = dto.FechaDesde,
                    FechaHasta = dto.FechaHasta,
                };
            }
            else
            {
                entity = PdmContext.Tarifario.Single(c => c.Id == dto.Id.Value);
                
                entity.UpdateDate = DateTime.Now;
                entity.UpdatedBy = UsuarioLogged;
                entity.FechaDesde = dto.FechaDesde;
                entity.FechaHasta = dto.FechaHasta;
            }

            return entity;
        }

        public override void Validate(Dtos.Tarifario dto)
        {
        }

        public override IQueryable GetQuery(FilterTarifarios filter)
        {
            var result = PdmContext.Tarifario.OrderBy(r => r.FechaDesde).AsQueryable();
          

            if (filter.FechaDesde.HasValue)
            {
                result = result.Where(r => r.FechaDesde >= filter.FechaDesde).AsQueryable();
            }

            if (filter.FechaHasta.HasValue)
            {
                var fechaHasta = filter.FechaHasta.Value.AddDays(1).AddMilliseconds(-1);
                result = result.Where(r => r.FechaDesde <= fechaHasta).AsQueryable();
            }                      

            return result;
        }

        #endregion

    }
}
