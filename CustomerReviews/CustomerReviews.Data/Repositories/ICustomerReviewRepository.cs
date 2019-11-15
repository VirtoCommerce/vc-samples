using System.Linq;
using CustomerReviews.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace CustomerReviews.Data.Repositories
{
    public interface ICustomerReviewRepository : IRepository
    {
        IQueryable<CustomerReviewEntity> CustomerReviews { get; }

        CustomerReviewEntity[] GetByIds(string[] ids);
        void RemoveByIds(string[] ids);
    }
}
