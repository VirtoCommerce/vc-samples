# VirtoCommerce Samples

This repository is the official storage for VirtoCommerce samples source code. Each folder represents an individual sample.

## Current samples

* CustomerReviews - an example of a custom module with best coding practices, recommended code structure and styleguides applied. Includes DAL, web API and custom Platform web UI  

* IntegrationOrders - sample [Microsoft Logic Apps](https://azure.microsoft.com/en-us/services/logic-apps/) application to show how to integrate VirtoCommerce platform with external services and how to handle Logic Apps exeptions

* ManagedModule - source code for "Sample Managed Module" in [Creating new module](https://virtocommerce.com/docs/vc2devguide/working-with-platform-manager/extending-functionality/creating-new-module) tutorial

* MemberExtensionSampleModule - source code for [Extending Members domain types](https://virtocommerce.com/docs/vc2devguide/extending-commerce/extending-members-domain-types) tutorial

* OrderModule2 - sample project demonstrated how to extend  API, UI and Domain types in [Order module](https://github.com/VirtoCommerce/vc-module-order)

* ProgressiveWebAppPrototype - sample PWA application to show Service Worker and IndexedDB API features as well as offline mode

* UnmanagedModule - source code for "Sample Unmanaged Module" in [Creating new module](https://virtocommerce.com/docs/vc2devguide/working-with-platform-manager/extending-functionality/creating-new-module) tutorial

* VcSapIntegration - [Microsoft Logic Apps](https://azure.microsoft.com/en-us/services/logic-apps/) application sample to show how to integrate VirtoCommerce platform with SAP. In this Integration solution example:

    1. Receives new orders from VC
    1. For each order creates 2 objects that will be sent to SAP: Sales_item_in and SALES_PARTNERS
    1. Attributes of the Sales_item_in object are copied from the attributes of the corresponding order
    1. PartnerRole attribute of the SALES_PARTNERS object set depending on the value of the isPrototype attribute of the corresponding order.
    1. Created Sales_item_in and SALES_PARTNERS objects are sent to the SAP API

* EnrichmentFormSample - [Sample UI module which allow you edit dynamic address properties using Google Map API](/EnrichmentFormSample/Readme.md)

* External.PricingModule - an example of expansion is the price module. The example added an additional field "BasePrice" for the entity PriceEntity. The repository has also been updated and the migration has been added. This sample was generated using "Virto Commerce 2.x Pricing Module extension" template in Visual Studio. The template can be obtained with [Virto Commerce 2.x Module Templates](https://marketplace.visualstudio.com/items?itemName=Virto-Commerce.VirtoCommerceModuleTemplates) extension

* Sample sending Order change notifications to WhatsApp via Twilio or Woowa gateways

## License

Copyright (c) Virto Solutions LTD.  All rights reserved.

Licensed under the Virto Commerce Open Software License (the "License"); you
may not use this file except in compliance with the License. You may
obtain a copy of the License at

http://virtocommerce.com/opensourcelicense

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
implied.
