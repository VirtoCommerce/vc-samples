using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductVideoModule.Core;
using ProductVideoModule.Core.Models;
using ProductVideoModule.Core.Services;

namespace ProductVideoModule.Web.Controllers.Api
{
    public class ProductVideoController : Controller
    {
        private readonly IProductVideoService _productVideoService;

        public ProductVideoController(IProductVideoService productVideoService)
        {
            _productVideoService = productVideoService;
        }
        [HttpGet]
        [Route("productvideo")]
        //[Authorize(ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<VideoLink[]>> GetVideoList([FromQuery] int page = 0, [FromQuery] int size = 10)
        {
            var result = await _productVideoService.GetVideoPage(page, size);
            return Ok(result);
        }

        //[HttpPost]
        //[Route("productvideo/create")]
        //[Authorize(ModuleConstants.Security.Permissions.Create)]


    }
}
