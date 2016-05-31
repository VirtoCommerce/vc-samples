using System.Web.Http;

namespace VirtoCommerce.ManagedModule.Web.Controllers.Api
{
    [RoutePrefix("api/managedModule")]
    public class ManagedModuleController : ApiController
    {
        // GET: api/managedModule
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(new { result = "Hello world!" });
        }
    }
}
