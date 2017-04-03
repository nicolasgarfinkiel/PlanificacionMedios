using AutoMapper;
using Irsa.PDM.Entities;

namespace Irsa.PDM.Admin
{
    public static class BootStrapper
    {
        public static void BootStrap()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Medio, Dtos.Medio>();
                cfg.CreateMap<Plaza, Dtos.Plaza>();              
            });         
        }
    }
}

