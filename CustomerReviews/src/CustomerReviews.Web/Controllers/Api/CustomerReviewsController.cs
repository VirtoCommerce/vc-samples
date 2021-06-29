using System.Threading.Tasks;
using CustomerReviews.Core;
using CustomerReviews.Core.Model;
using CustomerReviews.Core.Model.Search;
using CustomerReviews.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace CustomerReviews.Web.Controllers.Api
{
    [Route("api/customerReviews")]
    public class CustomerReviewsController : Controller
    {
        private readonly ISearchService<CustomerReviewSearchCriteria, CustomerReviewSearchResult, CustomerReview> _customerReviewSearchService;
        private readonly ICrudService<CustomerReview> _customerReviewService;

        public CustomerReviewsController(ICustomerReviewSearchService customerReviewSearchService, ICustomerReviewService customerReviewService)
        {
            _customerReviewSearchService = (ISearchService<CustomerReviewSearchCriteria, CustomerReviewSearchResult, CustomerReview>) customerReviewSearchService;
            _customerReviewService = (ICrudService<CustomerReview>) customerReviewService;
        }

        /// <summary>
        /// Return product Customer review search results
        /// </summary>
        [HttpPost]
        [Route("search")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<CustomerReviewSearchResult>> SearchCustomerReviews([FromBody]CustomerReviewSearchCriteria criteria)
        {
            var result = await _customerReviewSearchService.SearchAsync(criteria);
            return result;
        }

        /// <summary>
        ///  Create new or update existing customer review
        /// </summary>
        /// <param name="customerReviews">Customer reviews</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.Update)]
        public async Task<ActionResult> Update([FromBody]CustomerReview[] customerReviews)
        {
            await _customerReviewService.SaveChangesAsync(customerReviews);
            return NoContent();
        }

        /// <summary>
        /// Delete Customer Reviews by IDs
        /// </summary>
        /// <param name="ids">IDs</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.Delete)]
        public async Task<ActionResult> Delete([FromQuery] string[] ids)
        {
            await _customerReviewService.DeleteAsync(ids);
            return NoContent();
        }
    }
}
