using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Cart.Model;

namespace CartModule2.Model
{
    public class Cart2 : ShoppingCart
    {
        public Cart2Type CartType { get; set; }
    }
}