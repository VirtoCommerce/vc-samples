using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CustomerReviews.Core.Events;
using CustomerReviews.Core.Services;
using VirtoCommerce.Platform.Core.Events;

namespace CustomerReviews.Events.Data.Handlers
{
    public class OpenUrlCustomerReviewChangedEvent : IEventHandler<CustomerReviewChangedEvent>
    {
        private readonly ICustomerReviewService _customerReviewService;
        private static readonly HttpClient client = new HttpClient();

        public OpenUrlCustomerReviewChangedEvent(ICustomerReviewService customerReviewService)
        {
            _customerReviewService = customerReviewService;
        }

        public async Task Handle(CustomerReviewChangedEvent message)
        {
            /// TODO ideas:
            /// * send Notification
            /// * start a long-running background job
            /// * call custom / external service
            /// * trigger Logic App
            ///

            foreach (var item in message.CustomerReviews)
            {
                // sending the data to dummy URL

                var values = new Dictionary<string, string>
                {
                    { "thing1", "hello" },
                    { "thing2", "world" },
                    { "CustomerReviewId", item.Id }
                };
                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://dummy.site.com/api/customer-reviews", content);

                // var responseString = await response.Content.ReadAsStringAsync();
            }
        }
    }
}
