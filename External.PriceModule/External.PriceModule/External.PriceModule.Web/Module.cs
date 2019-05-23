using System;
using Microsoft.Practices.Unity;
using External.PriceModule.Core.Model;
using External.PriceModule.Data.Model;
using External.PriceModule.Data.Repositories;
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.PricingModule.Data.Model;
using VirtoCommerce.PricingModule.Data.Repositories;

namespace External.PriceModule.Web
{
    public class Module : ModuleBase
    {
        private readonly string _connectionString = ConfigurationHelper.GetConnectionStringValue("VirtoCommerce.Price") ?? ConfigurationHelper.GetConnectionStringValue("VirtoCommerce");
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public override void SetupDatabase()
        {
            base.SetupDatabase();

            using (var db = new PriceExRepository(_connectionString, _container.Resolve<AuditableInterceptor>()))
            {
                var initializer = new SetupDatabaseInitializer<PriceExRepository, Data.Migrations.Configuration>();
                initializer.InitializeDatabase(db);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            Func<IPricingRepository> repFactory = () =>
                   new PriceExRepository(_connectionString, new EntityPrimaryKeyGeneratorInterceptor(), _container.Resolve<AuditableInterceptor>(),
                       new ChangeLogInterceptor(_container.Resolve<Func<IPlatformRepository>>(), ChangeLogPolicy.Cumulative, new[] { nameof(PriceExEntity) }));
            _container.RegisterInstance(instance: repFactory);

            _container.RegisterType<IPricingRepository>(new InjectionFactory(c => new PriceExRepository(_connectionString, _container.Resolve<AuditableInterceptor>(),
                    new EntityPrimaryKeyGeneratorInterceptor())));
        }

        public override void PostInitialize()
        {
            base.Initialize();

            AbstractTypeFactory<Price>.OverrideType<Price, PriceEx>();
            AbstractTypeFactory<PriceEntity>.OverrideType<PriceEntity, PriceExEntity>();
        }
    }
}
