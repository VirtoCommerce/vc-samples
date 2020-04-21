using System;
using System.Linq;
using System.Threading.Tasks;
using CustomerReviews.Core.Model;
using CustomerReviews.Core.Services;
using CustomerReviews.Data.Caching;
using CustomerReviews.Data.Model;
using CustomerReviews.Data.Repositories;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace CustomerReviews.Data.Services
{
    public class CustomerReviewService : ICustomerReviewService
    {
        private readonly Func<ICustomerReviewRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _platformMemoryCache;

        public CustomerReviewService(Func<ICustomerReviewRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache)
        {
            _repositoryFactory = repositoryFactory;
            _platformMemoryCache = platformMemoryCache;
        }

        public virtual async Task<CustomerReview[]> GetByIdsAsync(string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), "GetByIdsAsync", string.Join("-", ids));
            return await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.AddExpirationToken(CustomerReviewCacheRegion.CreateChangeToken());

                using (var repository = _repositoryFactory())
                {
                    repository.DisableChangesTracking();

                    return (await repository.GetByIdsAsync(ids)).Select(x => x.ToModel(AbstractTypeFactory<CustomerReview>.TryCreateInstance())).ToArray();
                }
            });
        }

        public virtual async Task SaveCustomerReviewsAsync(CustomerReview[] items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (var repository = _repositoryFactory())
            {
                var pkMap = new PrimaryKeyResolvingMap();
                var alreadyExistEntities = await repository.GetByIdsAsync(items.Where(m => !m.IsTransient()).Select(x => x.Id).ToArray());
                foreach (var derivativeContract in items)
                {
                    var sourceEntity = AbstractTypeFactory<CustomerReviewEntity>.TryCreateInstance().FromModel(derivativeContract, pkMap);
                    var targetEntity = alreadyExistEntities.FirstOrDefault(x => x.Id == sourceEntity.Id);
                    if (targetEntity != null)
                    {
                        sourceEntity.Patch(targetEntity);
                    }
                    else
                    {
                        repository.Add(sourceEntity);
                    }
                }

                await repository.UnitOfWork.CommitAsync();
                pkMap.ResolvePrimaryKeys();
            }

            CustomerReviewCacheRegion.ExpireRegion();
        }

        public virtual async Task DeleteCustomerReviewsAsync(string[] ids)
        {

            using (var repository = _repositoryFactory())
            {
                var items = await repository.GetByIdsAsync(ids);

                foreach (var item in items)
                {
                    repository.Remove(item);
                }

                await repository.UnitOfWork.CommitAsync();

                CustomerReviewCacheRegion.ExpireRegion();
            }
        }
    }
}
