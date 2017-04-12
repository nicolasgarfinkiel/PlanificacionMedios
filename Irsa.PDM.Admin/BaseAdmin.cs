﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Repositories;

namespace Irsa.PDM.Admin
{
    public abstract class BaseAdmin<TID, TE, TD, TF> where TF: FilterBase
    {
        public readonly PDMContext PdmContext;
        public string UsuarioLogged { get; set; }

        public BaseAdmin()
        {
            PdmContext = new PDMContext();

            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Request.IsAuthenticated)
                {
                    UsuarioLogged = HttpContext.Current.User.Identity.Name;
                }
            }
            catch (Exception)
            {
            }

        }

        public virtual TD GetById(TID id)
        {
            var entity = (TE)PdmContext.Set(typeof(TE)).Find(id);
            return Mapper.Map<TE, TD>(entity);
        }

        public virtual IList<TD> GetAll()
        {
             var entities = (IList<TE>)PdmContext.Set(typeof(TE)).AsQueryable().OfType<TE>().ToList();
             return Mapper.Map<IList<TE>, IList<TD>>(entities);
        }

        public virtual PagedListResponse<TD> GetByFilter(TF filter)
        {          
            var query = GetQuery(filter).OfType<TE>();
            
            return new PagedListResponse<TD>
            {
                Count = query.Count(),
                Data = Mapper.Map<IList<TE>, IList<TD>>(query.Skip(filter.PageSize * (filter.CurrentPage - 1)).Take(filter.PageSize).ToList())
            };
        }

        public virtual TD Create(TD dto)
        {
            Validate(dto);
            var entity = ToEntity(dto);

            PdmContext.Set(typeof(TE)).Add(entity);
            PdmContext.SaveChanges();

            return Mapper.Map<TE, TD>(entity);
        }

        public virtual TD Update(TD dto)
        {
            Validate(dto);
            var entity = ToEntity(dto);
            PdmContext.SaveChanges();

            return Mapper.Map<TE, TD>(entity);
        }

        public virtual void Delete(TID id)
        {
            var entity = (TE)PdmContext.Set(typeof(TE)).Find(id);
            PdmContext.Set(typeof (TE)).Remove(entity);

            PdmContext.SaveChanges();            
        }

        #region Abstract Methods

        public abstract TE ToEntity(TD dto);
        public abstract void Validate(TD dto);
        public abstract IQueryable GetQuery(TF filter);

        #endregion
    }
}