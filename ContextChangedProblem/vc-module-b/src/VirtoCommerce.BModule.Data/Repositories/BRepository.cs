using System.Linq;
using VirtoCommerce.AModule.Data.Repositories;
using VirtoCommerce.BModule.Data.Model;
using VirtoCommerce.Platform.Data.Infrastructure;


namespace VirtoCommerce.BModule.Data.Repositories
{
    public class BRepository : ARepository
    {
        public BRepository(BDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<SeamlessEntity2> Seamlesses2 => DbContext.Set<SeamlessEntity2>();
    }
}
