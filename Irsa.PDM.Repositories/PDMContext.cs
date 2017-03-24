using System.Configuration;
using System.Data.Entity;
using Irsa.PDM.Entities;

namespace Irsa.PDM.Repositories
{
    public class PDMContext : DbContext
    {
        public PDMContext()
            : base(ConfigurationManager.ConnectionStrings["PDM"].ConnectionString)
        {
            Database.SetInitializer<PDMContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Empresa>().HasKey(t => t.Id);
            //modelBuilder.Entity<Empresa>().Property(t => t.Id).HasColumnName("IdEmpresa");
            //modelBuilder.Entity<Empresa>().Property(t => t.IdCliente).HasColumnName("IdCliente");
            //modelBuilder.Entity<Empresa>().Property(t => t.IdSapCanalExpor).HasColumnName("IdSapCanalExpor");
            //modelBuilder.Entity<Empresa>().Property(t => t.IdSapCanalLocal).HasColumnName("IdSapCanalLocal");
            //modelBuilder.Entity<Empresa>().Property(t => t.IdSapMoneda).HasColumnName("IdSapMoneda");
            //modelBuilder.Entity<Empresa>().Property(t => t.IdSapOrganizacionDeVenta).HasColumnName("IdSapOrganizacionDeVenta");
            //modelBuilder.Entity<Empresa>().Property(t => t.IdSapSector).HasColumnName("IdSapSector");
            //modelBuilder.Entity<Empresa>().Property(t => t.SapId).HasColumnName("Sap_Id");
            //modelBuilder.Entity<Empresa>().HasOptional(e => e.GrupoEmpresa).WithMany().Map(x => x.MapKey("IdGrupoEmpresa"));
            //modelBuilder.Entity<Empresa>().ToTable("Empresa");      
        }
      
     
    }
}

