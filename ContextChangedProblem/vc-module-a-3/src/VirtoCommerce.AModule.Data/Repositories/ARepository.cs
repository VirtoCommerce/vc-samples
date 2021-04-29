using System.Linq;
using VirtoCommerce.AModule.Data.Model;
using VirtoCommerce.Platform.Data.Infrastructure;


namespace VirtoCommerce.AModule.Data.Repositories
{
    public class ARepository : DbContextRepositoryBase<ADbContext>
    {
        public ARepository(ADbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<SeamlessEntity> Seamlesses => DbContext.Set<SeamlessEntity>();
    }
}
