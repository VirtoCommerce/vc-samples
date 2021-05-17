using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProductVideoModule.Core.Models.Search;

namespace ProductVideoModule.Core.Services
{
    public interface IProductVideoSearchService
    {
        Task<ProductVideoSearchResult> SearchVideoLinksAsync(ProductVideoSearchCriteria criteria);
    }
}
