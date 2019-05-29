using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using External.CustomerReviewsModule.Core.Models;
using External.CustomerReviewsModule.Core.Models.Search;

namespace External.CustomerReviewsModule.Data.Converters
{
    static class ClientModel
    {
        public static CustomerReviewResponseModel ToResponseModel(CustomerReview review)
        {
            var result = new CustomerReviewResponseModel()
            {
                Id = review.Id,
                AuthorNickname = review.AuthorNickname,
                CreatedBy = review.CreatedBy,
                CreatedDate = review.CreatedDate,
                UpdatedBy = review.ModifiedBy,
                UpdatedDate = review.ModifiedDate ?? review.CreatedDate,
                ProductId = review.ProductId,
                Content = review.Content,
                IsActive = review.IsActive
            };

            return result;
        }
        public static IdentifierModel ToIdentifierModel(CustomerReview review)
        {
            var result = new IdentifierModel()
            {
                Id = review.Id
            };

            return result;
        }

        public static CustomerReview FromCreateModel(CustomerReviewCreateModel createModel)
        {
            var result = new CustomerReview()
            {
                AuthorNickname = createModel.AuthorNickname,
                ProductId = createModel.ProductId,
                Content = createModel.Content,
                IsActive = createModel.IsActive,
                CreatedBy = createModel.AuthorNickname
            };
            return result;
        }

        public static CustomerReview FromUpdateModel(CustomerReviewUpdateModel updateModel, CustomerReview review)
        {
            review.Content = updateModel.Content;
            review.IsActive = updateModel.IsActive;
            return review;
        }
    }
}
