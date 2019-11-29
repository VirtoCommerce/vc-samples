using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerReviews.Core.Model.Search;
using CustomerReviews.Core.Services;
using CustomerReviews.Data.Caching;
using CustomerReviews.Data.Model;
using CustomerReviews.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;

namespace CustomerReviews.Data.Services
{
    public class CustomerReviewSearchService : ICustomerReviewSearchService
    {
        private readonly Func<ICustomerReviewRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly ICustomerReviewService _customerReviewService;

        public CustomerReviewSearchService(Func<ICustomerReviewRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, ICustomerReviewService customerReviewService)
        {
            _repositoryFactory = repositoryFactory;
            _platformMemoryCache = platformMemoryCache;
            _customerReviewService = customerReviewService;
        }

        public async Task<CustomerReviewSearchResult> SearchCustomerReviewsAsync(CustomerReviewSearchCriteria criteria)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(SearchCustomerReviewsAsync), criteria.GetCacheKey());
            return await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.AddExpirationToken(CustomerReviewCacheRegion.CreateChangeToken());
                var result = AbstractTypeFactory<CustomerReviewSearchResult>.TryCreateInstance();
                using (var repository = _repositoryFactory())
                {
                    var query = BuildQuery(repository, criteria);
                    var sortInfos = BuildSortExpression(criteria);

                    result.TotalCount = await query.CountAsync();
                    if (criteria.Take > 0)
                    {
                        var customerReviewIds = await query.OrderBySortInfos(sortInfos).ThenBy(x => x.Id)
                                                  .Select(x => x.Id)
                                                  .Skip(criteria.Skip).Take(criteria.Take)
                                                  .ToArrayAsync();

                        var unorderedResults = await _customerReviewService.GetByIdsAsync(customerReviewIds);
                        result.Results = unorderedResults.OrderBy(x => Array.IndexOf(customerReviewIds, x.Id)).ToArray();
                    }
                }
                return result;
            });
        }

        protected virtual IQueryable<CustomerReviewEntity> BuildQuery(ICustomerReviewRepository repository, CustomerReviewSearchCriteria criteria)
        {
            var query = repository.CustomerReviews;

            if (!criteria.ProductIds.IsNullOrEmpty())
            {
                query = query.Where(x => criteria.ProductIds.Contains(x.ProductId));
            }

            if (criteria.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == criteria.IsActive);
            }

            if (!criteria.SearchPhrase.IsNullOrEmpty())
            {
                query = query.Where(x => x.Content.Contains(criteria.SearchPhrase));
            }

            return query;
        }

        protected virtual IList<SortInfo> BuildSortExpression(CustomerReviewSearchCriteria criteria)
        {
            var sortInfos = criteria.SortInfos;
            if (sortInfos.IsNullOrEmpty())
            {
                sortInfos = new[]
                {
                    new SortInfo
                    {
                        SortColumn = nameof(CustomerReviewEntity.CreatedDate), SortDirection = SortDirection.Descending
                    }
                };
            }
            return sortInfos;
        }
    }
}
