using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Platform.Core.Common;

namespace CartModule2.Model
{
    public class Cart2Entity : ShoppingCartEntity
    {
        [StringLength(64)]
        public string CartType { get; set; }

        public override ShoppingCart ToModel(ShoppingCart cart)
        {
            var result = base.ToModel(cart);

            var cart2 = result as Cart2;
            cart2.CartType = EnumUtility.SafeParse<Cart2Type>(this.CartType, Cart2Type.Regular);

            return cart2;
        }

        public override ShoppingCartEntity FromModel(ShoppingCart cart, PrimaryKeyResolvingMap pkMap)
        {
            base.FromModel(cart, pkMap);

            var cart2 = cart as Cart2;
            this.CartType = cart2.CartType.ToString();

            return this;
        }

        public override void Patch(ShoppingCartEntity target)
        {
            base.Patch(target);

            var cart2entity = target as Cart2Entity;
            cart2entity.CartType = this.CartType;
        }
    }
}