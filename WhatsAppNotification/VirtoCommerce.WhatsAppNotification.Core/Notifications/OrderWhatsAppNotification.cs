using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.WhatsAppNotification.Core.Gateways;

namespace VirtoCommerce.WhatsAppNotification.Core.Notifications
{
    public class OrderWhatsAppNotification : WhatsAppNotification
    {
        public OrderWhatsAppNotification(IWhatsAppNotificationSendingGateway notificationSendingGateway)
            : base(notificationSendingGateway)
        {
            DisplayName = "WhatsApp order created & changed notification";
        }

        [NotificationParameter("Customer Order")]
        public CustomerOrder Order { get; set; }
    }
}
