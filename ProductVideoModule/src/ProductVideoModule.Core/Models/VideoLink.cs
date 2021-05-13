using System;
using System.Collections.Generic;
using System.Text;

namespace ProductVideoModule.Core.Models
{
    public class VideoLink 
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string ProductId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
