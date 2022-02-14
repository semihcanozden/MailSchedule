namespace MailSend_Class
{
    public interface IMailSend
    {
        void Send(string email, string pass, string to, string subject, string messagebody);
    }
}
