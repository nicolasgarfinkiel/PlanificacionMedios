using System;
using System.Collections.Generic;
using System.Linq;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Entities;

namespace Irsa.PDM.Admin
{
    public class MediosAdmin : BaseAdmin<int, Entities.Medio, Dtos.Medio, FilterBase>
    {
        #region Base

        public override Medio ToEntity(Dtos.Medio dto)
        {
            var entity = default(Medio);
          //  var tipoEspacio = (TipoEspacio) Enum.Parse(typeof(TipoEspacio), dto.TipoEspacio);

            if (!dto.Id.HasValue)
            {
                entity = new Medio
                {                 
                    CreateDate = DateTime.Now,
                    CreatedBy = UsuarioLogged,                                        
                    Enabled = true,                    
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                 //   TipoEspacio = tipoEspacio
                };
            }
            else
            {
                entity = PdmContext.Medios.Single(c => c.Id == dto.Id.Value);
             
                entity.Nombre = dto.Nombre;
                entity.Descripcion = dto.Descripcion;
                entity.UpdateDate = DateTime.Now;
                entity.UpdatedBy = UsuarioLogged;
                //entity.TipoEspacio = tipoEspacio;
            }

            return entity;
        }

        public override void Validate(Dtos.Medio dto)
        {
            var entity = PdmContext.Medios.FirstOrDefault(m => m.Nombre.ToLower().Equals(dto.Nombre.ToLower()));

            if (entity != null && entity.Id != dto.Id)
            {
                throw new Exception("Ya existe otro medio con el mismo nombre");
            }
        }

        public override IQueryable GetQuery(FilterBase filter)
        {
            var result = PdmContext.Medios.OrderBy(m => m.Nombre).AsQueryable();            

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

        public IEnumerable<string> GetTipoEspacioList()
        {
            return Enum.GetNames(typeof(TipoEspacio)).OrderBy(t => t);
        }
    }
}
