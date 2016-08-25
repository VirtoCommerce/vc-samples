using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Order.Model;

namespace OrderModule2.Model
{
    public class CustomerOrder2 : CustomerOrder
    {
        public CustomerOrder2()
        {
            Invoices = new List<Invoice>();
        }
        public string NewField { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
    }
}