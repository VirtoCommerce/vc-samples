using VirtoCommerce.ExportModule.Core.Model;

namespace CustomerReviews.Export.Data.ExportImport
{
    public class CustomerReviewExportDataQuery : ExportDataQuery
    {
        public string[] ProductIds { get; set; }
        public bool? IsActive { get; set; }
    }
}
