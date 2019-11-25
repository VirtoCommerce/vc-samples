using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace DummyModule.Data.Repositories
{
    public class DummyDbContext : DbContextWithTriggers
    {
        public DummyDbContext(DbContextOptions<DummyDbContext> options)
            : base(options)
        {
        }

        protected DummyDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
