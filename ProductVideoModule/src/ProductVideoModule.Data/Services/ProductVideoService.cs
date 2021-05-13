using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductVideoModule.Core.Models;
using ProductVideoModule.Core.Services;
using ProductVideoModule.Data.Models;
using ProductVideoModule.Data.Repositories;

namespace ProductVideoModule.Data.Services
{
    public class ProductVideoService : IProductVideoService
    {
        private readonly Func<IVideoLinkRepository> _repositoryFactory;
        public ProductVideoService(Func<IVideoLinkRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }
        public async Task<IEnumerable<VideoLink>> GetVideoPage(int page = 0, int size= 10)
        {
            using (var videoLinkRepository = _repositoryFactory())
            {
                var result = await videoLinkRepository.VideoLinks.OrderByDescending(x => x.CreatedDate)
                    .Skip(page * size).Take(size).ToListAsync();

                // костыль
                return result as IEnumerable<VideoLink>;
            }            
        }
    }
}
