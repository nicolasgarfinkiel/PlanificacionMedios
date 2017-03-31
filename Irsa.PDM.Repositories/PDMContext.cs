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
            modelBuilder.Entity<Medio>().HasKey(t => t.Id);
            modelBuilder.Entity<Medio>().Property(t => t.Id).HasColumnName("Id");
            modelBuilder.Entity<Medio>().Property(t => t.Nombre).HasColumnName("Nombre");
            modelBuilder.Entity<Medio>().Property(t => t.Descripcion).HasColumnName("Descripcion");
            modelBuilder.Entity<Medio>().ToTable("Medio");      
        }

        public IDbSet<Medio> Medios { get; set; }

    }
}

