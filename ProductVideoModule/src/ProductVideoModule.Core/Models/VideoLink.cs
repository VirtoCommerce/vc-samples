using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.Platform.Core.Common;

namespace ProductVideoModule.Core.Models
{
    public class VideoLink : AuditableEntity
    {
        public string Url { get; set; }
        public string ProductId { get; set; }
    }
}
