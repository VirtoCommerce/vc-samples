using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.Platform.Data;

namespace ProductVideoModule.Data.Models
{
    public class VideoLinkEntity
    {
        public string Id { get; set; }
        public string TargetUrl { get; set; }
        public string ProductId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
