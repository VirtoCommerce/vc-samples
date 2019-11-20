using CustomerReviews.Core.Model;
using VirtoCommerce.Platform.Core.Events;

namespace CustomerReviews.Core.Events
{
    public class CustomerReviewChangedEvent : DomainEvent
    {
        public CustomerReview[] CustomerReviews { get; set; }
    }
}
