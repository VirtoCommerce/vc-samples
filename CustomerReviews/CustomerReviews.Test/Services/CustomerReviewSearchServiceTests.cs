using System;
using System.Linq;
using AutoFixture;
using CustomerReviews.Core.Model;
using CustomerReviews.Core.Services;
using CustomerReviews.Data.Model;
using CustomerReviews.Data.Repositories;
using CustomerReviews.Data.Services;
using FluentAssertions;
using Moq;
using VirtoCommerce.Domain.Commerce.Model.Search;
using Xunit;

namespace CustomerReviews.Tests.Services
{
    public class CustomerReviewSearchServiceTests
    {
        private readonly Fixture _randomizer;
        private readonly Mock<ICustomerReviewRepository> _repository;
        private readonly Mock<ICustomerReviewService> _customerReviewService;
        private readonly CustomerReviewSearchService _searchService;

        public CustomerReviewSearchServiceTests()
        {
            _randomizer = new Fixture();
            _repository = new Mock<ICustomerReviewRepository>();
            _customerReviewService = new Mock<ICustomerReviewService>();
            _searchService = new CustomerReviewSearchService(() => _repository.Object, _customerReviewService.Object);
        }

        [Fact]
        public void SearchCustomerReviews_ShouldThrowArgumentNullException_IfCriteriaIsNull()
        {
            //arrange
            CustomerReviewSearchCriteria criteria = null;

            //act
            Action act = () => _searchService.SearchCustomerReviews(criteria);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void SearchCustomerReviews_ShouldReturnProductsByIds_IfProductIdsIsSet()
        {
            //arrange
            var productIds = _randomizer.CreateMany<string>(2).ToArray();
            var criteria = new CustomerReviewSearchCriteria
            {
                ProductIds = productIds,
                Take = 2
            };

            var customerReviewEntities = _randomizer.CreateMany<CustomerReviewEntity>(2);
            customerReviewEntities.ElementAt(0).ProductId = productIds[0];
            _repository.Setup(p => p.CustomerReviews).Returns(customerReviewEntities.AsQueryable());

            var customerReviews = _randomizer.CreateMany<CustomerReview>(1);
            customerReviews.ElementAt(0).Id = customerReviewEntities.ElementAt(0).Id;
            _customerReviewService
                .Setup(p => p.GetByIds(It.IsAny<string[]>()))
                .Returns(customerReviews.ToArray());

            //act
            var result = _searchService.SearchCustomerReviews(criteria);

            //assert
            result.Should().BeEquivalentTo(new GenericSearchResult<CustomerReview>
            {
                TotalCount = 1,
                Results = new[]
                {
                    new CustomerReview
                    {
                        Id = customerReviews.ElementAt(0).Id,
                        CreatedDate = customerReviews.ElementAt(0).CreatedDate,
                        ModifiedDate = customerReviews.ElementAt(0).ModifiedDate,
                        CreatedBy = customerReviews.ElementAt(0).CreatedBy,
                        ModifiedBy = customerReviews.ElementAt(0).ModifiedBy,
                        AuthorNickname = customerReviews.ElementAt(0).AuthorNickname,
                        Content = customerReviews.ElementAt(0).Content,
                        IsActive = customerReviews.ElementAt(0).IsActive,
                        ProductId = productIds[0]
                    }
                }
            });
        }

        [Fact]
        public void SearchCustomerReviews_ShouldReturnActiveProducts_IfIsActiveIsSet()
        {
            //arrange
            var isActive = _randomizer.Create<bool>();
            var criteria = new CustomerReviewSearchCriteria
            {
                Take = 2,
                IsActive = isActive
            };

            var customerReviewEntities = _randomizer
                .Build<CustomerReviewEntity>()
                .Without(p => p.IsActive)
                .CreateMany(2);
            customerReviewEntities.ElementAt(0).IsActive = isActive;
            _repository.Setup(p => p.CustomerReviews).Returns(customerReviewEntities.AsQueryable());

            var customerReviews = _randomizer.CreateMany<CustomerReview>(1);
            customerReviews.ElementAt(0).Id = customerReviewEntities.ElementAt(0).Id;
            _customerReviewService
                .Setup(p => p.GetByIds(It.IsAny<string[]>()))
                .Returns(customerReviews.ToArray());

            //act
            var result = _searchService.SearchCustomerReviews(criteria);

            //assert
            result.Should().BeEquivalentTo(new GenericSearchResult<CustomerReview>
            {
                TotalCount = 1,
                Results = new[]
                {
                    new CustomerReview
                    {
                        Id = customerReviews.ElementAt(0).Id,
                        CreatedDate = customerReviews.ElementAt(0).CreatedDate,
                        ModifiedDate = customerReviews.ElementAt(0).ModifiedDate,
                        CreatedBy = customerReviews.ElementAt(0).CreatedBy,
                        ModifiedBy = customerReviews.ElementAt(0).ModifiedBy,
                        AuthorNickname = customerReviews.ElementAt(0).AuthorNickname,
                        Content = customerReviews.ElementAt(0).Content,
                        IsActive = isActive,
                        ProductId = customerReviews.ElementAt(0).ProductId
                    }
                }
            });
        }

        [Fact]
        public void SearchCustomerReviews_ShouldReturnProductsWithPhraseInContent_IfSearchPhraseIsSet()
        {
            //arrange
            var searchPhrase = _randomizer.Create<string>();
            var criteria = new CustomerReviewSearchCriteria
            {
                Take = 2,
                SearchPhrase = searchPhrase
            };

            var customerReviewEntities = _randomizer.CreateMany<CustomerReviewEntity>(2);
            customerReviewEntities.ElementAt(0).Content = searchPhrase;
            _repository.Setup(p => p.CustomerReviews).Returns(customerReviewEntities.AsQueryable());

            var customerReviews = _randomizer.CreateMany<CustomerReview>(1);
            customerReviews.ElementAt(0).Id = customerReviewEntities.ElementAt(0).Id;
            _customerReviewService
                .Setup(p => p.GetByIds(It.IsAny<string[]>()))
                .Returns(customerReviews.ToArray());

            //act
            var result = _searchService.SearchCustomerReviews(criteria);

            //assert
            result.Should().BeEquivalentTo(new GenericSearchResult<CustomerReview>
            {
                TotalCount = 1,
                Results = new[]
                {
                    new CustomerReview
                    {
                        Id = customerReviews.ElementAt(0).Id,
                        CreatedDate = customerReviews.ElementAt(0).CreatedDate,
                        ModifiedDate = customerReviews.ElementAt(0).ModifiedDate,
                        CreatedBy = customerReviews.ElementAt(0).CreatedBy,
                        ModifiedBy = customerReviews.ElementAt(0).ModifiedBy,
                        AuthorNickname = customerReviews.ElementAt(0).AuthorNickname,
                        Content = searchPhrase,
                        IsActive = customerReviews.ElementAt(0).IsActive,
                        ProductId = customerReviews.ElementAt(0).ProductId
                    }
                }
            });

        }
    }
}
