using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.PricingModule.Data.Model;

namespace External.PriceModule.Web.Model
{
    public class Price2Entity : PriceEntity
    {
        public decimal? BasePrice { get; set; }

        public override Price ToModel(Price price)
        {
            var result = base.ToModel(price);

            var price2 = (Price2)result;
            price2.BasePrice = BasePrice;

            return price2;
        }

        public override PriceEntity FromModel(Price price, PrimaryKeyResolvingMap pkMap)
        {
            base.FromModel(price, pkMap);

            var price2 = (Price2)price;
            BasePrice = price2.BasePrice;

            return this;
        }

        public override void Patch(PriceEntity target)
        {
            base.Patch(target);

            var price2Entity = (Price2Entity)target;
            price2Entity.BasePrice = BasePrice;
        }
    }
}