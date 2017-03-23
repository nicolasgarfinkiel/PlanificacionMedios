using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Common;

namespace Irsa.PDM.Admin
{
    public class EmpresaAdmin : BaseAdmin<int, Empresa, Dtos.Empresa, FilterBase>
    {
        #region Base

        public override Empresa ToEntity(Dtos.Empresa dto)
        {
            return null;
        }

        public override void Validate(Dtos.Empresa dto)
        {
        }

        public override PagedListResponse<Dtos.Empresa> GetByFilter(FilterBase filter)
        {
            var query = GetQuery(filter).OfType<Empresa>();
            var dtos = Mapper.Map<IList<Empresa>, IList<Dtos.Empresa>>(query.Skip(filter.PageSize * (filter.CurrentPage - 1)).Take(filter.PageSize).ToList());           

            return new PagedListResponse<Dtos.Empresa>
            {
                Count = query.Count(),
                Data = dtos
            };
        }

        public override IQueryable GetQuery(FilterBase filter)
        {
            var result = PdmContext.Empresas.OrderBy(c => c.Descripcion).AsQueryable();

            if (!string.IsNullOrEmpty(filter.MultiColumnSearchText))
            {
                filter.MultiColumnSearchText = filter.MultiColumnSearchText.ToLower();

                result = result.Where(r =>
                    (r.Descripcion != null && r.Descripcion.ToLower().Contains(filter.MultiColumnSearchText)) ||
                    (r.IdSapMoneda != null && r.IdSapMoneda.ToLower().Contains(filter.MultiColumnSearchText)) ||
                    (r.GrupoEmpresa != null && r.GrupoEmpresa.Descripcion.ToLower().Contains(filter.MultiColumnSearchText)) ||
                    (r.GrupoEmpresa != null && r.GrupoEmpresa.Pais.Descripcion.ToLower().Contains(filter.MultiColumnSearchText))
                    ).AsQueryable();
            }

            return result;
        }

        #endregion       
    }
}
