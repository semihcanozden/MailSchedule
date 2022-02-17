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
        public void Send(MailModel mailModel)
        {
            var _email = configuration.GetSection("Mail:email").Value;
            var _pass = configuration.GetSection("Mail:pass").Value;
            var _host = configuration.GetSection("Mail:host").Value;
            var _port = configuration.GetSection("Mail:port").Value;
            var _enableSSL = configuration.GetSection("Mail:enableSSL").Value;


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
            client = new SmtpClient
            {
                Host = _host,
                Port = Convert.ToInt32(_port),
                EnableSsl = bool.Parse(_enableSSL),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_email, _pass)
            };
            
            msg = new MailMessage { From = new MailAddress(_email) };
            msg.To.Add(mailModel.To);
            msg.Subject = mailModel.Subject;
            msg.Body = mailModel.Body;
            if (mailModel.Attachments.Count>0)
            {
                foreach (var item in mailModel.Attachments)
                {
                    msg.Attachments.Add(new Attachment(item));
                }
            }
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = mailModel.IsBodyHtml;
            msg.Priority = MailPriority.Normal;
            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(msg);
        }
    }
}