using External.CustomerReviewsModule.Core.Models.Common;
using External.CustomerReviewsModule.Core.Models.Create;
using External.CustomerReviewsModule.Core.Models.Read;
using External.CustomerReviewsModule.Core.Models.Search;
using External.CustomerReviewsModule.Core.Models.Update;

namespace External.CustomerReviewsModule.Core.Services
{
    public interface ICustomerReviewService
    {
        CustomerReview[] GetByIds(string[] ids);

        IdentifierModel CreateCustomerReview(CustomerReviewCreateModel customerReviewCreateModel);

        void UpdateCustomerReview(CustomerReviewUpdateModel customerReviewUpdateModel);

        void DeleteCustomerReviews(string id);

        CustomerReviewResponseModel GetCustomerReviewById(string id);
        void SaveCustomerReviews(CustomerReview[] customerReview);
        void DeleteCustomerReviews(string[] ids);
    }
}
