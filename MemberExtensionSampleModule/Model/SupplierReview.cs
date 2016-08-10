using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.Common;

namespace MemberExtensionSampleModule.Web.Model
{
    public class SupplierReview : AuditableEntity
    {      
        public string Review { get; set; }

    }
}