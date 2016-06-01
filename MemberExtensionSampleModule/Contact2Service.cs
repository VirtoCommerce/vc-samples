using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactExtModule.Web.Model;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Data.Services;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.CustomerModule.Data.Model;
using System.Linq.Expressions;
using VirtoCommerce.Domain.Customer.Services;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace ContactExtModule.Web
{
    public class Contact2Service : CustomerMemberServiceImpl
    {
        public Contact2Service(Func<ICustomerRepository> repositoryFactory, IDynamicPropertyService dynamicPropertyService, ISecurityService securityService, IMemberFactory memberFactory)
            :base(repositoryFactory, dynamicPropertyService, securityService, memberFactory)
        {
        }

        protected override MemberDataEntity TryCreateDataMember(Member member)
        {
            MemberDataEntity retVal = null;
            var contact2 = member as Contact2;
            if(contact2 != null)
            {
                retVal = new Contact2Entity();
            }
            return retVal;
        }

        protected override Expression<Func<MemberDataEntity, bool>> GetQueryPredicate(MembersSearchCriteria criteria)
        {
            var retVal = base.GetQueryPredicate(criteria);
            if (criteria.Keyword != null)
            {
                var predicate = PredicateBuilder.False<MemberDataEntity>();
                predicate = predicate.Or(x => x is Contact2Entity && (x as Contact2Entity).JobTitle.Contains(criteria.Keyword));
                retVal = retVal.Or(LinqKit.Extensions.Expand(predicate));
            }           
            return retVal;
        }


    }
}