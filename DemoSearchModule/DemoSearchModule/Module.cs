using System.Collections.Generic;
using System.Linq;
using DemoSearchModule.Search;
using DemoSearchModule.Search.Indexing;
using Microsoft.Practices.Unity;
using VirtoCommerce.CatalogModule.Data.Search;
using VirtoCommerce.CatalogModule.Data.Search.Indexing;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Platform.Core.Modularity;

namespace DemoSearchModule
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

            // This method is called for each installed module on the first stage of initialization.

            // search
            _container.RegisterType<ITermFilterBuilder, TermFilterBuilderExtended>();
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            // extent indexing by adding new DocumentBuilder (ProductDemoDocumentBuilder)
            var productIndexingConfigurations = _container.Resolve<IndexDocumentConfiguration[]>();
            if (productIndexingConfigurations != null)
            {
                var productCompletenessDocumentSource = new IndexDocumentSource
                {
                    ChangesProvider = _container.Resolve<ProductDocumentChangesProvider>(),
                    DocumentBuilder = _container.Resolve<ProductDemoDocumentBuilder>(),
                };

                foreach (var configuration in productIndexingConfigurations.Where(c => c.DocumentType == KnownDocumentTypes.Product))
                {
                    if (configuration.RelatedSources == null)
                    {
                        configuration.RelatedSources = new List<IndexDocumentSource>();
                    }

                    configuration.RelatedSources.Add(productCompletenessDocumentSource);
                }
            }
        }
    }
}
