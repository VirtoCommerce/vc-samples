using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.AModule.Data.Repositories;
using VirtoCommerce.BModule.Data.Model;


namespace VirtoCommerce.BModule.Data.Repositories
{
    public class BDbContext : ADbContext
    {
        public BDbContext(DbContextOptions<BDbContext> options)
    : base(options)
        {
        }

        protected BDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SeamlessEntity2>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
