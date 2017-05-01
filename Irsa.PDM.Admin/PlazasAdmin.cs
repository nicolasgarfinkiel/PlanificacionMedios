using System;
using System.Linq;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Entities;

namespace Irsa.PDM.Admin
{
    public class PlazasAdmin : BaseAdmin<int, Entities.Plaza, Dtos.Plaza, FilterBase>
    {
        #region Base

        public override Plaza ToEntity(Dtos.Plaza dto)
        {
            var entity = default(Plaza);            

            if (!dto.Id.HasValue)
            {
                entity = new Plaza
                {                 
                    CreateDate = DateTime.Now,
                    CreatedBy = UsuarioLogged,                                        
                    Enabled = true,                    
                    Codigo = dto.Codigo,
                    Descripcion = dto.Descripcion                    
                };
            }
            else
            {
                entity = PdmContext.Plazas.Single(c => c.Id == dto.Id.Value);
             
                entity.Codigo = dto.Codigo;
                entity.Descripcion = dto.Descripcion;
                entity.UpdateDate = DateTime.Now;
                entity.UpdatedBy = UsuarioLogged;                
            }

            return entity;
        }

        public override void Validate(Dtos.Plaza dto)
        {
            var entity = PdmContext.Plazas.FirstOrDefault(m => m.Codigo.ToLower().Equals(dto.Codigo.ToLower()));

            if (entity != null && entity.Id != dto.Id)
            {
                throw new Exception("Ya existe otro plaza con el mismo nombre");
            }
        }

        public override IQueryable GetQuery(FilterBase filter)
        {
            var result = PdmContext.Plazas.OrderBy(m => m.Descripcion).AsQueryable();            

            if (!string.IsNullOrEmpty(filter.MultiColumnSearchText))
            {
                filter.MultiColumnSearchText = filter.MultiColumnSearchText.ToLower();

                result = result.Where(r =>
                    (r.Codigo != null && r.Codigo.ToLower().Contains(filter.MultiColumnSearchText)) ||
                    (r.Descripcion != null && r.Descripcion.ToLower().Contains(filter.MultiColumnSearchText))).AsQueryable();
            }

            return result;
        }

        #endregion
        
    }
}
