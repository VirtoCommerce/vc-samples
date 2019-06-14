using External.PricingModule.Core.Models;
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.PricingModule.Data.Model;

namespace External.PricingModule.Data.Models
{

    public class PricelistExEntity : PricelistEntity
    {
        public string NewDescription { get; set; }

        public decimal? SecretCode { get; set; }

        public override Pricelist ToModel(Pricelist pricelist)
        {
            var result = base.ToModel(pricelist);

            var pricelistEx = (PricelistEx)result;
            pricelistEx.NewDescription = NewDescription;
            pricelistEx.SecretCode = SecretCode;

            return pricelistEx;
        }

        public override PricelistEntity FromModel(Pricelist pricelist, PrimaryKeyResolvingMap pkMap)
        {
            base.FromModel(pricelist, pkMap);

            var pricelistEx = (PricelistEx)pricelist;
            NewDescription = pricelistEx.NewDescription;
            SecretCode = pricelistEx.SecretCode;

            return this;
        }

        public override void Patch(PricelistEntity target)
        {
            base.Patch(target);

            var pricelistExEntity = (PricelistExEntity)target;
            pricelistExEntity.NewDescription = NewDescription;
            pricelistExEntity.SecretCode = SecretCode;
        }
    }
}
