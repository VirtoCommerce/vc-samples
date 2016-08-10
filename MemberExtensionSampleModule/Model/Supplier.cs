using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Customer.Model;

namespace MemberExtensionSampleModule.Web.Model
{
    public class Supplier : Member
    {
        public Supplier()
        {
            Reviews = new List<SupplierReview>();
        }

        public string ContractNumber { get; set; }
        public ICollection<SupplierReview> Reviews { get; set; }
    }
}