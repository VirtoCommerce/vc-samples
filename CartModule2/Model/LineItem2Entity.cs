using System.ComponentModel.DataAnnotations;
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

            var lineItem2 = (LineItem2)result;
            lineItem2.OuterId = OuterId;

            return lineItem2;
        }

        public override LineItemEntity FromModel(LineItem lineItem, PrimaryKeyResolvingMap pkMap)
        {
            base.FromModel(lineItem, pkMap);

            var lineItem2 = (LineItem2)lineItem;
            OuterId = lineItem2.OuterId;

            return this;
        }

        public override void Patch(LineItemEntity target)
        {
            base.Patch(target);

            var lineItem2Entity = (LineItem2Entity)target;
            lineItem2Entity.OuterId = OuterId;
        }
    }
}
