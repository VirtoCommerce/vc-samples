using VirtoCommerce.Domain.Pricing.Model;

namespace External.PriceModule.Core.Model
{
    public class PriceEx : Price
    {
        public decimal? BasePrice { get; set; }
    }
}