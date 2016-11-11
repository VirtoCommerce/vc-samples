using System;
using MemberExtensionSampleModule.Web.Model;
using Microsoft.Practices.Unity;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Data.Services;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Domain.Customer.Events;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Common;
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

        public override void Initialize()
        {
            Func<SupplierRepository> contact2repositoryFactory = () => new SupplierRepository(_connectionStringName, new EntityPrimaryKeyGeneratorInterceptor(), _container.Resolve<AuditableInterceptor>());
            //Replace original repositories and services to own implementations 
            _container.RegisterInstance<Func<ICustomerRepository>>(contact2repositoryFactory);
            _container.RegisterInstance<Func<IMemberRepository>>(contact2repositoryFactory);

            _container.RegisterType<IMemberService, SupplierMemberService>();
            _container.RegisterType<IMemberSearchService, SupplierMemberService>();

            base.Initialize();

        }

        public override void PostInitialize()
        {
            AbstractTypeFactory<Member>.OverrideType<Contact, Model.Contact2>().MapToType<Contact2DataEntity>();
            AbstractTypeFactory<Member>.RegisterType<Model.Supplier>().MapToType<SupplierDataEntity>();
               
            AbstractTypeFactory<MemberDataEntity>.RegisterType<SupplierDataEntity>();
            AbstractTypeFactory<MemberDataEntity>.OverrideType<ContactDataEntity, Contact2DataEntity>();

            AbstractTypeFactory<MembersSearchCriteria>.RegisterType<Contact2SearchCriteria>();

            base.PostInitialize();
        }
     
        #endregion
    }
}