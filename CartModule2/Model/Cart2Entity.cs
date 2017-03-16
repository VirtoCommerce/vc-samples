using System.ComponentModel.DataAnnotations;
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

            var cart2 = (Cart2)result;
            cart2.CartType = EnumUtility.SafeParse(CartType, Cart2Type.Regular);

            return cart2;
        }

        public override ShoppingCartEntity FromModel(ShoppingCart cart, PrimaryKeyResolvingMap pkMap)
        {
            base.FromModel(cart, pkMap);

            var cart2 = (Cart2)cart;
            CartType = cart2.CartType.ToString();

            return this;
        }

        public override void Patch(ShoppingCartEntity target)
        {
            base.Patch(target);

            var cart2Entity = (Cart2Entity)target;
            cart2Entity.CartType = CartType;
        }
    }
}
