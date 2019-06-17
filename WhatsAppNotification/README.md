# WhatsApp notification example
An example of adding WhatsApp notification gateway.

https://virtocommerce.com/docs/vc2devguide/working-with-platform-manager/basic-functions/working-with-notifications

## Twilio
https://www.twilio.com/docs/sms/whatsapp/quickstart/csharp

## WooWa
http://api.woo-wa.com/

To include WooWa gateway, replace Module.cs

```C#
_container.RegisterType<IWhatsAppClient, TwilioWhatsApp>();
```

by
```C#
_container.RegisterType<IWhatsAppClient, WooWaClient>();
```