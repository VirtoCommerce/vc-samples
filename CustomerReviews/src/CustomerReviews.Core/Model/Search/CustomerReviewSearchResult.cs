using VirtoCommerce.Platform.Core.Common;

namespace CustomerReviews.Core.Model.Search
{
    public class CustomerReviewSearchResult : GenericSearchResult<CustomerReview>
    {
        public string[] ProductIds { get; set; }
        public bool? IsActive { get; set; }
    }
}
