using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProductVideoModule.Data.Repositories;

namespace DummyModule.Data.Repositories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProductVideoDbContext>
    {
        public ProductVideoDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ProductVideoDbContext>();

            builder.UseSqlServer("Data Source=localhost/MYST;Initial Catalog=VirtoCommerce;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30");

            return new ProductVideoDbContext(builder.Options);
        }
    }
}

