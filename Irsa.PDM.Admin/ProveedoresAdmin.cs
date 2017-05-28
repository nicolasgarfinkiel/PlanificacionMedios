using System;
using System.Collections.Generic;
using System.Linq;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Entities;

namespace Irsa.PDM.Admin
{
    public class ProveedoresAdmin : BaseAdmin<int, Entities.Proveedor, Dtos.Proveedor, FilterBase>
    {
        #region Base

        public override Proveedor ToEntity(Dtos.Proveedor dto)
        {
            var entity = default(Proveedor);            

            if (!dto.Id.HasValue)
            {
                entity = new Proveedor
                {                 
                    CreateDate = DateTime.Now,
                    CreatedBy = UsuarioLogged,                                        
                    Enabled = true,                    
                    Nombre =  dto.Nombre,
                    Vehiculos = new List<Vehiculo>()
                };
            }
            else
            {
                entity = PdmContext.Proveedores.Single(c => c.Id == dto.Id.Value);
                entity.Nombre = dto.Nombre;                
                entity.UpdateDate = DateTime.Now;
                entity.UpdatedBy = UsuarioLogged;                
            }

            if (dto.Vehiculos == null)
            {
                dto.Vehiculos = new List<Dtos.Vehiculo>();
            }

            var currentVehiculosId = entity.Vehiculos.Select(t => t.Id).ToList();
            var newVehiculosId = dto.Vehiculos.Select(t => t.Id.Value).ToList();
            var vehiculosToAddId = newVehiculosId.Except(currentVehiculosId).ToList();
            var vehiculosToDeleteId = currentVehiculosId.Except(newVehiculosId).ToList();

            vehiculosToAddId.ForEach(id =>
            {
                var vehiuclo = PdmContext.Vehiculos.Single(a => a.Id == id);                
                entity.Vehiculos.Add(vehiuclo);
            });

            vehiculosToDeleteId.ForEach(id =>
            {
                var vehiculo = entity.Vehiculos.Single(t => t.Id == id);                
                entity.Vehiculos.Remove(vehiculo);
            });            

            return entity;
        }

        public override void Validate(Dtos.Proveedor dto)
        {
            var entity = PdmContext.Proveedores.FirstOrDefault(m => m.Nombre.ToLower().Equals(dto.Nombre.ToLower()));

            if (entity != null && entity.Id != dto.Id)
            {
                throw new Exception("Ya existe otro proveedor con el mismo nombre");
            }
        }

        public override IQueryable GetQuery(FilterBase filter)
        {
            var result = PdmContext.Proveedores.OrderBy(m => m.Nombre).AsQueryable();            

            if (!string.IsNullOrEmpty(filter.MultiColumnSearchText))
            {
                filter.MultiColumnSearchText = filter.MultiColumnSearchText.ToLower();

                result = result.Where(r =>
                    (r.Nombre != null && r.Nombre.ToLower().Contains(filter.MultiColumnSearchText)) 
                    ).AsQueryable();
            }

            return result;
        }

        #endregion
        
    }
}
