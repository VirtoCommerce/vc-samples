using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerReviews.Core.Events;
using CustomerReviews.Core.Model;
using CustomerReviews.Core.Services;
using CustomerReviews.Data.Model;
using CustomerReviews.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace CustomerReviews.Data.Services
{
    public class CustomerReviewService : CrudService<CustomerReview, CustomerReviewEntity, CustomerReviewChangeEvent, CustomerReviewChangedEvent>, ICustomerReviewService
    {
        public CustomerReviewService(Func<ICustomerReviewRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, IEventPublisher eventPublisher)
            : base(repositoryFactory, platformMemoryCache, eventPublisher)
        {
        }

        protected override Task<IEnumerable<CustomerReviewEntity>> LoadEntities(IRepository repository, IEnumerable<string> ids, string responseGroup)
        {
            return ((ICustomerReviewRepository)repository).GetByIdsAsync(ids);
        }
    }
}
