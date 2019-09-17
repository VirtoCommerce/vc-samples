using CustomerReviews.Core.Model;
using VirtoCommerce.ExportModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;

namespace CustomerReviews.Export.Data.ExportImport
{
    public class ExportableCustomerReview : CustomerReview, IExportable, IExportViewable, ITabularConvertible
    {
        public object Clone()
        {
            return MemberwiseClone();
        }

        #region IExportViewable
        public string Name { get; set; }
        public string Code { get; set; }
        public string ImageUrl { get; set; }
        public string Parent { get; set; }
        public string Type { get; set; }
        #endregion

        #region Additional properties
        public string ProductName { get; set; }
        #endregion

        public virtual ExportableCustomerReview FromModel(CustomerReview source)
        {
            Type = nameof(CustomerReview);
            Id = source.Id;
            AuthorNickname = source.AuthorNickname;
            Content = source.Content;
            IsActive = source.IsActive;
            ProductId = source.ProductId;
            CreatedBy = source.CreatedBy;
            CreatedDate = source.CreatedDate;
            ModifiedBy = source.ModifiedBy;
            ModifiedDate = source.ModifiedDate;

            return this;
        }

        #region ITabularConvertible
        public IExportable ToTabular()
        {
            var result = AbstractTypeFactory<TabularCustomerReview>.TryCreateInstance();
            result.AuthorNickname = AuthorNickname;
            result.Content = Content;
            result.IsActive = IsActive;
            result.ProductId = ProductId;
            result.ProductName = ProductName;

            result.Id = Id;
            result.CreatedBy = CreatedBy;
            result.CreatedDate = CreatedDate;
            result.ModifiedBy = ModifiedBy;
            result.ModifiedDate = ModifiedDate;

            return result;
        }
        #endregion
    }
}
