using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Cart.Model;

namespace CartModule2.Model
{
    public class LineItem2 : LineItem
    {
        public string OuterId { get; set; }
    }
}