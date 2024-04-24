public interface ISmsSender
{
    void SendSms(string toPhoneNumber, string message);
}