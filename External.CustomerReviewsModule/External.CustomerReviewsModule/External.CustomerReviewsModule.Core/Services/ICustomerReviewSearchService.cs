using External.CustomerReviewsModule.Core.Models.Search;
using VirtoCommerce.Domain.Commerce.Model.Search;

namespace External.CustomerReviewsModule.Core.Services
{
    public interface ICustomerReviewSearchService
    {
        GenericSearchResult<CustomerReview> SearchCustomerReviews(CustomerReviewSearchCriteria criteria);
    }
}
