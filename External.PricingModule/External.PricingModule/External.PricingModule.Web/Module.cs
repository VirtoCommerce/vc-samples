using System;
using External.PricingModule.Core.Models;
using External.PricingModule.Data.ExportImport;
using External.PricingModule.Data.Models;
using External.PricingModule.Data.Repositories;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.ExportModule.Core.Services;
using VirtoCommerce.ExportModule.Data.Extensions;
using VirtoCommerce.ExportModule.Data.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.PricingModule.Data.ExportImport;
using VirtoCommerce.PricingModule.Data.Model;
using VirtoCommerce.PricingModule.Data.Repositories;

namespace External.PricingModule.Web
{
    public class Module : ModuleBase
    {
        private readonly string _connectionString = ConfigurationHelper.GetConnectionStringValue("VirtoCommerce.Pricing") ?? ConfigurationHelper.GetConnectionStringValue("VirtoCommerce");
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

            #region Generic Export

            AbstractTypeFactory<ExportablePrice>.OverrideType<ExportablePrice, ExportablePriceEx>();
            AbstractTypeFactory<TabularPrice>.OverrideType<TabularPrice, TabularPriceEx>();

            var exportTypesResolver = _container.Resolve<IKnownExportTypesResolver>();
            new ExportedTypeDefinitionBuilder(exportTypesResolver.ResolveExportedTypeDefinition(typeof(ExportablePrice).FullName))
                .WithMetadata(typeof(ExportablePriceEx).GetPropertyNames())
                .WithTabularMetadata(typeof(TabularPriceEx).GetPropertyNames());

            #endregion Generic Export
        }
    }
}
