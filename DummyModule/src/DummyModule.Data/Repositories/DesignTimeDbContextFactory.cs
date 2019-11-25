using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DummyModule.Data.Repositories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DummyDbContext>
    {
        public DummyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DummyDbContext>();

            builder.UseSqlServer("Data Source=(local);Initial Catalog=VirtoCommerce3;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30");

            return new DummyDbContext(builder.Options);
        }
    }
}
