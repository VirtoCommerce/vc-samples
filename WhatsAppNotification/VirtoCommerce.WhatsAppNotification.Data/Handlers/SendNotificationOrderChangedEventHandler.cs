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
                    var user = await _securityService.FindByIdAsync(changedEntry.OldEntry.CustomerId, UserDetails.Reduced);

                    var contact = user != null
                        ? _memberService.GetByIds(new[] { user.MemberId }).FirstOrDefault()
                        : _memberService.GetByIds(new[] { changedEntry.OldEntry.CustomerId }).FirstOrDefault();

                    var phoneNumber = contact?.Phones?.FirstOrDefault(x => !string.IsNullOrEmpty(x)) ?? user?.PhoneNumber;

                    if (!string.IsNullOrEmpty(phoneNumber))
                    {
                        var notification = _notificationManager.GetNewNotification<OrderWhatsAppNotification>();

                        notification.Recipient = phoneNumber;
                        notification.Order = changedEntry.NewEntry;

                        _notificationManager.ScheduleSendNotification(notification);
                    }
                }
            }
        }
    }
}
