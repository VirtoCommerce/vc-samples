using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using External.CustomerReviewsModule.Core.Models.Common;
using External.CustomerReviewsModule.Core.Models.Create;
using External.CustomerReviewsModule.Core.Models.Read;
using External.CustomerReviewsModule.Core.Models.Search;
using External.CustomerReviewsModule.Core.Models.Update;
using External.CustomerReviewsModule.Core.Services;
using External.CustomerReviewsModule.Web.Security;
using VirtoCommerce.Domain.Commerce.Model.Search;

namespace External.CustomerReviewsModule.Web.Controllers.Api
{
    [RoutePrefix("api/CustomerReviews")]
    public class CustomerReviewsController : ApiController
    {
        private readonly ICustomerReviewSearchService _customerReviewSearchService;
        private readonly ICustomerReviewService _customerReviewService;

        public CustomerReviewsController()
        {
        }

        public CustomerReviewsController(ICustomerReviewSearchService customerReviewSearchService, ICustomerReviewService customerReviewService)
        {
            _customerReviewSearchService = customerReviewSearchService;
            _customerReviewService = customerReviewService;
        }

        /// <summary>
        /// Return product Customer review search results
        /// </summary>
        [HttpPost]
        [Route("search")]
        [ResponseType(typeof(GenericSearchResult<CustomerReview>))]
        //[CheckPermission(Permission = PredefinedPermissions.CustomerReviewRead)]
        public IHttpActionResult SearchCustomerReviews(CustomerReviewSearchCriteria criteria)
        {
            var result = _customerReviewSearchService.SearchCustomerReviews(criteria);
            return Ok(result);
        }

        /// <summary>
        ///  Create new customer review
        /// </summary>
        /// <param name="model">Customer review</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(IdentifierModel))]
        //[CheckPermission(Permission = PredefinedPermissions.CustomerReviewUpdate)]
        public IHttpActionResult Create(CustomerReviewCreateModel model)
        {
            var result = _customerReviewService.CreateCustomerReview(model);
            return Ok(result);
        }

        /// <summary>
        /// Update existing customer review
        /// </summary>
        /// <param name="id">Customer review id</param>
        /// <param name="model">Customer review updated data</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ResponseType(typeof(void))]
        //[CheckPermission(Permission = PredefinedPermissions.CustomerReviewUpdate)]
        public IHttpActionResult Update(string id, CustomerReviewUpdateModel model)
        {
            _customerReviewService.UpdateCustomerReview(model);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Delete Customer Review by ID
        /// </summary>
        /// <param name="id">Customer review id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ResponseType(typeof(void))]
        //[CheckPermission(Permission = PredefinedPermissions.CustomerReviewDelete)]
        public IHttpActionResult Delete(string id)
        {
            _customerReviewService.DeleteCustomerReviews(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Get Customer Review by id
        /// </summary>
        /// <param name="id">Customer Review id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ResponseType(typeof(CustomerReviewResponseModel))]
        //[CheckPermission(Permission = PredefinedPermissions.CustomerReviewRead)]
        public IHttpActionResult Get(string id)
        {
            var result = _customerReviewService.GetCustomerReviewById(id);
            return Ok(result);
        }
    }
}
