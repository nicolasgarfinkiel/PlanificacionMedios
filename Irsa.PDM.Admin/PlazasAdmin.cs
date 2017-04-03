using System;
using System.Collections.Generic;
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
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion                    
                };
            }
            else
            {
                entity = PdmContext.Plazas.Single(c => c.Id == dto.Id.Value);
             
                entity.Nombre = dto.Nombre;
                entity.Descripcion = dto.Descripcion;
                entity.UpdateDate = DateTime.Now;
                entity.UpdatedBy = UsuarioLogged;                
            }

            return entity;
        }

        public override void Validate(Dtos.Plaza dto)
        {
            var entity = PdmContext.Plazas.FirstOrDefault(m => m.Nombre.ToLower().Equals(dto.Nombre.ToLower()));

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
                    (r.Nombre != null && r.Nombre.ToLower().Contains(filter.MultiColumnSearchText)) ||
                    (r.Descripcion != null && r.Descripcion.ToLower().Contains(filter.MultiColumnSearchText))).AsQueryable();
            }

            return result;
        }

        #endregion
        
    }
}
