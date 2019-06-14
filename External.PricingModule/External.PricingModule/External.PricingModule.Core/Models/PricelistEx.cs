using VirtoCommerce.Domain.Pricing.Model;

namespace External.PricingModule.Core.Models
{
    public class PricelistEx : Pricelist
    {
        public string NewDescription { get; set; }
        public decimal? SecretCode { get; set; }
    }
}
