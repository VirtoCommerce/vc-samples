using System.Collections.Generic;
using System.Linq;
using CustomerReviews.Core.Model;
using CustomerReviews.Core.Services;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.ExportModule.Core.Model;
using VirtoCommerce.ExportModule.Data.Services;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Common;

namespace CustomerReviews.Export.Data.ExportImport
{
    public class CustomerReviewExportPagedDataSource : ExportPagedDataSource<CustomerReviewExportDataQuery, CustomerReviewSearchCriteria>
    {
        private readonly ICustomerReviewSearchService _searchService;
        private readonly ICustomerReviewService _customerReviewService;
        private readonly IItemService _itemService;
        private readonly CustomerReviewExportDataQuery _dataQuery;
        private readonly IBlobUrlResolver _blobUrlResolver;

        public CustomerReviewExportPagedDataSource(
            ICustomerReviewSearchService searchService,
            ICustomerReviewService customerReviewService,
            IItemService itemService,
            IBlobUrlResolver blobUrlResolver,
            CustomerReviewExportDataQuery dataQuery) : base(dataQuery)
        {
            _searchService = searchService;
            _customerReviewService = customerReviewService;
            _itemService = itemService;
            _blobUrlResolver = blobUrlResolver;
            _dataQuery = dataQuery;
        }


        protected override CustomerReviewSearchCriteria BuildSearchCriteria(CustomerReviewExportDataQuery exportDataQuery)
        {
            var result = base.BuildSearchCriteria(exportDataQuery);
            // null or false value should result in "no restrictions"
            result.IsActive = (_dataQuery.IsActive ?? false) ? _dataQuery.IsActive : null;
            result.ProductIds = _dataQuery.ProductIds;

            return result;
        }

        protected override ExportableSearchResult FetchData(CustomerReviewSearchCriteria searchCriteria)
        {
            CustomerReview[] result;
            int totalCount;

            if (searchCriteria.ObjectIds.Any(x => !string.IsNullOrWhiteSpace(x)))
            {
                result = _customerReviewService.GetByIds(Enumerable.ToArray(searchCriteria.ObjectIds));
                totalCount = result.Length;
            }
            else
            {
                var priceSearchResult = _searchService.SearchCustomerReviews(searchCriteria);
                result = priceSearchResult.Results.ToArray();
                totalCount = priceSearchResult.TotalCount;
            }

            return new ExportableSearchResult
            {
                Results = ToExportable(result).ToList(),
                TotalCount = totalCount,
            };
        }

        protected virtual IEnumerable<IExportable> ToExportable(IEnumerable<AuditableEntity> objects)
        {
            var models = objects.Cast<CustomerReview>();
            var viewableMap = models.ToDictionary(x => x, x => AbstractTypeFactory<ExportableCustomerReview>.TryCreateInstance().FromModel(x));

            FillAdditionalProperties(viewableMap);

            var modelIds = models.Select(x => x.Id).ToList();

            return viewableMap.Values.OrderBy(x => modelIds.IndexOf(x.Id));
        }

        protected virtual void FillAdditionalProperties(Dictionary<CustomerReview, ExportableCustomerReview> viewableMap)
        {
            var models = viewableMap.Keys;
            var productIds = models.Select(x => x.ProductId).Distinct().ToArray();
            var products = _itemService.GetByIds(productIds, ItemResponseGroup.ItemInfo);

            foreach (var kvp in viewableMap)
            {
                var model = kvp.Key;
                var viewableEntity = kvp.Value;
                var product = products.FirstOrDefault(x => x.Id == model.ProductId);
                var imageUrl = product?.Images?.FirstOrDefault()?.Url;

                viewableEntity.Code = product?.Code;
                viewableEntity.ImageUrl = imageUrl != null ? _blobUrlResolver.GetAbsoluteUrl(imageUrl) : null;
                viewableEntity.Name = $"{model.AuthorNickname}: {model.Content}";
                viewableEntity.ProductName = product?.Name;
            }
        }
    }
}
