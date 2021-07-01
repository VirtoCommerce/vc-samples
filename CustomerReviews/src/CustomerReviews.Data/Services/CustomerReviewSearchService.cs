using System;
using System.Collections.Generic;
using System.Linq;
using CustomerReviews.Core.Model;
using CustomerReviews.Core.Model.Search;
using CustomerReviews.Core.Services;
using CustomerReviews.Data.Model;
using CustomerReviews.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace CustomerReviews.Data.Services
{
    public class CustomerReviewSearchService : SearchService<CustomerReviewSearchCriteria, CustomerReviewSearchResult, CustomerReview, CustomerReviewEntity>, ICustomerReviewSearchService
    {
        public CustomerReviewSearchService(Func<ICustomerReviewRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, ICustomerReviewService customerReviewService)
            : base(repositoryFactory, platformMemoryCache, (ICrudService<CustomerReview>)customerReviewService)
        {
        }

        protected override IQueryable<CustomerReviewEntity> BuildQuery(IRepository repository, CustomerReviewSearchCriteria criteria)
        {
            var query = ((ICustomerReviewRepository)repository).CustomerReviews;

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

        protected override IList<SortInfo> BuildSortExpression(CustomerReviewSearchCriteria criteria)
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
