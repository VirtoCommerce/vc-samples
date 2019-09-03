# VC export orders to CSV solution
Microsoft Logic Apps application to show how to export orders from VirtoCommerce platform to CSV file and sand it to FTP. This integration solution:

  1. Receives new orders from VC
  2. Creates CSV file file named YYYY-MM-DD-MM-HH.csv with the format: OrderNumber, Sku, Quantity, Price, TotalPrice
  3. Send CSV file to FTP folder
  4. Update order status to **Processing**
