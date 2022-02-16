using System.Net;
using System;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Configuration;

namespace MailSend_Class
{
    public class MailSend : IMailSend
    {
        private readonly IConfiguration configuration;
        public MailSend(IConfiguration config)
        {
            this.configuration = config;
        }

        NetworkCredential login;
        SmtpClient client;
        MailMessage msg;
        //string email, string pass, string to, string subject, string messagebody
        public void Send()
        {
            var _email = configuration.GetSection("Mail:email").Value;
            var _pass = configuration.GetSection("Mail:pass").Value;
            var _to = configuration.GetSection("Mail:to").Value;
            var _subject = configuration.GetSection("Mail:subject").Value;
            var _message = configuration.GetSection("Mail:message").Value;

            Console.WriteLine(_email);

            //var getSmtp = email.Split("@");
            //var smtp = "";
            //if (getSmtp[1] == "gmail.com")
            //{
            //    smtp = "smtp.gmail.com";
            //}
            //else if (getSmtp[1] == "hotmail.com")
            //{
            //    smtp = "smtp-mail.outlook.com";
            //}
            //else if (getSmtp[1] == "yahoo.com")
            //{
            //    smtp = "smtp.mail.yahoo.com";
            //}
            login = new NetworkCredential(_email, _pass);
            client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.EnableSsl = true;
            client.Credentials = login;
            msg = new MailMessage { From = new MailAddress(_email) };
            msg.To.Add(_to);
            msg.Subject = _subject;
            msg.Body = _message;
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.Normal;
            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            string userstate = "Sending...";
            client.SendAsync(msg, userstate);
        }
    }
}