using System;
using Microsoft.Practices.Unity;
using OrderModule2.Model;
using OrderModule2.Web.Model;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace OrderModule2.Web
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
            using (var db = new OrderRepository2(_connectionStringName, _container.Resolve<AuditableInterceptor>()))
            {
                var initializer = new SetupDatabaseInitializer<OrderRepository2, OrderModule2.Migrations.Configuration>();
                initializer.InitializeDatabase(db);
            }

            
        }

        public override void Initialize()
        {
            base.Initialize();

            _container.RegisterType<IOrderRepository>(new InjectionFactory(c => new OrderRepository2(_connectionStringName, _container.Resolve<AuditableInterceptor>(), new EntityPrimaryKeyGeneratorInterceptor())));

         
        }

        public override void PostInitialize()
        {
            base.Initialize();
            AbstractTypeFactory<IOperation>.OverrideType<CustomerOrder, CustomerOrder2>();
            AbstractTypeFactory<CustomerOrderEntity>.OverrideType<CustomerOrderEntity, CustomerOrder2Entity>();
            AbstractTypeFactory<CustomerOrder>.OverrideType<CustomerOrder, CustomerOrder2>()
                                           .WithFactory(() => new CustomerOrder2 { OperationType = "CustomerOrder" });
            AbstractTypeFactory<LineItem>.OverrideType<LineItem, LineItem2>();
            AbstractTypeFactory<LineItemEntity>.OverrideType<LineItemEntity, LineItem2Entity>();
            //Thats need for PolymorphicOperationJsonConverter for API deserialization
            AbstractTypeFactory<IOperation>.RegisterType<Invoice>();
        }
     
        #endregion
    }
}