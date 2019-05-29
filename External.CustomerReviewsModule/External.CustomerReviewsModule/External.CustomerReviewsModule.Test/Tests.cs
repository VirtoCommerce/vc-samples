using System;
using System.Data.Entity;
using External.CustomerReviewsModule.Core.Models.Search;
using External.CustomerReviewsModule.Core.Services;
using External.CustomerReviewsModule.Data.Repositories;
using External.CustomerReviewsModule.Data.Services;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Testing.Bases;
using Xunit;

namespace External.CustomerReviewsModule.Test
{
    public class Tests : FunctionalTestBase
    {
        private const string ProductId = "testProductId";
        private const string CustomerReviewId = "testId";

        public Tests()
        {
            ConnectionString = "VirtoCommerce";
        }

        [Fact]
        public void CanDo_CRUDandSearch()
        {
            // Read non-existing item
            var getByIdsResult = CustomerReviewService.GetByIds(new[] { CustomerReviewId });
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

            CustomerReviewService.SaveCustomerReviews(new[] { item });

            getByIdsResult = CustomerReviewService.GetByIds(new[] { CustomerReviewId });
            Assert.Single(getByIdsResult);

            item = getByIdsResult[0];
            Assert.Equal(CustomerReviewId, item.Id);

            // Update
            var updatedContent = "Updated content";
            Assert.NotEqual(updatedContent, item.Content);

            item.Content = updatedContent;
            CustomerReviewService.SaveCustomerReviews(new[] { item });
            getByIdsResult = CustomerReviewService.GetByIds(new[] { CustomerReviewId });
            Assert.Single(getByIdsResult);

            item = getByIdsResult[0];
            Assert.Equal(updatedContent, item.Content);

            // Search
            Assert.Throws<ArgumentNullException>(() => CustomerReviewSearchService.SearchCustomerReviews(null));

            var criteria = new CustomerReviewSearchCriteria { ProductIds = new[] { ProductId } };
            var searchResult = CustomerReviewSearchService.SearchCustomerReviews(criteria);

            Assert.NotNull(searchResult);
            Assert.Equal(1, searchResult.TotalCount);
            Assert.Single(searchResult.Results);

            // Delete
            CanDelete_CustomerReviews();
        }

        [Fact]
        public void CanDelete_CustomerReviews()
        {
            CustomerReviewService.DeleteCustomerReviews(new[] { CustomerReviewId });

            var getByIdsResult = CustomerReviewService.GetByIds(new[] { CustomerReviewId });
            Assert.NotNull(getByIdsResult);
            Assert.Empty(getByIdsResult);
        }

        private ICustomerReviewSearchService CustomerReviewSearchService
        {
            get
            {
                return new CustomerReviewSearchService(GetRepository, CustomerReviewService);
            }
        }

        private ICustomerReviewService CustomerReviewService
        {
            get
            {
                return new CustomerReviewService(GetRepository);
            }
        }


        protected ICustomerReviewRepository GetRepository()
        {
            var repository = new CustomerReviewRepository(ConnectionString, new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(null));
            EnsureDatabaseInitialized(() => new CustomerReviewRepository(ConnectionString), () => Database.SetInitializer(new SetupDatabaseInitializer<CustomerReviewRepository, Data.Migrations.Configuration>()));
            return repository;
        }
    }
}
