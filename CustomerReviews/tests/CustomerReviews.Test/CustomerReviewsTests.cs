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
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.GenericCrud;
using Xunit;

namespace CustomerReviews.Test
{
    public class CustomerReviewsTests
    {
        private const string ProductId = "testProductId";
        private const string CustomerReviewId = "testId";

        private readonly Mock<IPlatformMemoryCache> _platformMemoryCacheMock;
        private readonly Mock<ICacheEntry> _cacheEntryMock;
        private readonly Mock<IEventPublisher> _eventPublisherMock;

        public CustomerReviewsTests()
        {
            _eventPublisherMock = new Mock<IEventPublisher>();
            _platformMemoryCacheMock = new Mock<IPlatformMemoryCache>();
            _platformMemoryCacheMock.Setup(x => x.GetDefaultCacheEntryOptions()).Returns(() => new MemoryCacheEntryOptions());
            _cacheEntryMock = new Mock<ICacheEntry>();
            _cacheEntryMock.SetupGet(c => c.ExpirationTokens).Returns(new List<IChangeToken>());
            var cacheKeyCRUD = CacheKey.With(typeof(CustomerReviewService), "GetByIdsAsync", string.Join("-", CustomerReviewId), null);
            _platformMemoryCacheMock.Setup(pmc => pmc.CreateEntry(cacheKeyCRUD)).Returns(_cacheEntryMock.Object);
        }

        [Fact]
        public async Task CanDoCRUDandSearch()
        {
            IEnumerable<CustomerReview> result;

            // Read non-existing item

            result = await CustomerReviewService(null).GetByIdsAsync(new[] { CustomerReviewId });
            Assert.NotNull(result);
            Assert.Empty(result);

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

            await CustomerReviewService(null).SaveChangesAsync(new[] { item });


            result = await CustomerReviewService(x => SetupExist(x, item)).GetByIdsAsync(new[] { CustomerReviewId });
            Assert.Single(result);
            item = result.First();
            Assert.Equal(CustomerReviewId, item.Id);

            // Update
            var updatedContent = "Updated content";
            Assert.NotEqual(updatedContent, item.Content);

            item.Content = updatedContent;
            await CustomerReviewService(null).SaveChangesAsync(new[] { item });

            result = await CustomerReviewService(x => SetupExist(x, item)).GetByIdsAsync(new[] { CustomerReviewId });
            Assert.Single(result);

            item = result.First();
            Assert.Equal(updatedContent, item.Content);

            // Search 
            var criteria = new CustomerReviewSearchCriteria { ProductIds = new[] { ProductId } };
            var cacheKeySearch = CacheKey.With(typeof(CustomerReviewSearchService), "SearchAsync", criteria.GetCacheKey());
            _platformMemoryCacheMock.Setup(pmc => pmc.CreateEntry(cacheKeySearch)).Returns(_cacheEntryMock.Object);
            var searchResult = await CustomerReviewSearchService(x => SetupExist(x, item)).SearchAsync(criteria);
            Assert.NotNull(searchResult);
            Assert.Equal(1, searchResult.TotalCount);
            Assert.Single(searchResult.Results);

            // Delete
            await CustomerReviewService(null).DeleteAsync(new[] { CustomerReviewId });

            var getByIdsResult = await CustomerReviewService(x => SetupNone(x)).GetByIdsAsync(new[] { CustomerReviewId });
            Assert.NotNull(getByIdsResult);
            Assert.Empty(getByIdsResult);
        }


        private ISearchService<CustomerReviewSearchCriteria, CustomerReviewSearchResult, CustomerReview> CustomerReviewSearchService(Action<Mock<ICustomerReviewRepository>> setup)
        {
            return new CustomerReviewSearchService(() => GetRepository(setup), _platformMemoryCacheMock.Object, (ICustomerReviewService)CustomerReviewService(setup));
        }

        private ICrudService<CustomerReview> CustomerReviewService(Action<Mock<ICustomerReviewRepository>> setup)
        {
            return new CustomerReviewService(() => GetRepository(setup), _platformMemoryCacheMock.Object, _eventPublisherMock.Object);
        }

        protected ICustomerReviewRepository GetRepository(Action<Mock<ICustomerReviewRepository>> setup)
        {
            var _mockCustomerReviewRepository = new Mock<ICustomerReviewRepository>();
            var _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCustomerReviewRepository.Setup(ss => ss.UnitOfWork).Returns(_mockUnitOfWork.Object);

            setup?.Invoke(_mockCustomerReviewRepository);

            return _mockCustomerReviewRepository.Object;
        }

        protected void SetupExist(Mock<ICustomerReviewRepository> repositoryMock, CustomerReview item)
        {
            repositoryMock.Setup(ss => ss.GetByIdsAsync(new[] { CustomerReviewId }))
                .ReturnsAsync(new[] { new CustomerReviewEntity().FromModel(item, new PrimaryKeyResolvingMap()) });

            var mockReviews = new[] { new CustomerReviewEntity().FromModel(item, new PrimaryKeyResolvingMap()) }.AsQueryable().BuildMock();

            repositoryMock.SetupGet(x => x.CustomerReviews).Returns(mockReviews.Object);

        }

        protected void SetupNone(Mock<ICustomerReviewRepository> repositoryMock)
        {
            repositoryMock.Setup(ss => ss.GetByIdsAsync(new[] { CustomerReviewId })).ReturnsAsync(new CustomerReviewEntity[0]);
        }

    }
}
