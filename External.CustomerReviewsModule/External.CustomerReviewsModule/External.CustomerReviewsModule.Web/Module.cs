using System.Linq;
using External.CustomerReviewsModule.Core.Services;
using External.CustomerReviewsModule.Data.Repositories;
using External.CustomerReviewsModule.Data.Services;
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace External.CustomerReviewsModule.Web
{
    public class Module : ModuleBase
    {
        private readonly string _connectionString = ConfigurationHelper.GetConnectionStringValue("External.CustomerReviewsModule") ?? ConfigurationHelper.GetConnectionStringValue("VirtoCommerce");
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public override void SetupDatabase()
        {
            using (var db = new CustomerReviewRepository(_connectionString, _container.Resolve<AuditableInterceptor>()))
            {
                var initializer = new SetupDatabaseInitializer<CustomerReviewRepository, Data.Migrations.Configuration>();
                initializer.InitializeDatabase(db);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            // This method is called for each installed module on the first stage of initialization.

            // Register implementations:
            _container.RegisterType<ICustomerReviewRepository>(new InjectionFactory(c => new CustomerReviewRepository(_connectionString, new EntityPrimaryKeyGeneratorInterceptor(), _container.Resolve<AuditableInterceptor>())));
            _container.RegisterType<ICustomerReviewSearchService, CustomerReviewSearchService>();
            _container.RegisterType<ICustomerReviewService, CustomerReviewService>();

            // Try to avoid calling _container.Resolve<>();
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            // Registering settings to store module allows to use individual values in each store
            var settingManager = _container.Resolve<ISettingsManager>();
            var storeSettingsNames = new[] { "External.CustomerReviewsModule.CustomerReviewsEnabled" };
            var storeSettings = settingManager.GetModuleSettings("External.CustomerReviewsModule").Where(x => storeSettingsNames.Contains(x.Name)).ToArray();
            settingManager.RegisterModuleSettings("VirtoCommerce.Store", storeSettings);
        }
    }
}
