using RabbitMQ.Client;
using System;
using RabbitMQ.Client.Events;
using MailSend_Class;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace RabbitMQ_Class
{
    public class Consumer : IConsumer
    {
        public event EventHandler<MailQueueModel> messageEvents;
        IModel channel;

        private readonly IConfiguration configuration;
        public Consumer(IConfiguration config)
        {
            this.configuration = config;
        }


        public void ReciveMessages()
        {            
            var hostName = configuration.GetSection("RabbitMQ:hostName").Value;
            var queueName = configuration.GetSection("RabbitMQ:queueName").Value;

            var factory = new ConnectionFactory { HostName = hostName };

            IConnection connection = factory.CreateConnection();

            channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);


            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                var messageBody = JsonConvert.DeserializeObject<MailQueueModel>(message);
                messageEvents?.Invoke(ea.DeliveryTag,messageBody);
            };
        }

        public void Delete(ulong deliveryTag)
        {
            channel.BasicAck(deliveryTag, false);
        }
    }
}