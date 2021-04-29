using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VirtoCommerce.AModule.Data.Repositories
{
    class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ADbContext>
    {
        public ADbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ADbContext>();

            builder.UseSqlServer("Data Source=tv.;Initial Catalog=VirtoCommerce3;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30");

            return new ADbContext(builder.Options);
        }

    }
}
