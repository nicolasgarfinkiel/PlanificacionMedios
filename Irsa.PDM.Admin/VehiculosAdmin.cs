using System;
using System.Linq;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Entities;

namespace Irsa.PDM.Admin
{
    public class VehiculosAdmin : BaseAdmin<int, Entities.Vehiculo, Dtos.Vehiculo, FilterBase>
    {
        #region Base

        public override Vehiculo ToEntity(Dtos.Vehiculo dto)
        {
            var entity = default(Vehiculo);
            var medio = PdmContext.Medios.Single(m => m.Id == dto.Medio.Id);

            if (!dto.Id.HasValue)
            {
                entity = new Vehiculo
                {                 
                    CreateDate = DateTime.Now,
                    CreatedBy = UsuarioLogged,
                    Enabled = true,
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Medio = medio
                };
            }
            else
            {
                entity = PdmContext.Vehiculos.Single(c => c.Id == dto.Id.Value);
             
                entity.Nombre = dto.Nombre;
                entity.Descripcion = dto.Descripcion;
                entity.UpdateDate = DateTime.Now;
                entity.UpdatedBy = UsuarioLogged;
                entity.Medio = medio;
            }

            return entity;
        }

        public override void Validate(Dtos.Vehiculo dto)
        {
            var entity = PdmContext.Vehiculos.FirstOrDefault(m => m.Nombre.ToLower().Equals(dto.Nombre.ToLower()));

            if (entity != null && entity.Id != dto.Id)
            {
                throw new Exception("Ya existe otro vehículo con el mismo nombre");
            }
        }

        public override IQueryable GetQuery(FilterBase filter)
        {
            var result = PdmContext.Vehiculos.OrderBy(m => m.Descripcion).AsQueryable();            

            if (!string.IsNullOrEmpty(filter.MultiColumnSearchText))
            {
                filter.MultiColumnSearchText = filter.MultiColumnSearchText.ToLower();

                result = result.Where(r =>
                    (r.Medio != null && r.Medio.Nombre.ToLower().Contains(filter.MultiColumnSearchText)) ||
                    (r.Nombre != null && r.Nombre.ToLower().Contains(filter.MultiColumnSearchText)) ||
                    (r.Descripcion != null && r.Descripcion.ToLower().Contains(filter.MultiColumnSearchText))).AsQueryable();
            }

            return result;
        }

        #endregion
        
    }
}
