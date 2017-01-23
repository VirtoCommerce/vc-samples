using System;
using System.Linq;
using CacheManager.Core;
using External.CatalogModule.Web.ApiClient;
using External.CatalogModule.Web.CatalogModuleApi;
using External.CatalogModule.Web.Services;
using Microsoft.Practices.Unity;
using Top.Api;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.SearchModule.Core.Model.Search;

namespace External.CatalogModule.Web
{
    public class Module : ModuleBase
    {
        private const string _connectionStringName = "VirtoCommerce";
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }
        #region IModule Members

        public override void SetupDatabase()
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            var topClient = new DefaultTopClient("http://gw.api.taobao.com/router/rest", "****", "*****", null);

            var catalogService = _container.Resolve<ICatalogService>();
            //var taobaoCatalog = catalogService.GetById("Taobao");
            //if (taobaoCatalog == null)
            //{
            //    taobaoCatalog = new Catalog
            //    {
            //        Id = "Taobao",
            //        Name = "Taobao",
            //        Languages = new CatalogLanguage[] { new CatalogLanguage() { IsDefault = true, LanguageCode = "zh-CHS" } }.ToList()
            //    };
            //    catalogService.Create(taobaoCatalog);
            //}
            var vcDemoCatalog = catalogService.GetById("4974648a41df4e6ea67ef2ad76d7bbd4");
            if (vcDemoCatalog == null)
            {
                vcDemoCatalog = new Catalog
                {
                    Id = "4974648a41df4e6ea67ef2ad76d7bbd4",
                    Name = "demo.virtocommerce.com",
                    Languages = new CatalogLanguage[] { new CatalogLanguage() { IsDefault = true, LanguageCode = "en-US" } }.ToList()
                };
                catalogService.Create(vcDemoCatalog);
            }
            _container.RegisterInstance<ITopClient>(topClient);
            var virtoApiClient = new CatalogModuleApiClient(new Uri("http://demo.virtocommerce.com/admin"), new VirtoApiSecurityRequest("27e0d789f12641049bd0e939185b4fd2", "34f0a3c12c9dbb59b63b5fece955b7b2b9a3b20f84370cba1524dd5c53503a2e2cb733536ecf7ea1e77319a47084a3a2c9d94d36069a432ecc73b72aeba6ea78"));
            _container.RegisterInstance<ICatalogModuleApiClient>(virtoApiClient);
            var luceneQueryBuilder = _container.Resolve<ISearchQueryBuilder>("lucene");
            var vcDemoCatalogService = new VirtoCatalogSearchImpl(_container.Resolve<Func<ICatalogRepository>>(), _container.Resolve<ICacheManager<object>>(), _container.Resolve<ICatalogSearchService>(), virtoApiClient, _container.Resolve<IItemService>(), _container.Resolve<ICategoryService>(),
                                                                  _container.Resolve<IUserNameResolver>(), luceneQueryBuilder, _container.Resolve<IMemberService>(), _container.Resolve<ISecurityService>());
            _container.RegisterInstance<ICatalogSearchService>(vcDemoCatalogService);           
            _container.RegisterInstance<IItemService>(vcDemoCatalogService);
            _container.RegisterInstance<ICategoryService>(vcDemoCatalogService);
            _container.RegisterInstance<ISearchQueryBuilder>("lucene", vcDemoCatalogService);



            //var taobaoCatalogService = new TaobaoCatalogSearchImpl(_container.Resolve<Func<ICatalogRepository>>(), _container.Resolve<ICacheManager<object>>(), _container.Resolve<ICatalogSearchService>(), topClient, _container.Resolve<IItemService>());
            //_container.RegisterInstance<ICatalogSearchService>(taobaoCatalogService);
            //_container.RegisterInstance<IItemService>(taobaoCatalogService);
        }

        public override void PostInitialize()
        {
            base.Initialize();

        }

        #endregion
    }
}