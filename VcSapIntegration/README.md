# VC SAP integration solution
Microsoft Logic Apps application to show how to integrate VirtoCommerce platform with SAP. This integration solution:

  1. Receives new orders from VC
  2. Creates 2 objects for sending them to SAP for each order: Sales_item_in and SALES_PARTNERS
  3. Attributes of the Sales_item_in are filled from the attributes of the corresponding order
  4. PartnerRole attribute of the SALES_PARTNERS object set depending on the value of the isPrototype attribute of the corresponding order
  5. Created Sales_item_in and SALES_PARTNERS objects are sent to the SAP API
