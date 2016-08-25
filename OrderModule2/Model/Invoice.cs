using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Order.Model;

namespace OrderModule2.Model
{
    public class Invoice : OrderOperation
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }

    }
}