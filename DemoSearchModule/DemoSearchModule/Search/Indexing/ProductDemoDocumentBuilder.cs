using VirtoCommerce.CatalogModule.Data.Search.Indexing;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Settings;

namespace DemoSearchModule.Search.Indexing
{
    public class ProductDemoDocumentBuilder : ProductDocumentBuilder, IIndexDocumentBuilder
    {
        public ProductDemoDocumentBuilder(ISettingsManager settingsManager, IItemService itemService, IBlobUrlResolver blobUrlResolver) : base(settingsManager, itemService, blobUrlResolver)
        {
        }

        protected override IndexDocument CreateDocument(CatalogProduct product)
        {
            var document = new IndexDocument(product.Id);

            // adding an index field for the first letter of name 
            document.Add(new IndexDocumentField(FiltersHelper.FirstLetterField, product.Name.ToUpper()[0])
            {
                IsRetrievable = true,
                IsFilterable = true
                //IsSearchable = true
            });

            return document;
        }
    }
}