using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProductVideoModule.Core.Models.Search;
using ProductVideoModule.Web.Validators;

namespace ProductVideoModule.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddValidators(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IValidator<ProductVideoSearchCriteria>, ProductVideoSearchCriteriaValidator>();
        }
    }
}
