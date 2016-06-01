using System;
using Microsoft.Practices.Unity;
using VirtoCommerce.CustomerModule.Data.Services;
using VirtoCommerce.Domain.Customer.Events;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace MemberExtensionSampleModule.Web
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
            using (var db = new SupplierRepository(_connectionStringName, _container.Resolve<AuditableInterceptor>()))
            {
                var initializer = new SetupDatabaseInitializer<SupplierRepository, MemberExtensionSampleModule.Web.Migrations.Configuration>();
                initializer.InitializeDatabase(db);
            }
        }

        public override void PostInitialize()
        {
            var memberServiceDecorator = _container.Resolve<MemberServiceDecorator>();

            Func<SupplierRepository> contact2repositoryFactory = () => new SupplierRepository(_connectionStringName, new EntityPrimaryKeyGeneratorInterceptor(), _container.Resolve<AuditableInterceptor>());
            var contactExtModuleMemberservice = new SupplierMemberService(contact2repositoryFactory, _container.Resolve<IDynamicPropertyService>(), _container.Resolve<ISecurityService>(), _container.Resolve<IMemberFactory>(), _container.Resolve<IEventPublisher<MemberChangingEvent>>());
            memberServiceDecorator.OverrideMemberType<Contact>()
                                  .WithType<Model.Contact2>()
                                  .WithService(contactExtModuleMemberservice)
                                  .WithSearchService(contactExtModuleMemberservice);

            memberServiceDecorator.RegisterMemberTypes(typeof(Model.Supplier))
                                  .WithService(contactExtModuleMemberservice)
                                  .WithSearchService(contactExtModuleMemberservice);
  

        }
     
        #endregion
    }
}