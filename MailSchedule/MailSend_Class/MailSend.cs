using System.Net;
using System.Net.Mail;
using System.Text;

namespace MailSend_Class
{
    public class MailSend : IMailSend
    {

        NetworkCredential login;
        SmtpClient client;
        MailMessage msg;
        public void Send(string email, string pass, string to, string subject, string messagebody)
        {
            var getSmtp = email.Split("@");
            var smtp = "";
            if (getSmtp[1] == "gmail.com")
            {
                smtp = "smtp.gmail.com";
            }
            else if (getSmtp[1] == "hotmail.com")
            {
                smtp = "smtp-mail.outlook.com";
            }
            else if (getSmtp[1] == "yahoo.com")
            {
                smtp = "smtp.mail.yahoo.com";
            }
            login = new NetworkCredential(email, pass);
            client = new SmtpClient(smtp);
            client.Port = 587;
            client.EnableSsl = true;
            client.Credentials = login;
            msg = new MailMessage { From = new MailAddress(email) };
            msg.To.Add(to);
            msg.Subject = subject;
            msg.Body = messagebody;
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.Normal;
            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            string userstate = "Sending...";
            client.SendAsync(msg, userstate);
        }
    }
}