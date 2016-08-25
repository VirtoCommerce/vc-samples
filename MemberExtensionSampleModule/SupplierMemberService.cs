using System;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Data.Services;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.CustomerModule.Data.Model;
using System.Linq.Expressions;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Domain.Customer.Events;
using MemberExtensionSampleModule.Web.Model;
using VirtoCommerce.Domain.Commerce.Services;

namespace MemberExtensionSampleModule.Web
{
    /// <summary>
    ///  Provide CRUD operations for custom member instances.
    /// </summary>
    public class SupplierMemberService : CommerceMembersServiceImpl
    {
        public SupplierMemberService(Func<ICustomerRepository> repositoryFactory, IDynamicPropertyService dynamicPropertyService, ISecurityService securityService, IEventPublisher<MemberChangingEvent> eventPublisher, ICommerceService commerceService)
            :base(repositoryFactory, dynamicPropertyService, commerceService, securityService, eventPublisher)
        {
        }     
    

        //Override this method you can use for search members you custom tables and columns
        protected override Expression<Func<MemberDataEntity, bool>> GetQueryPredicate(MembersSearchCriteria criteria)
        {
            var retVal = base.GetQueryPredicate(criteria);
            if (criteria.Keyword != null)
            {
                var predicate = PredicateBuilder.False<MemberDataEntity>();
                predicate = predicate.Or(x => x is Contact2DataEntity && (x as Contact2DataEntity).JobTitle.Contains(criteria.Keyword));
                predicate = predicate.Or(x => x is SupplierDataEntity && (x as SupplierDataEntity).ContractNumber.Contains(criteria.Keyword));
                retVal = retVal.Or(LinqKit.Extensions.Expand(predicate));
            }           
            return retVal;
        }


    }
}