using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using ProductVideoModule.Core.Models.Search;

namespace ProductVideoModule.Web.Validators
{
    public class ProductVideoSearchCriteriaValidator : AbstractValidator<ProductVideoSearchCriteria>
    {
        public ProductVideoSearchCriteriaValidator()
        {
            RuleFor(x => x.ProductIds).NotEmpty().WithMessage("Search request must contain a product Id.");
            RuleFor(x => x.Skip).GreaterThan(-1)
                .WithMessage("Search request parameter 'Skip' must be not negative integer digit.");
            RuleFor(x => x.Take).InclusiveBetween(0, 100)
                .WithMessage("Search request parameter 'Take' must be positive integer digit between '0' and '100' inclusively");
        }
    }
}
