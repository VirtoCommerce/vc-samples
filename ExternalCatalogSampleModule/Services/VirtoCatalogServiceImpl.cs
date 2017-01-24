using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CacheManager.Core;
using External.CatalogModule.Web.CatalogModuleApi;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Omu.ValueInjecter;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.SearchApiModule.Data.Model;
using VirtoCommerce.SearchModule.Core.Model.Indexing;
using VirtoCommerce.SearchModule.Core.Model.Search;
using VirtoCommerce.SearchModule.Core.Model.Search.Criterias;
using VirtoCommerce.SearchModule.Data.Providers.Lucene;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;

namespace External.CatalogModule.Web.Services
{
    public class VirtoCatalogSearchImpl : CatalogServiceBase, ICatalogSearchService, IItemService, ICategoryService, ISearchQueryBuilder
    {
   
        private readonly ICatalogSearchService _catalogSearchService;
        private readonly Func<string, ICatalogModuleApiClient> _vcCatalogClientFactory;
        private readonly IItemService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISearchQueryBuilder _luceneQueryBuilder;
        private readonly IUserNameResolver _userNameResolver;
        private readonly IMemberService _memberService;
        private readonly IMemberSearchService _memberSearchService;
        private readonly ISecurityService _securityService;
        private readonly ICatalogService _catalogService;

        public VirtoCatalogSearchImpl(Func<ICatalogRepository> catalogRepositoryFactory, ICacheManager<object> cacheManager,
                                      ICatalogSearchService catalogSearchService, Func<string, ICatalogModuleApiClient> vcCatalogClientFactory, 
                                      IItemService productService, ICategoryService categoryService, IUserNameResolver userNameResolver,
                                      ISearchQueryBuilder luceneQueryBuilder, IMemberService memberService, 
                                      ISecurityService securityService, ICatalogService catalogService)
            :base(catalogRepositoryFactory, cacheManager)
        {
            _catalogSearchService = catalogSearchService;
            _vcCatalogClientFactory = vcCatalogClientFactory;
            _productService = productService;
            _categoryService = categoryService;
            _luceneQueryBuilder = luceneQueryBuilder;
            _userNameResolver = userNameResolver;
            _memberService = memberService;
            _securityService = securityService;
            _catalogService = catalogService;
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
            var queryBuilder = _luceneQueryBuilder.BuildQuery<T>(scope, criteria) as QueryBuilder;

            if (criteria is CatalogItemSearchCriteria)
            {
                var userName = _userNameResolver.GetCurrentUserName();
                var userAccount = _securityService.FindByNameAsync(userName, UserDetails.Reduced).Result;
                if (userAccount != null && !userAccount.MemberId.IsNullOrEmpty())
                {
                    var contact = _memberService.GetByIds(new[] { userAccount.MemberId }).FirstOrDefault();
                    if (contact != null)
                    {
                        var userExperienceDp = contact.DynamicProperties.FirstOrDefault(x => x.Name.EqualsInvariant("userexperience"));
                        if (userExperienceDp != null && !userExperienceDp.Values.IsNullOrEmpty())
                        {
                            var userExpValue = userExperienceDp.Values.First().Value as DynamicPropertyDictionaryItem;
                            if (userExpValue != null)
                            {
                                var catalogSearchCriteria = criteria as CatalogItemSearchCriteria;
                                var query = queryBuilder.Query as BooleanQuery;
                                query.Add(new TermQuery(new Term(userExperienceDp.Name, userExpValue.Name)), Occur.MUST);
                            }
                        }
                    }
                }
            }
            return queryBuilder;
        }
        #endregion

        public SearchResult Search(SearchCriteria criteria)
        {
            var retVal = _catalogSearchService.Search(criteria);
            if (!string.IsNullOrEmpty(criteria.CatalogId))
            {
                var catalog = base.AllCachedCatalogs.FirstOrDefault(x => x.Id.EqualsInvariant(criteria.CatalogId));
                if(catalog != null && !catalog.Virtual)
                {
                    criteria.WithHidden = false;
                }                
            }
            var extCatalogs = GetExternalCatalogs();
            if (!string.IsNullOrEmpty(criteria.CatalogId))
            {
                extCatalogs = extCatalogs.Where(x => x.Id.EqualsInvariant(criteria.CatalogId));
            }
            var vcSearchCriteria = new External.CatalogModule.Web.CatalogModuleApi.Models.SearchCriteria();
            vcSearchCriteria.InjectFrom(criteria);
            vcSearchCriteria.Skip = criteria.Skip;
            vcSearchCriteria.Take = criteria.Take;
            vcSearchCriteria.Sort = criteria.Sort;
            vcSearchCriteria.WithHidden = false;
            vcSearchCriteria.SearchInChildren = vcSearchCriteria.CatalogId.IsNullOrEmpty() ? true : criteria.SearchInChildren;
            vcSearchCriteria.ResponseGroup = criteria.ResponseGroup.ToString();         

            foreach (var extCatalog in extCatalogs)
            {
                var apiUrl = extCatalog.PropertyValues.FirstOrDefault(x => x.PropertyName.EqualsInvariant("apiUrl")).Value.ToString();
                if (criteria.Take >= 0)
                {
                    vcSearchCriteria.CatalogId = extCatalog.Id;
                    var result = _vcCatalogClientFactory(apiUrl).CatalogModuleSearch.Search(vcSearchCriteria);

                    retVal.ProductsTotalCount += result.ProductsTotalCount.Value;
                    vcSearchCriteria.Skip = Math.Max(0, criteria.Skip - retVal.ProductsTotalCount);
                    vcSearchCriteria.Take = Math.Max(0, criteria.Take - retVal.Products.Count());
                    foreach (var dtoCategory in result.Categories)
                    {
                        if (!retVal.Categories.Any(x => x.Id.EqualsInvariant(dtoCategory.Id)))
                        {
                            var cat = new Category();
                            cat.InjectFrom(dtoCategory);
                            retVal.Categories.Add(cat);
                        }
                    }
                    foreach (var dtoProduct in result.Products)
                    {
                        if (!retVal.Products.Any(x => x.Id.EqualsInvariant(dtoProduct.Id)))
                        {
                            var prod = ConvertToProduct(dtoProduct);
                            retVal.Products.Add(prod);
                        }
                    }                   
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
            var extCatalogs = GetExternalCatalogs();
            foreach (var extCatalog in extCatalogs)
            {
                var apiUrl = extCatalog.PropertyValues.FirstOrDefault(x => x.PropertyName.EqualsInvariant("apiUrl")).Value.ToString();
                var chunkSize = 30;
                for (int i = 0; i < itemIds.Count(); i += chunkSize)
                {
                    var externalProducts = _vcCatalogClientFactory(apiUrl).CatalogModuleProducts.GetProductByIds(itemIds.Skip(i).Take(chunkSize).ToArray(), respGroup.ToString());
                    foreach (var externalProduct in externalProducts)
                    {
                        var product = ConvertToProduct(externalProduct);
                        var existProduct = retVal.FirstOrDefault(x => x.Id.EqualsInvariant(externalProduct.Id));
                        if (existProduct != null)
                        {
                            retVal.Remove(existProduct);
                            product.Properties = existProduct.Properties;
                            product.PropertyValues = existProduct.PropertyValues;
                            product.Links = existProduct.Links;
                            if (product.Outlines.IsNullOrEmpty())
                            {
                                product.Outlines = existProduct.Outlines;
                            }
                            else if (!existProduct.Outlines.IsNullOrEmpty())
                            {
                                product.Outlines.AddRange(existProduct.Outlines);
                            }                       
                        }
                        retVal.Add(product);
                    }
                }
            }

            foreach(var product in retVal.Where(x=>x.CategoryId != null))
            {
                var dataCategory = base.AllCachedCategories.FirstOrDefault(x => x.Id == product.CategoryId);
                if(dataCategory != null)
                {
                    var userExpProperty = dataCategory.CategoryPropertyValues.FirstOrDefault(x => x.Name.EqualsInvariant("userexperience"));
                    if (userExpProperty != null)
                    {
                        product.PropertyValues.Add(userExpProperty.ToCoreModel());
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
            var extCatalogs = GetExternalCatalogs();
            var externalProducts = items.Where(x => extCatalogs.Any(y => y.Id.EqualsInvariant(x.CatalogId)));
            var externalProductIds = externalProducts.Select(x => x.Id).ToArray();
            foreach (var externalProduct in externalProducts)
            {
                externalProduct.CategoryId = null;
                externalProduct.IsActive = false;
            }       
            var notExistProductIds = externalProductIds.Except(_productService.GetByIds(externalProductIds, ItemResponseGroup.ItemInfo).Select(x => x.Id));
            if(notExistProductIds.Any())
            {
                _productService.Create(items.Where(x => notExistProductIds.Contains(x.Id)).ToArray());
            }
            _productService.Update(items);
        }
        #endregion

        #region Category service
        public Category[] GetByIds(string[] categoryIds, CategoryResponseGroup responseGroup, string catalogId = null)
        {
            var extCatalogs = GetExternalCatalogs();
            var retVal = _categoryService.GetByIds(categoryIds, responseGroup, catalogId).ToList();
            foreach(var extCatalog in extCatalogs)
            {
                var apiUrl = extCatalog.PropertyValues.FirstOrDefault(x => x.PropertyName.EqualsInvariant("apiUrl")).Value.ToString();
                var extCategories = _vcCatalogClientFactory(apiUrl).CatalogModuleCategories.GetCategoriesByIds(categoryIds.ToList(), responseGroup.ToString());
                foreach (var extCategory in extCategories)
                {
                    var category = ConvertToCategory(extCategory);
                    var localCategory = retVal.FirstOrDefault(x => x.Id.EqualsInvariant(extCategory.Id));
                    if (localCategory != null)
                    {
                        retVal.Remove(localCategory);
                        category.Properties = localCategory.Properties;
                        category.PropertyValues = localCategory.PropertyValues;
                        category.Links = localCategory.Links;
                        if (category.Outlines.IsNullOrEmpty())
                        {
                            category.Outlines = localCategory.Outlines;
                        }
                        else if (!localCategory.Outlines.IsNullOrEmpty())
                        {
                            category.Outlines.AddRange(localCategory.Outlines);
                        }                       
                    }
                    retVal.Add(category);
                }
            }
           
            return retVal.ToArray();
        }

        public Category GetById(string categoryId, CategoryResponseGroup responseGroup, string catalogId = null)
        {
            return GetByIds(new string[] { categoryId }, responseGroup, catalogId).FirstOrDefault();
        }

        public void Create(Category[] categories)
        {           
            _categoryService.Create(categories);
        }

        public Category Create(Category category)
        {
           return _categoryService.Create(category);
        }

        public void Update(Category[] categories)
        {
            var extCatalogs = GetExternalCatalogs();
            var externalCategories = categories.Where(x => extCatalogs.Any(y => y.Id.EqualsInvariant(x.CatalogId)));
            var externalCategoriesIds = externalCategories.Select(x => x.Id).ToArray();          
            var notExistCategoriesIds = externalCategoriesIds.Except(_categoryService.GetByIds(externalCategoriesIds, CategoryResponseGroup.Info).Select(x => x.Id));
            if (notExistCategoriesIds.Any())
            {
                _categoryService.Create(categories.Where(x => notExistCategoriesIds.Contains(x.Id)).ToArray());
            }
            _categoryService.Update(categories);
        }

        public void Delete(string[] categoryIds)
        {
            _categoryService.Delete(categoryIds);
        }
        #endregion

        private Category ConvertToCategory(External.CatalogModule.Web.CatalogModuleApi.Models.Category dto)
        {
            var dbCategory = new dataModel.Category()
            {
                Id = dto.Id,
                CatalogId = dto.CatalogId,
            };

            var retVal = dbCategory.ToCoreModel(base.AllCachedCatalogs, base.AllCachedCategories.Concat(new[] { dbCategory }).ToArray());
            retVal.Parents = new Category[] { };
            retVal.Links = new List<CategoryLink>();
            retVal.PropertyValues = new List<PropertyValue>();
            retVal.InjectFrom(dto);

            if (!dto.Outlines.IsNullOrEmpty())
            {
                retVal.Outlines = dto.Outlines.Select(x => ConvertToOutline(x)).ToList();

            }
            return retVal;
        }
        private CatalogProduct ConvertToProduct(External.CatalogModule.Web.CatalogModuleApi.Models.Product dto)
        {
            var retVal = new dataModel.Item() { CatalogId = dto.CatalogId }.ToCoreModel(base.AllCachedCatalogs, base.AllCachedCategories);
            retVal.InjectFrom(dto);
            if(!dto.Images.IsNullOrEmpty())
            {
                retVal.Images = dto.Images.Select(x => new Image().InjectFrom(x) as Image).ToList();
            }
            if(!dto.Reviews.IsNullOrEmpty())
            {
                retVal.Reviews = dto.Reviews.Select(x => new EditorialReview().InjectFrom(x) as EditorialReview).ToList();
            }
            if(!dto.Outlines.IsNullOrEmpty())
            {
                retVal.Outlines = dto.Outlines.Select(x => ConvertToOutline(x)).ToList();
            
            }
            return retVal;
        }

        private Outline ConvertToOutline(External.CatalogModule.Web.CatalogModuleApi.Models.Outline outlineDto)
        {
            var outline = new Outline
            {
                Items = new List<OutlineItem>()
            };
            foreach (var itemDto in outlineDto.Items)
            {
                var item = new OutlineItem();
                item.InjectFrom(itemDto);
                item.SeoInfos = itemDto.SeoInfos.Select(x => new SeoInfo().InjectFrom(x) as SeoInfo).ToList();
                outline.Items.Add(item);
            }
            return outline;
        }

        private IEnumerable<Catalog> GetExternalCatalogs()
        {
             return _catalogService.GetCatalogsList().ToArray().Where(x => x.PropertyValues.Any(y => y.PropertyName.EqualsInvariant("apiUrl")));
        }
    }
}