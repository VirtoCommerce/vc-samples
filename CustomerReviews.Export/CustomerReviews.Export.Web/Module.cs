using CustomerReviews.Export.Data.ExportImport;
using Microsoft.Practices.Unity;
using VirtoCommerce.ExportModule.Core.Services;
using VirtoCommerce.ExportModule.Data.Extensions;
using VirtoCommerce.ExportModule.Data.Services;
using VirtoCommerce.Platform.Core.Modularity;

namespace CustomerReviews.Export.Web
{
    public class Module : ModuleBase
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public override void Initialize()
        {
            base.Initialize();

            _container.RegisterType<ICustomerReviewsExportPagedDataSourceFactory, CustomerReviewsExportPagedDataSourceFactory>();
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            var registrar = _container.Resolve<IKnownExportTypesRegistrar>();
            registrar.RegisterType(
                ExportedTypeDefinitionBuilder.Build<ExportableCustomerReview, CustomerReviewExportDataQuery>()
                    .WithDataSourceFactory(_container.Resolve<ICustomerReviewsExportPagedDataSourceFactory>())
                    .WithPermissionAuthorization(Core.ModuleConstants.Security.Permissions.Export, CustomerReviews.Core.ModuleConstants.Security.Permissions.CustomerReviewRead)
                    .WithMetadata(typeof(ExportableCustomerReview).GetPropertyNames())
                    .WithTabularMetadata(typeof(TabularCustomerReview).GetPropertyNames()));
        }
    }
}
