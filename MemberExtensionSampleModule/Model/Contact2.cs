using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Customer.Model;

namespace MemberExtensionSampleModule.Web.Model
{
    public class Contact2 : Contact
    {
        public Contact2()
        {
            base.MemberType = typeof(Contact).Name;
        }
        public string JobTitle { get; set; }
    }
}