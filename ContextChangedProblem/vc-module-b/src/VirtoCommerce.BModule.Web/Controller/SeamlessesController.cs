using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VirtoCommerce.AModule.Data.Repositories;
using VirtoCommerce.BModule.Data.Repositories;

namespace VirtoCommerce.BModule.Web.Controller
{
    [Route("api")]
    public class SeamlessesController : ControllerBase
    {
        private readonly ADbContext _aDbContext;
        private readonly BDbContext _bDbContext;
        private readonly ILogger<SeamlessesController> _logger;

        public SeamlessesController(ADbContext aDbContext, BDbContext bDbContext, ILogger<SeamlessesController> logger)
        {
            _aDbContext = aDbContext;
            _bDbContext = bDbContext;
            _logger = logger;
        }

        [HttpGet]
        [Route("b/seamlesses")]
        public ActionResult Seamlesses()
        {
            _logger.LogInformation("before seamll");
            var dsddddd = (new ARepository(_aDbContext)).Seamlesses.ToArray();
            var dsddddd1 = (new BRepository(_bDbContext)).Seamlesses.ToArray();
            //var dsddddd2 = (new BRepository(_bDbContext)).Seamlesses2;
            _logger.LogInformation("after seamll");
            return Ok();
        }

    }
}
