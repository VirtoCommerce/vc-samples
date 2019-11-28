using System.Threading.Tasks;
using CustomerReviews.Core.Model;

namespace CustomerReviews.Core.Services
{
    public interface ICustomerReviewService
    {
        Task<CustomerReview[]> GetByIdsAsync(string[] ids);

        Task SaveCustomerReviewsAsync(CustomerReview[] items);

        Task DeleteCustomerReviewsAsync(string[] ids);
    }
}
