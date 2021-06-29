using System;
using VirtoCommerce.Platform.Core.Common;

namespace CustomerReviews.Core.Model
{
    public class CustomerReview : AuditableEntity, ICloneable
    {
        public string AuthorNickname { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public string ProductId { get; set; }

        public object Clone()
        {
            return MemberwiseClone() as CustomerReview;
        }
    }
}
