using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductVideoModule.Data.Models;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace ProductVideoModule.Data.Repositories
{
    public class VideoLinkRepsitoryBase : DbContextRepositoryBase<ProductVideoDbContext>, IVideoLinkRepository
    {
        public VideoLinkRepsitoryBase(ProductVideoDbContext dbContext) : base(dbContext)
        {
        }

        #region Members

        public IQueryable<VideoLinkEntity> VideoLinks/* => DbContext.Set<VideoLinkEntity>();*/
        {
            get
            {
                return DbContext.Set<VideoLinkEntity>();
            }
        }

        #endregion

        public Task<VideoLinkEntity[]> GetByIdsAsync(IEnumerable<string> Ids)
        {
            return VideoLinks.Where(x => Ids.Contains(x.Id)).ToArrayAsync();
        }

        public async Task DeleteVideoLinksAsync(IEnumerable<string> Ids)
        {
            var deleted = await GetByIdsAsync(Ids);
            foreach (var member in deleted)
            {
                Remove(member);
            }
        }
    }
}
