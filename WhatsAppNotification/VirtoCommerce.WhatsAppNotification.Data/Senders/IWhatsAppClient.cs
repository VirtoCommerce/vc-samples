namespace VirtoCommerce.WhatsAppNotification.Data.Senders
{
    public interface IWhatsAppClient
    {
        void SendMessage(string recipient, string body);
    }
}
