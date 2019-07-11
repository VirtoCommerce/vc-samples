# VC SAP Integration solution

In this Integration solution example:

    1. Receives new orders from VC
    1. For each order creates 2 objects that will be sent to SAP: Sales_item_in and SALES_PARTNERS
    1. Attributes of the Sales_item_in object are copied from the attributes of the corresponding order
    1. PartnerRole attribute of the SALES_PARTNERS object set depending on the value of the isPrototype attribute of the corresponding order.
    1. Created Sales_item_in and SALES_PARTNERS objects are sent to the SAP API
