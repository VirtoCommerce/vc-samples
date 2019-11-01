# VC export orders to XML solution
Microsoft Logic Apps application to show how to export orders from VirtoCommerce platform to XML file and sand it to FTP. This integration solution:

  1. Receives orders with **Awaiting Shipping** status from VC
  2. Creates XML file file named order-number.xml
  3. Send XML file to FTP folder
  4. Update order status to **Exported and Awaiting Shipping**
