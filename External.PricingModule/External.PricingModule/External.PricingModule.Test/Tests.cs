using External.PricingModule.Core.Models;
using External.PricingModule.Data.Models;
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.PricingModule.Data.Model;
using Xunit;

namespace External.PricingModule.Test
{
    public class Tests
    {
        public Tests()
        {
        }

        [Fact]
        public void Convert_PriceEx_DTOToEntity_EntityToDTO()
        {
            const decimal outerPrice = 1.0M;

            var model = new PriceEx()
            {
                BasePrice = outerPrice
            };

            var pkMap = new PrimaryKeyResolvingMap();
            var entityForPatch = (PriceExEntity)AbstractTypeFactory<PriceExEntity>.TryCreateInstance().FromModel(model, pkMap);

            var entity = AbstractTypeFactory<PriceExEntity>.TryCreateInstance();
            entityForPatch.Patch(entity);

            var modelResult = (PriceEx)entity.ToModel(AbstractTypeFactory<PriceEx>.TryCreateInstance());

            Assert.Equal(outerPrice, entityForPatch.BasePrice);
            Assert.Equal(outerPrice, entity.BasePrice);
            Assert.Equal(outerPrice, modelResult.BasePrice);
        }

        [Fact]
        public void Convert_PricelistEx_DTOToEntity_EntityToDTO()
        {
            const decimal secretCode = 1.0M;
            const string descr = "Test";

            var model = new PricelistEx()
            {
                NewDescription = descr,
                SecretCode = secretCode,
            };

            AbstractTypeFactory<Pricelist>.OverrideType<Pricelist, PricelistEx>();
            AbstractTypeFactory<PricelistEntity>.OverrideType<PricelistEntity, PricelistExEntity>();

            var pkMap = new PrimaryKeyResolvingMap();
            var entityForPatch = (PricelistExEntity)AbstractTypeFactory<PricelistEntity>.TryCreateInstance().FromModel(model, pkMap);

            var entity = (PricelistExEntity)AbstractTypeFactory<PricelistEntity>.TryCreateInstance();
            entityForPatch.Patch(entity);

            var modelResult = (PricelistEx)entity.ToModel(AbstractTypeFactory<Pricelist>.TryCreateInstance());

            Assert.Equal(secretCode, entityForPatch.SecretCode);
            Assert.Equal(secretCode, entity.SecretCode);
            Assert.Equal(secretCode, modelResult.SecretCode);
            Assert.Equal(descr, entityForPatch.NewDescription);
            Assert.Equal(descr, entity.NewDescription);
            Assert.Equal(descr, modelResult.NewDescription);
        }
    }
}
