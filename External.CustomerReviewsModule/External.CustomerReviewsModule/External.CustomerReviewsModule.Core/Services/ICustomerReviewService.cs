using External.CustomerReviewsModule.Core.Models;
using External.CustomerReviewsModule.Core.Models.Search;

namespace External.CustomerReviewsModule.Core.Services
{
    public interface ICustomerReviewService
    {
        CustomerReview[] GetByIds(string[] ids);

        IdentifierModel CreateCustomerReview(CustomerReviewCreateModel customerReviewCreateModel);

        void UpdateCustomerReview(string id, CustomerReviewUpdateModel customerReviewUpdateModel);

        void DeleteCustomerReviews(string id);

        CustomerReviewResponseModel GetCustomerReviewById(string id);
        void SaveCustomerReviews(CustomerReview[] customerReview);
        void DeleteCustomerReviews(string[] ids);
    }
}
