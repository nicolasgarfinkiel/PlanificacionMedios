using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using AutoMapper;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Repositories;
using ServiceStack.ServiceClient.Web;

namespace Irsa.PDM.Admin
{
    public abstract class BaseAdmin<TID, TE, TD, TF> where TF: FilterBase
    {
        public static string FcMediosTarifarioUrl = ConfigurationManager.AppSettings["fcMediosTarifarioUrl"];
        public  PDMContext PdmContext;
        public string UsuarioLogged { get; set; }
        public JsonServiceClient FCMediosClient;
        

        public BaseAdmin()
        {          
            PdmContext = new PDMContext();
            FCMediosClient = new JsonServiceClient(FcMediosTarifarioUrl);
            //FCMediosclient.Credentials = NetWorkCredential;
            //FCMediosclient.Proxy = WebProxy;

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

        public string GetExceptionDetail(Exception ex)
        {
            var sb = new StringBuilder();

            sb.Append(ex.StackTrace);
            sb.Append(" | ");

            while (ex != null)
            {
                sb.Append(ex.Message);
                sb.Append(" | ");
                ex = ex.InnerException;
            }

            return sb.ToString();
        }

        #region Abstract Methods

        public abstract TE ToEntity(TD dto);
        public abstract void Validate(TD dto);
        public abstract IQueryable GetQuery(TF filter);

        #endregion

        #region Credenciales
        public NetworkCredential NetWorkCredential
        {
            get
            {
                return new NetworkCredential("procmailer", "prc01mail#07snd", "IRSACORP");
            }
        }

        public WebProxy WebProxy
        {
            get
            {
                WebProxy proxy = new WebProxy("http://10.100.250.2:8080");
                proxy.Credentials = NetWorkCredential;

                return proxy;
            }
        }
        #endregion
    }
}
