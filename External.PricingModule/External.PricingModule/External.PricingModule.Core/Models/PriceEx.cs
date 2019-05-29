using VirtoCommerce.Domain.Pricing.Model;

namespace External.PricingModule.Core.Models
{
    public class PriceEx : Price
    {
        public decimal? BasePrice { get; set; }
    }
}