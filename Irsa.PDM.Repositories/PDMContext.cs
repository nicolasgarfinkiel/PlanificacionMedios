﻿using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Irsa.PDM.Entities;

namespace Irsa.PDM.Repositories
{
    public class PDMContext : DbContext
    {
        public PDMContext()
            : base(ConfigurationManager.ConnectionStrings["PDM"].ConnectionString)
        {     
          //  Database.SetInitializer<PDMContext>(null);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PDMContext, Migrations.Configuration>("PDM"));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
        }

        public IDbSet<Medio> Medios { get; set; }
        public IDbSet<Plaza> Plazas { get; set; }
        public IDbSet<Vehiculo> Vehiculos { get; set; }
        public IDbSet<Tarifario> Tarifario { get; set; }
        public IDbSet<Tarifa> Tarifas { get; set; }

    }
}
