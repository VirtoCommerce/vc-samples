using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProductVideoModule.Data.Models;
using VirtoCommerce.Platform.Core.Common;

namespace ProductVideoModule.Data.Repositories
{
    public interface IVideoLinkRepository : IRepository
    {
        IQueryable<VideoLinkEntity> VideoLinks { get; }
    }
}
