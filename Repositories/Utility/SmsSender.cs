using Twilio;
using Twilio.Rest.Api.V2010.Account;

public class SmsSender:ISmsSender
{
    // Twilio credentials
    private string accountSid = "AC10614d702fdee437f6f1a8b43d650a8a";
    private string authToken = "1d45f137ba8c4bc9c63ed4f882297f16";
    private string fromPhoneNumber = "+18046217056";

    public void SendSms(string toPhoneNumber, string message)
    {
        TwilioClient.Init(accountSid, authToken);

        var twilioMessage = MessageResource.Create(
            body: message,
            from: new Twilio.Types.PhoneNumber(fromPhoneNumber),
            to: new Twilio.Types.PhoneNumber(toPhoneNumber)
        );

        Console.WriteLine($"SMS sent with SID: {twilioMessage.Sid}");
    }
}
