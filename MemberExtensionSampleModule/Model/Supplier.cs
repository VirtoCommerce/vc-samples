using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Customer.Model;

namespace MemberExtensionSampleModule.Web.Model
{
    public class Supplier : Member
    {
        public string ContractNumber { get; set; }
    }
}