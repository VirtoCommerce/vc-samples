using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VirtoCommerce.BModule.Data.Repositories
{
    class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BDbContext>
    {
        public BDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BDbContext>();

            builder.UseSqlServer("Data Source=tv.;Initial Catalog=VirtoCommerce3;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30");

            return new BDbContext(builder.Options);
        }

    }
}
