using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductVideoModule.Core;
using ProductVideoModule.Core.Services;

namespace ProductVideoModule.Web.Controllers.Api
{
    [Route("api/productVideo/youtube")]
    public class YoutubeController : Controller
    {
        private readonly IInternalYoutubeService _internalYoutubeService;

        public YoutubeController(IInternalYoutubeService internalYoutubeService)
        {
            _internalYoutubeService = internalYoutubeService;
        }

        /// <summary>
        /// Get video list from Youtube
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{keyWord}")]
        [ProducesResponseType(typeof(SearchListResponse), statusCode: StatusCodes.Status200OK)]
        //[CheckPermission(Permission = Core.ModuleConstants.Security.Permissions.Read)]
        public async Task<IActionResult> GetVideoList([FromRoute] string keyWord) => Ok(await _internalYoutubeService.SearchByExternalApi(keyWord));
    }
}
