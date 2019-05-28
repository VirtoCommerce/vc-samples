using VirtoCommerce.Domain.Commerce.Model.Search;

namespace External.CustomerReviewsModule.Core.Models.Search
{
    public class CustomerReviewSearchCriteria : SearchCriteriaBase
    {
        public string[] ProductIds { get; set; }
        public bool? IsActive { get; set; }
    }
}
