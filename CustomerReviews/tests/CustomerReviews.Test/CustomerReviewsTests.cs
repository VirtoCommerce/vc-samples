using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerReviews.Core.Model;
using CustomerReviews.Core.Model.Search;
using CustomerReviews.Core.Services;
using CustomerReviews.Data.Model;
using CustomerReviews.Data.Repositories;
using CustomerReviews.Data.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using MockQueryable.Moq;
using Moq;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using Xunit;

namespace CustomerReviews.Test
{
    public class CustomerReviewsTests
    {
        private const string ProductId = "testProductId";
        private const string CustomerReviewId = "testId";

        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICustomerReviewRepository> _mockCustomerReviewRepository;
        private readonly Mock<IPlatformMemoryCache> _platformMemoryCacheMock;
        private readonly Mock<ICacheEntry> _cacheEntryMock;

        public CustomerReviewsTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCustomerReviewRepository = new Mock<ICustomerReviewRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCustomerReviewRepository.Setup(ss => ss.UnitOfWork).Returns(_mockUnitOfWork.Object);
            _platformMemoryCacheMock = new Mock<IPlatformMemoryCache>();
            _cacheEntryMock = new Mock<ICacheEntry>();
            _cacheEntryMock.SetupGet(c => c.ExpirationTokens).Returns(new List<IChangeToken>());
            var cacheKey = CacheKey.With(CustomerReviewService.GetType(), "GetByIdsAsync", string.Join("-", CustomerReviewId));
            _platformMemoryCacheMock.Setup(pmc => pmc.CreateEntry(cacheKey)).Returns(_cacheEntryMock.Object);
        }

        [Fact]
        public async Task CanDoCRUDandSearch()
        {
            // Read non-existing item
            var getByIdsResult = await CustomerReviewService.GetByIdsAsync(new[] { CustomerReviewId });
            Assert.NotNull(getByIdsResult);
            Assert.Empty(getByIdsResult);

            // Create
            var item = new CustomerReview
            {
                Id = CustomerReviewId,
                ProductId = ProductId,
                CreatedDate = DateTime.Now,
                CreatedBy = "initial data seed",
                AuthorNickname = "John Doe",
                Content = "Liked that"
            };

            await CustomerReviewService.SaveCustomerReviewsAsync(new[] { item });

            _mockCustomerReviewRepository.Setup(ss => ss.GetByIdsAsync(new[] { CustomerReviewId }))
                .ReturnsAsync(new[] { new CustomerReviewEntity().FromModel(item, new PrimaryKeyResolvingMap()) });

            getByIdsResult = await CustomerReviewService.GetByIdsAsync(new[] { CustomerReviewId });
            Assert.Single(getByIdsResult);

            item = getByIdsResult[0];
            Assert.Equal(CustomerReviewId, item.Id);

            // Update
            var updatedContent = "Updated content";
            Assert.NotEqual(updatedContent, item.Content);

            item.Content = updatedContent;
            await CustomerReviewService.SaveCustomerReviewsAsync(new[] { item });
            getByIdsResult = await CustomerReviewService.GetByIdsAsync(new[] { CustomerReviewId });
            Assert.Single(getByIdsResult);

            item = getByIdsResult[0];
            Assert.Equal(updatedContent, item.Content);

            // Search
            var mockReviews = new[] { new CustomerReviewEntity().FromModel(item, new PrimaryKeyResolvingMap()) }.AsQueryable().BuildMock();
            _mockCustomerReviewRepository.SetupGet(x => x.CustomerReviews).Returns(mockReviews.Object);

            var criteria = new CustomerReviewSearchCriteria { ProductIds = new[] { ProductId } };
            var cacheKey = CacheKey.With(CustomerReviewSearchService.GetType(), nameof(CustomerReviewSearchService.SearchCustomerReviewsAsync), criteria.GetCacheKey());
            _platformMemoryCacheMock.Setup(pmc => pmc.CreateEntry(cacheKey)).Returns(_cacheEntryMock.Object);

            var searchResult = await CustomerReviewSearchService.SearchCustomerReviewsAsync(criteria);

            Assert.NotNull(searchResult);
            Assert.Equal(1, searchResult.TotalCount);
            Assert.Single(searchResult.Results);

            // Delete
            _mockCustomerReviewRepository.Setup(ss => ss.GetByIdsAsync(new[] { CustomerReviewId }))
                .ReturnsAsync(new CustomerReviewEntity[0]);

            await CanDeleteCustomerReviews();
        }

        [Fact]
        public async Task CanDeleteCustomerReviews()
        {
            await CustomerReviewService.DeleteCustomerReviewsAsync(new[] { CustomerReviewId });

            var getByIdsResult = await CustomerReviewService.GetByIdsAsync(new[] { CustomerReviewId });
            Assert.NotNull(getByIdsResult);
            Assert.Empty(getByIdsResult);
        }

        private ICustomerReviewSearchService CustomerReviewSearchService
        {
            get
            {
                return new CustomerReviewSearchService(() => _mockCustomerReviewRepository.Object, _platformMemoryCacheMock.Object, CustomerReviewService);
            }
        }
        private ICustomerReviewService CustomerReviewService
        {
            get
            {
                return new CustomerReviewService(() => _mockCustomerReviewRepository.Object, _platformMemoryCacheMock.Object);
            }
        }
    }
}
