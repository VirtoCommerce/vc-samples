# VcEventbusModuleSample
Microsoft Logic Apps application to show how to subscribe to Event Grid Topic. This integration solution:

  1. Receives events Event Grid Topic
  1. Get from event body changed order Id
  1. Get order by Id from Virtocommerce
  1. Checks order status, if Paid - send approval email
  1. Wait for email response
  1. If response Approve set order status to **Processing**
  1. If response Reject set order status to **Cancelled**
  1. Update order in Virtocommerce
