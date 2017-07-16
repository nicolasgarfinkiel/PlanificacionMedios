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
                cfg.CreateMap<Campania, Dtos.Campania>();
                cfg.CreateMap<Pauta, Dtos.Pauta>();
                cfg.CreateMap<PautaItem, Dtos.PautaItem>();
                cfg.CreateMap<Proveedor, Dtos.Proveedor>();
                cfg.CreateMap<Certificacion, Dtos.Certificacion>();
                cfg.CreateMap<Log, Dtos.Log>();
                cfg.CreateMap<Dtos.Log, Log>();
            });         
        }
    }
}

