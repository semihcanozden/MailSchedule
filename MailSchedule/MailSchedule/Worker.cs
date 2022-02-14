using Microsoft.Extensions.Hosting;
using RabbitMQ_Class;
using System.Threading;
using System.Threading.Tasks;

namespace MailSchedule
{
    public class Worker : BackgroundService
    {
        IConsumer _consumer;
        IPublisher _publisher;
        public Worker(IConsumer consumer, IPublisher publisher)
        {
            _publisher = publisher;
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _publisher.getMessage("can.oz.den.semih@gmail.com", "1903semih2002", "semih34_can55@hotmail.com", "Test", "Testİçerik");
                _consumer.QueueSendMessage();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
