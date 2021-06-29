using System;
using System.ComponentModel.DataAnnotations;
using CustomerReviews.Core.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace CustomerReviews.Data.Model
{
    public class CustomerReviewEntity : AuditableEntity, IDataEntity<CustomerReviewEntity, CustomerReview>
    {
        [StringLength(128)]
        public string AuthorNickname { get; set; }

        [Required]
        [StringLength(1024)]
        public string Content { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [StringLength(128)]
        public string ProductId { get; set; }

        public virtual CustomerReview ToModel(CustomerReview model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.Id = Id;
            model.CreatedBy = CreatedBy;
            model.CreatedDate = CreatedDate;
            model.ModifiedBy = ModifiedBy;
            model.ModifiedDate = ModifiedDate;

            model.AuthorNickname = AuthorNickname;
            model.Content = Content;
            model.IsActive = IsActive;
            model.ProductId = ProductId;

            return model;
        }

        public virtual CustomerReviewEntity FromModel(CustomerReview model, PrimaryKeyResolvingMap pkMap)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            pkMap.AddPair(model, this);

            Id = model.Id;
            CreatedBy = model.CreatedBy;
            CreatedDate = model.CreatedDate;
            ModifiedBy = model.ModifiedBy;
            ModifiedDate = model.ModifiedDate;

            AuthorNickname = model.AuthorNickname;
            Content = model.Content;
            IsActive = model.IsActive;
            ProductId = model.ProductId;

            return this;
        }

        public virtual void Patch(CustomerReviewEntity target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            target.AuthorNickname = AuthorNickname;
            target.Content = Content;
            target.IsActive = IsActive;
            target.ProductId = ProductId;
        }
    }
}
