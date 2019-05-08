using System.ComponentModel.DataAnnotations.Schema;
using External.PriceModule.Core.Model;
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.PricingModule.Data.Model;

namespace External.PriceModule.Data.Model
{
    public class PriceExEntity : PriceEntity
    {
        [Column(TypeName = "Money")]
        public decimal? BasePrice { get; set; }

        public override Price ToModel(Price price)
        {
            var result = base.ToModel(price);

            var priceEx = (PriceEx)result;
            priceEx.BasePrice = BasePrice;

            return priceEx;
        }

        public override PriceEntity FromModel(Price price, PrimaryKeyResolvingMap pkMap)
        {
            base.FromModel(price, pkMap);

            var priceEx = (PriceEx)price;
            BasePrice = priceEx.BasePrice;

            return this;
        }

        public override void Patch(PriceEntity target)
        {
            base.Patch(target);

            var priceExEntity = (PriceExEntity)target;
            priceExEntity.BasePrice = BasePrice;
        }
    }
}