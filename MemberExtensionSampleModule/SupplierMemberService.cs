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

namespace MemberExtensionSampleModule.Web
{
    /// <summary>
    ///  Provide CRUD operations for custom member instances.
    /// </summary>
    public class SupplierMemberService : CommerceMembersServiceImpl
    {
        public SupplierMemberService(Func<ICustomerRepository> repositoryFactory, IDynamicPropertyService dynamicPropertyService, ISecurityService securityService, IMemberFactory memberFactory, IEventPublisher<MemberChangingEvent> eventPublisher)
            :base(repositoryFactory, dynamicPropertyService, securityService, memberFactory, eventPublisher)
        {
        }

        //Override this method you can construct you customer data model from domain member type instance
        protected override MemberDataEntity TryCreateDataMember(Member member)
        {
            MemberDataEntity retVal = null;
            var contact2 = member as Contact2;
            var supplier = member as Supplier;
            if(contact2 != null)
            {
                retVal = new Contact2DataEntity();
            }
            if(supplier != null)
            {
                retVal = new SupplierDataEntity();
            }
            return retVal;
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