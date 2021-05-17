using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.Platform.Core.Common;

namespace ProductVideoModule.Core.Models.Search
{
    public class ProductVideoSearchCriteria : SearchCriteriaBase
    {
        public ProductVideoSearchCriteria()
        {
            ObjectType = nameof(VideoLink);
        }

        public string[] ProductIds { get; set; }
    }
}
