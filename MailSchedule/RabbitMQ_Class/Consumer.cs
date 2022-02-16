using System.Text;
using System.Net.Mail;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net;
using MailSend_Class;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RabbitMQ_Class
{
    public class Consumer : IConsumer
    {
        private readonly IConfiguration configuration;
        public Consumer(IConfiguration config)
        {
            this.configuration = config;
        }
        public void QueueSendMessage()
        {
            var hostName = configuration.GetSection("RabbitMQ:hostName").Value;
            var queueName = configuration.GetSection("RabbitMQ:queueName").Value;

            var factory = new ConnectionFactory { HostName = hostName };

            IConnection connection = factory.CreateConnection();

            IModel channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            NetworkCredential login;
            SmtpClient client;
            MailMessage msg;

            ulong previousTag = 1;
            ulong newTag;

            var isGone = false;
            var isSend = false;

            while (isGone == false)
            {
                consumer.Received += (model, ea) =>
                {

                    if (previousTag == ea.DeliveryTag)
                    {
                        newTag = previousTag;
                        var body = ea.Body.ToArray();
                        string[] message = Encoding.UTF8.GetString(body).Split(" ");
                        var email = message[0];
                        var pass = message[1];
                        var to = message[2];
                        var subject = message[3];
                        var messagebody = message[4];

                        if (isSend == false)
                        {
                            //MailSend ms = new MailSend();
                            //ms.Send(email, pass, to, subject, messagebody);
                            isSend = true;
                        }
                        channel.BasicAck(newTag, false);
                        previousTag = ea.DeliveryTag;
                        isGone = true;
                    }
                };
            }
        }
    }
}