using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductVideoModule.Core.Models;
using ProductVideoModule.Core.Models.Search;
using ProductVideoModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;

namespace ProductVideoModule.Web.Controllers.Api
{
    [Route("api/productVideo")]
    public class ProductVideoController : Controller
    {
        private readonly IProductVideoService _productVideoService;
        private readonly IProductVideoSearchService _productVideoSearchService;

        public ProductVideoController(IProductVideoService productVideoService, IProductVideoSearchService productVideoSearchService)
        {
            _productVideoService = productVideoService;
            _productVideoSearchService = productVideoSearchService;
        }

        /// <summary>
        /// Return product Video search results
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("search")]
        [ProducesResponseType(typeof(GenericSearchResult<VideoLink>), statusCode: StatusCodes.Status200OK)]
        //[CheckPermission(Permission = Core.ModuleConstants.Security.Permissions.Read)]
        public async Task<IActionResult> SearchProductVideos(ProductVideoSearchCriteria criteria)
        {
            string errorMessage;
            if (!ValidateSearchCriteria(criteria, out errorMessage))
            {
                return BadRequest(errorMessage);
            }

            var result = await _productVideoSearchService.SearchVideoLinksAsync(criteria);
            return Ok(result);            
        }

        private bool ValidateSearchCriteria(ProductVideoSearchCriteria criteria, out string errorMessage)
        {
            errorMessage = null;
            bool result = false;/*default(bool)*/

            if (criteria.ProductIds == null || criteria.ProductIds.Length == 0)
            {
                errorMessage = "Search request must contain a product Id.";
            }
            else if (criteria.Skip < 0 || criteria.Take < 0 || criteria.Take > 100)
            {
                errorMessage = "Search request parameters 'Skip' and 'Take' must be positive digits. Take parameter must be less then a hundred.";
            }
            else
            {
                return !result;
            }
            return result;
        }

        /// <summary>
        /// Get video by Id
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("{id}")]
        [ProducesResponseType(typeof(VideoLink), statusCode: StatusCodes.Status200OK)]
        //[CheckPermission(Permission = Core.ModuleConstants.Security.Permissions.Read)]
        public async Task<IActionResult> GetById([FromRoute] string id) => Ok(await _productVideoService.GetByIdsAsync(new string[] { id }));


        /// <summary>
        ///  Create new or update existing product video
        /// </summary>
        /// <param name="customerReviews">Customer reviews</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("")]
        [ProducesResponseType(typeof(void), statusCode: StatusCodes.Status204NoContent)]
        //[CheckPermission(Permission = Core.ModuleConstants.Security.Permissions.Create)]
        public IActionResult Update([FromBody] VideoLink[] videoLinks)
        {
            _productVideoService.SaveVideoLinksAsync(videoLinks);
            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Delete Videos by IDs
        /// </summary>
        /// <param name="ids">IDs</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Microsoft.AspNetCore.Mvc.Route("")]
        [ProducesResponseType(typeof(void), statusCode: StatusCodes.Status204NoContent)]
        //[CheckPermission(Permission = Core.ModuleConstants.Security.Permissions.Delete)]
        public IActionResult Delete(string[] ids)
        {
            _productVideoService.DeleteVideoLinksAsync(ids);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
