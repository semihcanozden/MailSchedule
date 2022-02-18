using Microsoft.Extensions.Hosting;
using RabbitMQ_Class;
using System.Threading;
using System.Threading.Tasks;
using MailSend_Class;

namespace MailSchedule
{
  public class Worker : BackgroundService
  {
    IConsumer _consumer;
    IPublisher _publisher;
    IMailSend _mailSend;
    public Worker(IConsumer consumer, IPublisher publisher, IMailSend mailSend)
    {
      _publisher = publisher;
      _consumer = consumer;
      _mailSend = mailSend;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      //_consumer.MailQueueEvents += _consumer_MailQueueEvents;
      var mailModel = new MailModel();
      mailModel.To.Add("semih34_can55@hotmail.com");
      mailModel.Body = message;
      _mailSend.Send(mailModel);

      while (!stoppingToken.IsCancellationRequested)
      {
        //_publisher.SendLogQueue(new LogQueueModel() { Date = System.DateTime.Now, Log = "Abc" });
        //var mailQueue = new MailQueueModel();
        //mailQueue.To.Add("semih34_can55@hotmail.com");
        //mailQueue.Subject = "Elma";
        //mailQueue.Body = "mesaj";
        //mailQueue.Cc.Add("elma.com");
        //mailQueue.Cc.Add("armut.com");
        //_publisher.SendMailQueue(mailQueue);
        //_consumer.QueueSendMessage();
        //_mailSend.Send(new MailModel() { To="semih34_can55@hotmail.com",Subject="test",Body="deneme"});
        System.Console.WriteLine("Mail Scheduler Working..");
        await Task.Delay(60000, stoppingToken);
      }
    }
    
    private void _consumer_MailQueueEvents(object sender, MailQueueModel e)
    {
      ulong tag = (ulong)sender;

      _mailSend.Send(new MailModel()
      {
        To = e.To,
        Body = e.Body,
        Subject = e.Subject
      });
      _consumer.DeleteMail(tag);
    }


    private string message = @"<!DOCTYPE html>
<html lang='en' xmlns='http://www.w3.org/1999/xhtml'xmlns:o='urn:schemas-microsoft-com:office:office'>
<head>
    <meta charset = 'UTF-8' >
    <meta name='viewport' content='width=device-width,initial-scale=1'>
    <meta name = 'x-apple-disable-message-reformatting'>
    <title></title >
    <!--[if mso]>
    <noscript >
        <xml >
            <o:OfficeDocumentSettings>
                <o:PixelsPerInch>96</o:PixelsPerInch>
            </o:OfficeDocumentSettings>
        </xml>
    </noscript>
    <![endif]-->
    <style>
        table, td, div, h1, p {font-family: Arial, sans-serif;}
  table, td {border:2px solid #000000 !important;}
    </style>
</head>
<body style='margin:0;padding:0;'>
    <table role ='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;background:#ffffff;'>
        <tr>
            <td align='center' style='padding:0;'>
                Hello!
            </td>
        </tr>
    </table>
</body>
</html>";
  }
}