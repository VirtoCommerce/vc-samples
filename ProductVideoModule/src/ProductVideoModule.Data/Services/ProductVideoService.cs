using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProductVideoModule.Core.Events;
using ProductVideoModule.Core.Models;
using ProductVideoModule.Core.Services;
using ProductVideoModule.Data.Caching;
using ProductVideoModule.Data.Models;
using ProductVideoModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace ProductVideoModule.Data.Services
{
    public class ProductVideoService : IProductVideoService
    {
        private readonly Func<IVideoLinkRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly IEventPublisher _eventPublisher;
        public ProductVideoService(Func<IVideoLinkRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache
            , IEventPublisher eventPublisher)
        {
            _repositoryFactory = repositoryFactory;
            _platformMemoryCache = platformMemoryCache;
            _eventPublisher = eventPublisher;
        }

        public async Task<VideoLink[]> GetByIdsAsync(string[] videoLinksIds)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetByIdsAsync), string.Join("-", videoLinksIds));
            return await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.AddExpirationToken(ProductVideoCacheRegion.CreateChangeToken());
                using (var repository = _repositoryFactory())
                {
                    repository.DisableChangesTracking();

                    var links = await repository.GetByIdsAsync(videoLinksIds);

                    return links.Select(x => x.ToModel(AbstractTypeFactory<VideoLink>.TryCreateInstance())).ToArray();
                }
            });
        }

        public async Task SaveVideoLinksAsync(VideoLink[] items)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var newEntries = new List<GenericChangedEntry<VideoLink>>();

            using (var repository = _repositoryFactory())
            {

                //
                var alreadyExistEntities = await repository.GetByIdsAsync(items.Select(x => x.Id));
                foreach (var videolink in items)
                {
                    var sourceEntity = AbstractTypeFactory<VideoLinkEntity>.TryCreateInstance().FromModel(videolink, pkMap);
                    var targetEntity = alreadyExistEntities.FirstOrDefault(x => x.Id == sourceEntity.Id);
                    if (targetEntity != null)
                    {
                        sourceEntity.Patch(targetEntity);
                    }
                    else
                    {
                        repository.Add(sourceEntity);
                        newEntries.Add(new GenericChangedEntry<VideoLink>(videolink, EntryState.Added));
                    }
                }


                await repository.UnitOfWork.CommitAsync();
                pkMap.ResolvePrimaryKeys();

                ClearCache();

                //
                await _eventPublisher.Publish(new ProductVideoAddedEvent(newEntries));
            }
        }

        public async Task DeleteVideoLinksAsync(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                var videolinks = await GetByIdsAsync(ids);

                var changedEntries = videolinks.Select(x => new GenericChangedEntry<VideoLink>(x, EntryState.Deleted)).ToArray();

                await repository.DeleteVideoLinksAsync(ids);
                await repository.UnitOfWork.CommitAsync();

                ClearCache();

            }
        }

        protected virtual void ClearCache()
        {
            ProductVideoCacheRegion.ExpireRegion();
        }
    }
}
