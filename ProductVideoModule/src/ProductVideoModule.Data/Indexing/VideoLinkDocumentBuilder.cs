using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductVideoModule.Core;
using ProductVideoModule.Core.Models;
using ProductVideoModule.Core.Services;
using VirtoCommerce.SearchModule.Core.Extenstions;
using VirtoCommerce.SearchModule.Core.Model;
using VirtoCommerce.SearchModule.Core.Services;

namespace ProductVideoModule.Data.Indexing
{
    public class VideoLinkDocumentBuilder : IIndexDocumentBuilder
    {
        private readonly IProductVideoService _productVideoService;

        public VideoLinkDocumentBuilder(IProductVideoService productVideoService)
        {
            _productVideoService = productVideoService;
        }

        public virtual async Task<IList<IndexDocument>> GetDocumentsAsync(IList<string> documentIds)
        {
            var categories = await _productVideoService.GetByIdsAsync(documentIds.ToArray());

            IList<IndexDocument> result = categories
                .Select(CreateDocument)
                .Where(doc => doc != null)
                .ToArray();

            return result;
        }

        protected virtual IndexDocument CreateDocument(VideoLink link)
        {
            var document = new IndexDocument(link.Id);

            document.AddFilterableValue("__sort", link.Url);

            var statusField = Enum.GetName(typeof(VideoLinkStatus), link.Status);
            IndexIsProperty(document, statusField);

            document.AddFilterableValue("status", statusField);
            document.AddFilterableValue("createddate", link.CreatedDate);
            document.AddFilterableValue("lastmodifieddate", link.ModifiedDate ?? DateTime.MaxValue);
            document.AddFilterableValue("modifieddate", link.ModifiedDate ?? DateTime.MaxValue);

            return document;
        }

        protected virtual void IndexIsProperty(IndexDocument document, string value)
        {
            document.Add(new IndexDocumentField("is", value) { IsRetrievable = true, IsFilterable = true, IsCollection = true });
        }
    }
}
