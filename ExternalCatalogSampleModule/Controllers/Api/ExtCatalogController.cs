using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using External.CatalogModule.Web.CatalogModuleApi;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;

namespace External.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalogs/external")]
    public class ExtCatalogModuleCatalogsController : ApiController
    {
        private readonly Func<string, ICatalogModuleApiClient> _extCatalogApiFactory;
        private readonly ICatalogService _catalogService;

        public ExtCatalogModuleCatalogsController(Func<string, ICatalogModuleApiClient> extCatalogApiFactory, ICatalogService catalogService)
        {
            _extCatalogApiFactory = extCatalogApiFactory;
            _catalogService = catalogService;
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(CatalogModuleApi.Models.Catalog[]))]
        public IHttpActionResult DiscoverExtCatalogs([FromUri]string apiUrl)
        {
            var extCatalogApi = _extCatalogApiFactory(apiUrl);
                     
            var result = extCatalogApi.CatalogModuleCatalogs.GetCatalogs();
            return Ok(result);
        }
   
    }
}