using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.WhatsAppNotification.Core.Notifications;

namespace VirtoCommerce.WhatsAppNotification.Data.Handlers
{
    public class SendNotificationOrderChangedEventHandler : IEventHandler<OrderChangedEvent>
    {
        private readonly INotificationManager _notificationManager;
        private readonly ISecurityService _securityService;
        private readonly IMemberService _memberService;

        public SendNotificationOrderChangedEventHandler(INotificationManager notificationManager, ISecurityService securityService, IMemberService memberService)
        {
            _notificationManager = notificationManager;
            _securityService = securityService;
            _memberService = memberService;
        }

        public async Task Handle(OrderChangedEvent message)
        {
            foreach (var changedEntry in message.ChangedEntries)
            {
                if (changedEntry.EntryState == EntryState.Modified || changedEntry.EntryState == EntryState.Added)
                {
                    var notification = _notificationManager.GetNewNotification<OrderWhatsAppNotification>();

                    var user = await _securityService.FindByIdAsync(changedEntry.OldEntry.CustomerId, UserDetails.Reduced);

                    var contact = user != null
                        ? _memberService.GetByIds(new[] { user.MemberId }).FirstOrDefault()
                        : _memberService.GetByIds(new[] { changedEntry.OldEntry.CustomerId }).FirstOrDefault();

                    notification.Recipient = contact?.Phones?.FirstOrDefault(x => !string.IsNullOrEmpty(x)) ?? user?.PhoneNumber;
                    notification.Order = changedEntry.NewEntry;

                    if (!string.IsNullOrEmpty(notification.Recipient))
                    {
                        _notificationManager.ScheduleSendNotification(notification);
                    }
                }
            }
        }
    }
}
