# VC Javascript shopping cart intergation sample
It is sample ASP NET MVC app with integrated [JS shopping cart module](https://github.com/VirtoCommerce/vc-module-javascript-shoppingcart) functionality.

Sample allows to add items to the cart using "Buy" buttons and go through whole checkout process after pressing "Checkout".

![SampleView](\docs\media\sample-integration-view.png)

Notes:
- To use vc-shopping-cart scripts you need to add its references, like [here](https://github.com/VirtoCommerce/vc-samples/blob/js-shopping-cart-integration-sample/JsShoppngCartIntergationSample/VirtoCommerce.JavaScriptShoppingCart.IntegrationSample/Views/Shared/_Layout.cshtml#L10-L21). [angular-credit-cards-3.0.1.js](https://github.com/VirtoCommerce/vc-samples/blob/js-shopping-cart-integration-sample/JsShoppngCartIntergationSample/VirtoCommerce.JavaScriptShoppingCart.IntegrationSample/Views/Shared/_Layout.cshtml#L18) was added to the project sources. others are linked from CDN.
- Need to add [vc-cart](https://github.com/VirtoCommerce/vc-samples/blob/js-shopping-cart-integration-sample/JsShoppngCartIntergationSample/VirtoCommerce.JavaScriptShoppingCart.IntegrationSample/Views/Shared/_Layout.cshtml#L24) component on the page.
- [Buy](https://github.com/VirtoCommerce/vc-samples/blob/js-shopping-cart-integration-sample/JsShoppngCartIntergationSample/VirtoCommerce.JavaScriptShoppingCart.IntegrationSample/Views/Home/Index.cshtml#L13) and [Checkout](https://github.com/VirtoCommerce/vc-samples/blob/js-shopping-cart-integration-sample/JsShoppngCartIntergationSample/VirtoCommerce.JavaScriptShoppingCart.IntegrationSample/Views/Home/Index.cshtml#L18) buttons examples.