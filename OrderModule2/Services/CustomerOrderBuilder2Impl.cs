using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.OrderModule.Data.Services;

namespace OrderModule2.Web.Services
{
    /// <summary>
    /// Override the original CustomerOrderBuilderImpl to copy extended fields which added in this module
    /// </summary>
    public class CustomerOrderBuilder2Impl : CustomerOrderBuilderImpl
    {
        public CustomerOrderBuilder2Impl(ICustomerOrderService customerOrderService, IStoreService storeService) : base(customerOrderService, storeService)
        {
        }

        protected override VirtoCommerce.Domain.Order.Model.LineItem ToOrderModel(VirtoCommerce.Domain.Cart.Model.LineItem lineItem)
        {
            var result = base.ToOrderModel(lineItem) as Model.LineItem2;
           
            //Next lines just copy OuterId from cart LineItem2 to order LineItem2
            var cartLineItem2 = lineItem as CartModule2.Model.LineItem2;
            if(cartLineItem2 != null)
            {
                result.OuterId = cartLineItem2.OuterId;
            }
            return result;
        }
    }
}