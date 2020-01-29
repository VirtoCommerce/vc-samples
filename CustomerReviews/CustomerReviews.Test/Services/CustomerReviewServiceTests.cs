using System;
using System.Linq;
using AutoFixture;
using CustomerReviews.Core.Model;
using CustomerReviews.Data.Model;
using CustomerReviews.Data.Repositories;
using CustomerReviews.Data.Services;
using FluentAssertions;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace CustomerReviews.Tests.Services
{
    public class CustomerReviewServiceTests
    {
        private readonly Fixture _randomizer;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ICustomerReviewRepository> _repository;
        private readonly CustomerReviewService _service;

        public CustomerReviewServiceTests()
        {
            _randomizer = new Fixture();
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<ICustomerReviewRepository>();
            _repository.SetupGet(m => m.UnitOfWork).Returns(_unitOfWork.Object);
            _service = new CustomerReviewService(() => _repository.Object);
        }

        [Fact]
        public void GetByIds_ShouldReturnItemsByIds()
        {
            //arrange
            var ids = _randomizer.CreateMany<string>(2).ToArray();
            var customerReviewEntities = _randomizer.CreateMany<CustomerReviewEntity>(2).ToArray();

            _repository.Setup(m => m.GetByIds(It.IsAny<string[]>())).Returns(customerReviewEntities);

            //act
            var result = _service.GetByIds(ids);

            //assert
            result.Should().BeEquivalentTo(new[]
            {
                new CustomerReview
                {
                    Id = customerReviewEntities[0].Id,
                    CreatedDate = customerReviewEntities[0].CreatedDate,
                    ModifiedDate = customerReviewEntities[0].ModifiedDate,
                    CreatedBy = customerReviewEntities[0].CreatedBy,
                    ModifiedBy = customerReviewEntities[0].ModifiedBy,
                    AuthorNickname = customerReviewEntities[0].AuthorNickname,
                    Content = customerReviewEntities[0].Content,
                    IsActive = customerReviewEntities[0].IsActive,
                    ProductId = customerReviewEntities[0].ProductId
                },
                new CustomerReview
                {
                    Id = customerReviewEntities[1].Id,
                    CreatedDate = customerReviewEntities[1].CreatedDate,
                    ModifiedDate = customerReviewEntities[1].ModifiedDate,
                    CreatedBy = customerReviewEntities[1].CreatedBy,
                    ModifiedBy = customerReviewEntities[1].ModifiedBy,
                    AuthorNickname = customerReviewEntities[1].AuthorNickname,
                    Content = customerReviewEntities[1].Content,
                    IsActive = customerReviewEntities[1].IsActive,
                    ProductId = customerReviewEntities[1].ProductId
                },
            });
        }

        [Fact]
        public void SaveCustomerReviews_ShouldThrowArgumentNullException_IfItemsIsNull()
        {
            //arrange
            CustomerReview[] items = null;

            //act
            Action act = () => _service.SaveCustomerReviews(items);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void SaveCustomerReviews_ShouldUpdateEntities_IfItsAlreadyExist()
        {
            //arrange
            var items = _randomizer.CreateMany<CustomerReview>(1).ToArray();

            var existingEntities = _randomizer.CreateMany<CustomerReviewEntity>(1).ToArray();
            existingEntities[0].Id = items[0].Id;
            _repository.Setup(m => m.GetByIds(It.IsAny<string[]>())).Returns(existingEntities);

            //act
            _service.SaveCustomerReviews(items);

            //assert
            _repository.Verify(m => m.Add(It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public void SaveCustomerReviews_ShouldInsertEntities_IfItsNotAlreadyExist()
        {
            //arrange
            var items = _randomizer.CreateMany<CustomerReview>(1).ToArray();

            var existingEntities = _randomizer.CreateMany<CustomerReviewEntity>(1).ToArray();
            _repository.Setup(m => m.GetByIds(It.IsAny<string[]>())).Returns(existingEntities);

            //act
            _service.SaveCustomerReviews(items);

            //assert
            _repository.Verify(m =>
                m.Add(It.Is<CustomerReviewEntity>(e =>
                    e.Id == items[0].Id
                    && e.ProductId == items[0].ProductId
                    && e.Content == items[0].Content
                    && e.AuthorNickname == items[0].AuthorNickname)),
                Times.Once);
        }

        [Fact]
        public void DeleteCustomerReviews_ShouldPerformRemoveByIds_IfIdsIsNotNull()
        {
            //arrange
            var ids = _randomizer.CreateMany<string>(1).ToArray();
            var customerReviewEntities = _randomizer.CreateMany<CustomerReviewEntity>(1).ToArray();

            _repository.Setup(m => m.GetByIds(It.IsAny<string[]>())).Returns(customerReviewEntities);

            //act
            _service.DeleteCustomerReviews(ids);

            //assert
            _repository.Verify(m =>
                m.RemoveByIds(It.Is<string[]>(r =>
                    r.Count() == 1
                    && r.Contains(customerReviewEntities[0].Id))),
                Times.Once);
        }
    }
}
