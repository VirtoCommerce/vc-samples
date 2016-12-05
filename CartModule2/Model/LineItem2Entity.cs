using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Platform.Core.Common;

namespace CartModule2.Model
{
    public class LineItem2Entity : LineItemEntity
    {
        [StringLength(64)]
        public string OuterId { get; set; }

        public override LineItem ToModel(LineItem lineItem)
        {
            var result = base.ToModel(lineItem);

            var lineItem2 = result as LineItem2;
            lineItem2.OuterId = this.OuterId;

            return lineItem2;
        }

        public override LineItemEntity FromModel(LineItem lineItem, PrimaryKeyResolvingMap pkMap)
        {
            base.FromModel(lineItem, pkMap);

            var lineItem2 = lineItem as LineItem2;
            this.OuterId = lineItem2.OuterId;

            return this;
        }

        public override void Patch(LineItemEntity target)
        {
            base.Patch(target);

            var lineItem2entity = target as LineItem2Entity;
            lineItem2entity.OuterId = this.OuterId;
        }
    }
}