using VirtoCommerce.Platform.Core.Common;

namespace CustomerReviews.Core.Model.Search
{
    public class CustomerReviewSearchCriteria : SearchCriteriaBase
    {
        public string[] ProductIds { get; set; }
        public bool? IsActive { get; set; }
    }
}
