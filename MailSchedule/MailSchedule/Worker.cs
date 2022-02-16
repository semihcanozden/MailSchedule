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
            while (!stoppingToken.IsCancellationRequested)
            {
                //_publisher.getMessage("can.oz.den.semih@gmail.com", "1903semih2002", "semih34_can55@hotmail.com", "Test", "Testİçerik");
                //_consumer.QueueSendMessage();
                _mailSend.Send();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}