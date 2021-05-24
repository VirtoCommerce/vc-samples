using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
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
        private readonly IValidator<ProductVideoSearchCriteria> _validator;

        public ProductVideoController(
            IProductVideoService productVideoService,
            IProductVideoSearchService productVideoSearchService,
            IValidator<ProductVideoSearchCriteria> validator)
        {
            _productVideoService = productVideoService;
            _productVideoSearchService = productVideoSearchService;
            _validator = validator;
        }

        /// <summary>
        /// Return product Video search results
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ProducesResponseType(typeof(GenericSearchResult<VideoLink>), statusCode: StatusCodes.Status200OK)]
        //[CheckPermission(Permission = Core.ModuleConstants.Security.Permissions.Read)]
        public async Task<IActionResult> SearchProductVideos([FromBody] ProductVideoSearchCriteria criteria)
        {
            var validationResult = await _validator.ValidateAsync(criteria);

            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join(' ', validationResult.Errors.Select(x => x.ErrorMessage)));
            }

            var result = await _productVideoSearchService.SearchVideoLinksAsync(criteria);
            return Ok(result);
        }

        /// <summary>
        /// Get video by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(VideoLink), statusCode: StatusCodes.Status200OK)]
        //[CheckPermission(Permission = Core.ModuleConstants.Security.Permissions.Read)]
        public async Task<IActionResult> GetById([FromRoute] string id) => Ok(await _productVideoService.GetByIdsAsync(new string[] { id }));

        /// <summary>
        ///  Create new or update existing product video
        /// </summary>
        /// <param name="customerReviews">Customer reviews</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(void), statusCode: StatusCodes.Status204NoContent)]
        //[CheckPermission(Permission = Core.ModuleConstants.Security.Permissions.Create)]
        public async Task<IActionResult> Update([FromBody] VideoLink[] videoLinks)
        {
            await _productVideoService.SaveVideoLinksAsync(videoLinks);
            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Delete Videos by IDs
        /// </summary>
        /// <param name="ids">IDs</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        [ProducesResponseType(typeof(void), statusCode: StatusCodes.Status204NoContent)]
        //[CheckPermission(Permission = Core.ModuleConstants.Security.Permissions.Delete)]
        public async Task<IActionResult> Delete([FromQuery] string[] ids)
        {
            await _productVideoService.DeleteVideoLinksAsync(ids);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
