using CustomerReviews.Core.Model;
using VirtoCommerce.Domain.Commerce.Model.Search;

namespace CustomerReviews.Core.Services
{
    public interface ICustomerReviewSearchService
    {
        GenericSearchResult<CustomerReview> SearchCustomerReviews(CustomerReviewSearchCriteria criteria);
    }
}
