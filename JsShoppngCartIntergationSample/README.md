# VC Javascript shopping cart intergation sample
It is sample ASP NET MVC app with integrated [JS shopping cart module](https://github.com/VirtoCommerce/vc-module-javascript-shoppingcart) functionality.

Sample allows to add items to the cart using "Buy" buttons and go through whole checkout process after pressing "Checkout".

## Preview
![Main page](/JsShoppngCartIntergationSample/docs/media/main-page.png)

![Cart](/JsShoppngCartIntergationSample/docs/media/cart-popup.png)

![Checkout](/JsShoppngCartIntergationSample/docs/media/checkout-popup.png)

## Notes:
- To use vc-shopping-cart scripts you need to add its references, like [here](https://github.com/VirtoCommerce/vc-samples/blob/master/JsShoppngCartIntergationSample/VirtoCommerce.JavaScriptShoppingCart.IntegrationSample/Views/Shared/_Layout.cshtml#L19-L28). [angular-credit-cards-3.0.1.js](https://github.com/VirtoCommerce/vc-samples/blob/master/JsShoppngCartIntergationSample/VirtoCommerce.JavaScriptShoppingCart.IntegrationSample/Views/Shared/_Layout.cshtml#L27) was added to the project sources. Others are linked from CDN.
- Need to add [vc-cart](https://github.com/VirtoCommerce/vc-samples/blob/master/JsShoppngCartIntergationSample/VirtoCommerce.JavaScriptShoppingCart.IntegrationSample/Views/Shared/_Layout.cshtml#L40) component to the page.
- [Buy](https://github.com/VirtoCommerce/vc-samples/blob/master/JsShoppngCartIntergationSample/VirtoCommerce.JavaScriptShoppingCart.IntegrationSample/Views/Home/Index.cshtml#L8) button example.
- Platform url and API key are configured in [Web.Config](https://github.com/VirtoCommerce/vc-samples/blob/master/JsShoppngCartIntergationSample/VirtoCommerce.JavaScriptShoppingCart.IntegrationSample/Web.config#L12-L13). Please make sure you have trailing '/' in the platform url.