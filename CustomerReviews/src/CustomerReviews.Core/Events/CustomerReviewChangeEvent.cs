using System.Collections.Generic;
using CustomerReviews.Core.Model;
using VirtoCommerce.Platform.Core.Events;

namespace CustomerReviews.Core.Events
{
    public class CustomerReviewChangeEvent : GenericChangedEntryEvent<CustomerReview>
    {
        public CustomerReviewChangeEvent(IEnumerable<GenericChangedEntry<CustomerReview>> changedEntries) : base(changedEntries)
        {
        }
    }
}
