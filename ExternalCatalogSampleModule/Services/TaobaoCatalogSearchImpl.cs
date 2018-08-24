using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CacheManager.Core;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.SearchModule.Core.Model.Search;
using VirtoCommerce.SearchModule.Core.Model.Search.Criterias;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;

namespace External.CatalogModule.Web.Services
{
    public class TaobaoCatalogSearchImpl : CatalogServiceBase, ICatalogSearchService, IItemService, ISearchQueryBuilder
    {
       private Catalog _taobaoCatalog = new Catalog
       {
           Id = "Taobao",
           Name = "Taobao",
           Languages = new CatalogLanguage[] { new CatalogLanguage() { IsDefault = true, LanguageCode = "zh-CHS" } }.ToList()
       };
        private readonly ICatalogSearchService _catalogSearchService;
        private readonly ITopClient _topClient;
        private readonly IItemService _productService;
        private readonly ISearchQueryBuilder _luceneQueryBuilder;


        public TaobaoCatalogSearchImpl(Func<ICatalogRepository> catalogRepositoryFactory, ICacheManager<object> cacheManager, ICatalogSearchService catalogSearchService, ITopClient topClient, IItemService productService, ISearchQueryBuilder luceneQueryBuilder)
            : base(catalogRepositoryFactory, cacheManager)
        {
            _catalogSearchService = catalogSearchService;
            _topClient = topClient;
            _productService = productService;
            _luceneQueryBuilder = luceneQueryBuilder;
        }

        #region ISearchQueryBuilder
        public string DocumentType
        {
            get
            {
                return _luceneQueryBuilder.DocumentType;
            }
        }
        public object BuildQuery<T>(string scope, ISearchCriteria criteria) where T : class
        {
            return _luceneQueryBuilder.BuildQuery<T>(scope, criteria);
        } 
        #endregion

        public SearchResult Search(SearchCriteria criteria)
        {
            var retVal = _catalogSearchService.Search(criteria);
            if (criteria.ResponseGroup.HasFlag(SearchResponseGroup.WithCatalogs))
            {
                if (!retVal.Catalogs.Contains(_taobaoCatalog))
                {
                    retVal.Catalogs.Add(_taobaoCatalog);
                }
            }
            if (criteria.CatalogId.EqualsInvariant("Taobao") || criteria.CatalogIds.IsNullOrEmpty())
            {
            

                var request = new ItemsSearchRequest() {  Q = criteria.Keyword,  PageNo = criteria.Skip / Math.Max(1, criteria.Take), PageSize = criteria.Take };
                var itemsSearchResponse = _topClient.Execute<ItemsSearchResponse>(request);
                retVal.ProductsTotalCount += (int)itemsSearchResponse.TotalResults;
                foreach (var item in itemsSearchResponse.ItemSearch.Items)
                {
                    var product = new CatalogProduct
                    {
                        Id = item.NumIid.ToString(),
                        Catalog = _taobaoCatalog,
                        CatalogId = _taobaoCatalog.Id,
                        Code = item.NumIid.ToString(),
                        Name = item.Title,
                        Images = new Image[] { new Image { Url = item.PicUrl } }
                    };
                    retVal.Products.Add(product);
                }
            }           

            return retVal;
        }

        #region IItemService
        public CatalogProduct GetById(string itemId, ItemResponseGroup respGroup, string catalogId = null)
        {
            return GetByIds(new[] { itemId }, respGroup, catalogId).FirstOrDefault();
        }

        public CatalogProduct[] GetByIds(string[] itemIds, ItemResponseGroup respGroup, string catalogId = null)
        {
            var retVal = _productService.GetByIds(itemIds, respGroup, catalogId).ToList();
            var foundIds = retVal.Select(x => x.Id).ToArray();
            var taobaoProductIds = itemIds.Except(foundIds).Concat(retVal.Where(x => x.CatalogId.EqualsInvariant(_taobaoCatalog.Id)).Select(x => x.Id)).ToArray();
            foreach(var taobaoProductId in taobaoProductIds)
            {
                var request = new ItemGetRequest() { NumIid = Convert.ToInt64(taobaoProductId),
                    Fields = @"num_iid,title,nick,type,cid,seller_cids,props,pic_url,num,modified,product_id,item_img,prop_img,sku,video,outer_id,skus, desc",
                };
                var itemGetResponse = _topClient.Execute<ItemGetResponse>(request);
                if (itemGetResponse.Item != null)
                {
                    var topItem = itemGetResponse.Item;

                    var product = new dataModel.Item() { CatalogId = _taobaoCatalog.Id }.ToCoreModel(base.AllCachedCatalogs, base.AllCachedCategories);
                    product.Id = topItem.NumIid.ToString();
                    product.Catalog = _taobaoCatalog;
                    product.CatalogId = _taobaoCatalog.Id;
                    product.Code = topItem.Num.ToString();
                    product.Name = topItem.Title;
                    product.Images = new Image[] { new Image { Url = topItem.PicUrl } };
                    product.Reviews = new EditorialReview[] { new EditorialReview { ReviewType = "Description", LanguageCode = "en-US", Content = topItem.Desc } };

                    var existProduct = retVal.FirstOrDefault(x => x.Id == topItem.NumIid.ToString());
                    if (existProduct != null)
                    {
                        existProduct.Images = product.Images;
                        existProduct.Reviews = product.Reviews;
                    }
                    else
                    {
                        retVal.Add(product);
                    }
                }
            }
            return retVal.ToArray();
        }

        public CatalogProduct Create(CatalogProduct item)
        {
            return _productService.Create(item);
        }

        void IItemService.Delete(string[] itemIds)
        {
            _productService.Delete(itemIds);
        }

        public void Create(CatalogProduct[] items)
        {
            _productService.Create(items);
        }

        public void Update(CatalogProduct[] items)
        {
            var topItemsIds = items.Where(x => x.CatalogId.EqualsInvariant(_taobaoCatalog.Id)).Select(x => x.Id).ToArray();
            var notExistTaobaoItemsIds = topItemsIds.Except(_productService.GetByIds(topItemsIds, ItemResponseGroup.ItemInfo).Select(x => x.Id));
            if(notExistTaobaoItemsIds.Any())
            {
                _productService.Create(items.Where(x => notExistTaobaoItemsIds.Contains(x.Id)).ToArray());
            }
            _productService.Update(items);
        }

   
        #endregion
    }
}