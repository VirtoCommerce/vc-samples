using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public IQueryable<VideoLinkEntity> VideoLinks => DbContext.Set<VideoLinkEntity>();
        //{
        //    get
        //    {
        //        return DbContext.Set<VideoLinkEntity>();
        //    }
        //}

        #endregion

        
    }
}
