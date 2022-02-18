using System.Net;
using System;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Configuration;
using System.Linq;

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
    public void Send(MailModel mailModel)
    {
      var _email = configuration.GetSection("Mail:email").Value;
      var _pass = configuration.GetSection("Mail:pass").Value;
      var _host = configuration.GetSection("Mail:host").Value;
      var _port = configuration.GetSection("Mail:port").Value;
      var _enableSSL = configuration.GetSection("Mail:enableSSL").Value;

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

      foreach (var to in mailModel.To)
      {
        msg.To.Add(to);
      }

      msg.Subject = mailModel.Subject;
      msg.Body = mailModel.Body;

      foreach (var item in mailModel.Attachments)
      {
        msg.Attachments.Add(new Attachment(item));
      }
      foreach (var cc in mailModel.Cc)
      {
        msg.CC.Add(cc);
      }
      foreach (var bcc in mailModel.Bcc)
      {
        msg.Bcc.Add(bcc);
      }

      msg.BodyEncoding = Encoding.UTF8;
      msg.IsBodyHtml = mailModel.IsBodyHtml;
      msg.Priority = MailPriority.Normal;
      msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
      client.Send(msg);
    }

  }
}