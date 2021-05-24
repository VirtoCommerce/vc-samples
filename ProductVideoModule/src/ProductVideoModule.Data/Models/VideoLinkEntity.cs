using System;
using System.Collections.Generic;
using System.Text;
using ProductVideoModule.Core;
using ProductVideoModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data;

namespace ProductVideoModule.Data.Models
{
    public class VideoLinkEntity : AuditableEntity
    {
        public string Url { get; set; }
        public string ProductId { get; set; }
        public VideoLinkStatus Status { get; set; }

        public virtual VideoLink ToModel(VideoLink videolink)
        {
            if (videolink == null)
                throw new ArgumentNullException(nameof(videolink));

            videolink.Id = Id;
            videolink.Url = Url;
            videolink.ProductId = ProductId;
            videolink.CreatedDate = CreatedDate;
            videolink.ModifiedDate = ModifiedDate;
            videolink.Status = Status;

            return videolink;
        }

        public virtual VideoLinkEntity FromModel(VideoLink videolink, PrimaryKeyResolvingMap pkMap)
        {
            if (videolink == null)
                throw new ArgumentNullException(nameof(videolink));

            pkMap.AddPair(videolink, this);

            Id = videolink.Id;
            Url = videolink.Url;
            ProductId = videolink.ProductId;
            CreatedDate = videolink.CreatedDate;
            ModifiedDate = videolink.ModifiedDate;
            Status = videolink.Status;

            return this;
        }

        public virtual void Patch(VideoLinkEntity target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            target.Url = Url;
            target.ProductId = ProductId;
            target.Status = Status;
        }
    }
}
