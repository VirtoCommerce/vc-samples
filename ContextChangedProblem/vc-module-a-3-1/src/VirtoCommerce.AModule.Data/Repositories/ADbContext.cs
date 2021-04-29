using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.AModule.Data.Model;


namespace VirtoCommerce.AModule.Data.Repositories
{
    public class ADbContext : DbContextWithTriggers
    {
        public ADbContext(DbContextOptions<ADbContext> options)
    : base(options)
        {
        }

        protected ADbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SeamlessEntity>().ToTable("Seamless").HasKey(x => x.Id);
            modelBuilder.Entity<SeamlessEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<SeamlessEntity>().Property(x => x.Numeric).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<SeamlessEntity>().HasDiscriminator<string>("Discriminator");
            modelBuilder.Entity<SeamlessEntity>().Property("Discriminator").HasMaxLength(128);


            base.OnModelCreating(modelBuilder);
        }
    }
}
