using System;
using System.Linq;
using External.CustomerReviewsModule.Core.Models;
using External.CustomerReviewsModule.Core.Models.Search;
using External.CustomerReviewsModule.Core.Services;
using External.CustomerReviewsModule.Data.Converters;
using External.CustomerReviewsModule.Data.Model;
using External.CustomerReviewsModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace External.CustomerReviewsModule.Data.Services
{
    public class CustomerReviewService : ServiceBase, ICustomerReviewService
    {
        private readonly Func<ICustomerReviewRepository> _repositoryFactory;

        public CustomerReviewService(Func<ICustomerReviewRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public CustomerReview[] GetByIds(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                return repository.GetByIds(ids).Select(x => x.ToModel(AbstractTypeFactory<CustomerReview>.TryCreateInstance())).ToArray();
            }
        }

        public void SaveCustomerReviews(CustomerReview[] items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            {
                using (var changeTracker = GetChangeTracker(repository))
                {
                    var alreadyExistEntities = repository.GetByIds(items.Where(m => !m.IsTransient()).Select(x => x.Id).ToArray());
                    foreach (var derivativeContract in items)
                    {
                        var sourceEntity = AbstractTypeFactory<CustomerReviewEntity>.TryCreateInstance().FromModel(derivativeContract, pkMap);
                        var targetEntity = alreadyExistEntities.FirstOrDefault(x => x.Id == sourceEntity.Id);
                        if (targetEntity != null)
                        {
                            changeTracker.Attach(targetEntity);
                            sourceEntity.Patch(targetEntity);
                        }
                        else
                        {
                            repository.Add(sourceEntity);
                        }
                    }

                    CommitChanges(repository);
                    pkMap.ResolvePrimaryKeys();
                }
            }
        }

        public void DeleteCustomerReviews(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                repository.DeleteCustomerReviews(ids);
                CommitChanges(repository);
            }
        }

        public IdentifierModel CreateCustomerReview(CustomerReviewCreateModel customerReviewCreateModel)
        {
            var review = ClientModel.FromCreateModel(customerReviewCreateModel);
            SaveCustomerReviews(new[] { review });

            var result = ClientModel.ToIdentifierModel(review);
            return result;
        }

        public void UpdateCustomerReview(string id, CustomerReviewUpdateModel customerReviewUpdateModel)
        {
            var review = GetByIds(new[] { id }).FirstOrDefault();
            review = ClientModel.FromUpdateModel(customerReviewUpdateModel, review);
            SaveCustomerReviews(new[] { review });
        }

        public void DeleteCustomerReviews(string id)
        {
            DeleteCustomerReviews(new[] { id });
        }

        public CustomerReviewResponseModel GetCustomerReviewById(string id)
        {
            var reviews = GetByIds(new[] { id });

            if (!reviews.Any())
                return null;

            var result = ClientModel.ToResponseModel(reviews.FirstOrDefault());
            return result;
        }
    }
}
