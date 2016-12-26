using CartModule2.Model;
using CartModule2.Repositories;
using Microsoft.Practices.Unity;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace CartModule2
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
            using (var db = new Cart2Repository(_connectionStringName, _container.Resolve<AuditableInterceptor>()))
            {
                var initializer = new SetupDatabaseInitializer<Cart2Repository, CartModule2.Migrations.Configuration>();
                initializer.InitializeDatabase(db);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            _container.RegisterType<ICartRepository>(new InjectionFactory(c => new Cart2Repository(_connectionStringName, _container.Resolve<AuditableInterceptor>(), new EntityPrimaryKeyGeneratorInterceptor())));
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            AbstractTypeFactory<ShoppingCart>.OverrideType<ShoppingCart, Cart2>();
            AbstractTypeFactory<ShoppingCartEntity>.OverrideType<ShoppingCartEntity, Cart2Entity>();
            AbstractTypeFactory<LineItem>.OverrideType<LineItem, LineItem2>();
            AbstractTypeFactory<LineItemEntity>.OverrideType<LineItemEntity, LineItem2Entity>();
        }

        #endregion
    }
}
