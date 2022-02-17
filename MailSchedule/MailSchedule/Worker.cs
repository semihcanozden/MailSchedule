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
            _consumer.ReciveMessages();
            _consumer.messageEvents += _consumer_messageEvents;
            while (!stoppingToken.IsCancellationRequested)
            {
                //_publisher.getMessage(new MailQueueModel() { Email="can.oz.den.semih@gmail.com", Password="1903semih2002", To="semih34_can55@hotmail.com", Subject="test", Body="deneme"});
                //_consumer.QueueSendMessage();
                //_mailSend.Send(new MailModel() { To="semih34_can55@hotmail.com",Subject="test",Body="deneme"});
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void _consumer_messageEvents(object sender, MailQueueModel e)
        {
            ulong tag = (ulong)sender;
            _mailSend.Send(new MailModel()
            {
                To = e.To,
                Body = e.Body,
                Subject = e.Subject
            });
            _consumer.Delete(tag);
        }
    }
}