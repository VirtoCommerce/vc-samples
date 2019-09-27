using External.PricingModule.Core.Models;
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.ExportModule.Core.Model;
using VirtoCommerce.PricingModule.Data.ExportImport;

namespace External.PricingModule.Data.ExportImport
{
    public class ExportablePriceEx : ExportablePrice
    {
        public decimal? BasePrice { get; set; }

        public override ExportablePrice FromModel(Price source)
        {
            var result = base.FromModel(source) as ExportablePriceEx;

            if (source is PriceEx priceEx)
            {
                BasePrice = priceEx.BasePrice;
            }

            return result;
        }

        public override IExportable ToTabular()
        {
            var result = base.ToTabular() as TabularPriceEx;

            result.BasePrice = BasePrice;

            return result;
        }
    }
}
