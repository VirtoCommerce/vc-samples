using System.Linq;
using External.CustomerReviewsModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace External.CustomerReviewsModule.Data.Repositories
{
    public interface ICustomerReviewRepository : IRepository
    {
        IQueryable<CustomerReviewEntity> CustomerReviews { get; }

        CustomerReviewEntity[] GetByIds(string[] ids);
        void DeleteCustomerReviews(string[] ids);
    }
}
