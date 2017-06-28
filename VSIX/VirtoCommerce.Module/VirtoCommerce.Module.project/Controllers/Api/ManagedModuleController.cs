using System.Web.Http;

namespace $projectname$.Controllers.Api
{
    [RoutePrefix("api/$safeprojectname$")]
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
