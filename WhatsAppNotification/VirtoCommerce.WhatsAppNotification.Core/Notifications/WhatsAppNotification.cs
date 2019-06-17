using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.WhatsAppNotification.Core.Gateway;

namespace VirtoCommerce.WhatsAppNotification.Core.Notifications
{
    public abstract class WhatsAppNotification : Notification
    {
        protected WhatsAppNotification(IWhatsAppNotificationSendingGateway notificationSendingGateway) 
            : base(notificationSendingGateway)
        {
        }
    }
}
