using System;
using System.Linq;
using AutoMapper;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Entities;

namespace Irsa.PDM.Admin
{
    public class LogAdmin : BaseAdmin<int, Entities.Log, Dtos.Log, FilterBase>
    {
        #region Base

        public override Log ToEntity(Dtos.Log dto)
        {
            var result =  Mapper.Map<Dtos.Log, Entities.Log>(dto);
            result.Enabled = true;
            return result;
        }

        public override void Validate(Dtos.Log dto)
        {            
        }

        public override IQueryable GetQuery(FilterBase filter)
        {
            throw new NotImplementedException();
        }

        #endregion

       
    }
}
