using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;
using ProductVideoModule.Core.Models.Search;
using ProductVideoModule.Core.Services;
using ProductVideoModule.Data.Caching;
using ProductVideoModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;

namespace ProductVideoModule.Data.Services
{
    public class ProductVideoSearchService : IProductVideoSearchService
    {
        private readonly Func<IVideoLinkRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly IProductVideoService _productVideoService;
        public ProductVideoSearchService(Func<IVideoLinkRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache
            , IProductVideoService productVideoService)
        {
            _repositoryFactory = repositoryFactory;
            _platformMemoryCache = platformMemoryCache;
            _productVideoService = productVideoService;
        }

        public async Task<ProductVideoSearchResult> SearchVideoLinksAsync(ProductVideoSearchCriteria criteria)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(SearchVideoLinksAsync), criteria.GetCacheKey());
            return await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.AddExpirationToken(ProductVideoCacheRegion.CreateChangeToken());
                var result = AbstractTypeFactory<ProductVideoSearchResult>.TryCreateInstance();

                using (var repository = _repositoryFactory())
                {
                    var query = repository.VideoLinks.Where(x => criteria.ProductIds.Contains(x.ProductId));

                    result.TotalCount = await query.CountAsync();

                    if (criteria.Take > 0)
                    {
                        var videoLinksIds = await query.OrderByDescending(x => x.CreatedDate).Skip(criteria.Skip)
                            .Take(criteria.Take)
                            .Select(x => x.Id)
                            .ToArrayAsync();

                        var priorResults = await _productVideoService.GetByIdsAsync(videoLinksIds);
                        //TODO warning EF1001: Microsoft.EntityFrameworkCore.Internal.EnumerableExtensions is an internal API that supports the Entity Framework Core infrastructure and not subject to the same compatibility standards as public APIs.
                        result.Results = priorResults;
                    }

                    return result;
                }
            });
        }

    }
}
