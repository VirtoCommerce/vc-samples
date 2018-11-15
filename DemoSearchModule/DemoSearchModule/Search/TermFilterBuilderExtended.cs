using System.Linq;
using VirtoCommerce.CatalogModule.Data.Search;
using VirtoCommerce.CatalogModule.Data.Search.BrowseFilters;
using VirtoCommerce.Domain.Catalog.Model.Search;
using VirtoCommerce.Domain.Search;

namespace DemoSearchModule.Search
{
    public class TermFilterBuilderExtended : TermFilterBuilder
    {
        public TermFilterBuilderExtended(IBrowseFilterService browseFilterService) : base(browseFilterService)
        {
        }

        public override FiltersContainer GetTermFilters(ProductSearchCriteria criteria)
        {
            var result = base.GetTermFilters(criteria);

            // starts_with filter
            var startsWithFilter = result.PermanentFilters.OfType<TermFilter>().SingleOrDefault(f => f.FieldName == FiltersHelper.StartsWithFilter);
            if (startsWithFilter != null)
            {
                var firstLetterFilters = new[] {
                    new TermFilter {
                        FieldName = FiltersHelper.FirstLetterField,
                        Values = startsWithFilter.Values.Select(x => x.ToUpper()).ToArray()
                    }
                };

                result.PermanentFilters.Remove(startsWithFilter);
                result.PermanentFilters.Add(firstLetterFilters.And());
            }

            return result;
        }
    }
}
