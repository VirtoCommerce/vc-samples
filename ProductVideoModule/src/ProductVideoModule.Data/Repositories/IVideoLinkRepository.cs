using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductVideoModule.Data.Models;
using VirtoCommerce.Platform.Core.Common;

namespace ProductVideoModule.Data.Repositories
{
    public interface IVideoLinkRepository : IRepository
    {
        IQueryable<VideoLinkEntity> VideoLinks { get; }
        Task<VideoLinkEntity[]> GetByIdsAsync(IEnumerable<string> Ids);
        Task DeleteVideoLinksAsync(IEnumerable<string> Ids);
    }
}
