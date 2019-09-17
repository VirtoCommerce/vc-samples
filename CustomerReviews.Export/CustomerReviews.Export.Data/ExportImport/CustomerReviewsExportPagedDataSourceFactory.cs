using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerReviews.Core.Services;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.ExportModule.Core.Model;
using VirtoCommerce.Platform.Core.Assets;

namespace CustomerReviews.Export.Data.ExportImport
{
    public class CustomerReviewsExportPagedDataSourceFactory : ICustomerReviewsExportPagedDataSourceFactory
    {
        private readonly ICustomerReviewSearchService _searchService;
        private readonly ICustomerReviewService _customerReviewService;
        private readonly IItemService _itemService;
        private readonly IBlobUrlResolver _blobUrlResolver;

        public CustomerReviewsExportPagedDataSourceFactory(ICustomerReviewSearchService searchService, ICustomerReviewService customerReviewService, IItemService itemService, IBlobUrlResolver blobUrlResolver)
        {
            _searchService = searchService;
            _customerReviewService = customerReviewService;
            _itemService = itemService;
            _blobUrlResolver = blobUrlResolver;
        }

        public virtual IPagedDataSource Create(ExportDataQuery dataQuery)
        {
            IPagedDataSource result = null;
            if (dataQuery is CustomerReviewExportDataQuery customerReviewExportDataQuery)
            {
                result = new CustomerReviewExportPagedDataSource(_searchService, _customerReviewService, _itemService, _blobUrlResolver, customerReviewExportDataQuery);
            }

            return result ?? throw new ArgumentException($"Unsupported export query type: {dataQuery.GetType().Name}");
        }

    }
}
