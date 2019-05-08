using External.PriceModule.Core.Model;
using External.PriceModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace External.PriceModule.Test
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
    }
}
