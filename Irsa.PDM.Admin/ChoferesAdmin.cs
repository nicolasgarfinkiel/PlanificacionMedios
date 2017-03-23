//using System;
//using System.Linq;
//using Cresud.CDP.Dtos.Common;
//using Cresud.CDP.Dtos.Filters;
//using Cresud.CDP.Entities;
//using Irsa.PDM.Admin;

//namespace Cresud.CDP.Admin
//{
//    public class ChoferesAdmin : BaseAdmin<int, Entities.Chofer, Dtos.Chofer, FilterChoferes>
//    {
//        #region Base

//        public override Chofer ToEntity(Dtos.Chofer dto)
//        {
//            var entity = default(Chofer);

//            if (!dto.Id.HasValue)
//            {
//                var grupoEmpresa = PdmContext.GrupoEmpresas.Single(g => g.Id == dto.GrupoEmpresaId);

//                entity = new Chofer
//                {
//                    Acoplado = dto.Acoplado,
//                    Apellido = dto.Apellido,
//                    Camion = dto.Camion,
//                    CreateDate = DateTime.Now,
//                    CreatedBy = UsuarioLogged,
//                    Cuit = dto.Cuit,
//                    Domicilio = dto.Domicilio,
//                    Enabled = true,
//                    EsChoferTransportista = dto.EsChoferTransportista,
//                    GrupoEmpresa = grupoEmpresa,
//                    Marca = dto.Marca,
//                    Nombre = dto.Nombre
//                };
//            }
//            else
//            {
//                entity = PdmContext.Choferes.Single(c => c.Id == dto.Id.Value);
//                entity.Acoplado = dto.Acoplado;
//                entity.Apellido = dto.Apellido;
//                entity.Camion = dto.Camion;
//                entity.Cuit = dto.Cuit;
//                entity.Domicilio = dto.Domicilio;
//                entity.EsChoferTransportista = dto.EsChoferTransportista;
//                entity.Marca = dto.Marca;
//                entity.Nombre = dto.Nombre;
//                entity.UpdateDate = DateTime.Now;
//                entity.UpdatedBy = UsuarioLogged;
//            }

//            return entity;
//        }

//        public override void Validate(Dtos.Chofer dto)
//        {
//            var entity = PdmContext.Choferes.FirstOrDefault(c => string.Equals(c.Cuit, dto.Cuit));

//            if(entity != null && entity.Id != dto.Id) 
//                throw new Exception("El cuit ya se encuentra asignado a otro chofer");
//        }

//        public override IQueryable GetQuery(FilterChoferes filter)
//        {
//            var result = PdmContext.Choferes.Where(c => c.GrupoEmpresa.Id == filter.IdGrupoEmpresa).OrderBy(c => c.Nombre).ThenBy(c => c.Apellido).AsQueryable();

//            if (filter.EsChoferTransportista.HasValue)
//            {
//                result = result.Where(c => c.EsChoferTransportista == filter.EsChoferTransportista.Value).AsQueryable();
//            }

//            if (!string.IsNullOrEmpty(filter.MultiColumnSearchText))
//            {
//                filter.MultiColumnSearchText = filter.MultiColumnSearchText.ToLower();

//                result = result.Where(r => 
//                    (r.Nombre != null && r.Nombre.ToLower().Contains(filter.MultiColumnSearchText)) ||
//                    (r.Apellido != null && r.Apellido.ToLower().Contains(filter.MultiColumnSearchText)) ||
//                    (r.Cuit != null && r.Cuit.ToLower().Contains(filter.MultiColumnSearchText))).AsQueryable();
//            }

//            return result;
//        }

//        #endregion
//    }
//}
