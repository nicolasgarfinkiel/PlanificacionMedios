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
                cfg.CreateMap<Vehiculo, Dtos.Vehiculo>();
                cfg.CreateMap<Tarifa, Dtos.Tarifa>();
                cfg.CreateMap<Tarifario, Dtos.Tarifario>();
                cfg.CreateMap<Tarifario, Dtos.TarifarioEdit>();
            });         
        }
    }
}

