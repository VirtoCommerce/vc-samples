using VirtoCommerce.ExportModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;

namespace CustomerReviews.Export.Data.ExportImport
{
    public class TabularCustomerReview : AuditableEntity, IExportable
    {
        public string AuthorNickname { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
