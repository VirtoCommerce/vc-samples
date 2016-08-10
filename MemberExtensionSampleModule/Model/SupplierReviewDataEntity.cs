using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;

namespace MemberExtensionSampleModule.Web.Model
{
    public class SupplierReviewDataEntity : AuditableEntity
    {
        public string Review { get; set; }

        public string SupplierId { get; set; }
        public virtual SupplierDataEntity Supplier { get; set; }


        public virtual SupplierReview ToModel(SupplierReview review)
        {
            review.InjectFrom(this);
            return review;

        }

        public virtual SupplierReviewDataEntity FromModel(SupplierReview review)
        {
            this.InjectFrom(review);
            return this;
        }

        public virtual void Patch(SupplierReviewDataEntity target)
        {
            target.Review = this.Review;
        }
    }
}