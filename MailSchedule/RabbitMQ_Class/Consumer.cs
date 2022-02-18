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
    public event EventHandler<MailQueueModel> MailQueueEvents;
    //public event EventHandler<LogQueueModel> LogQueueEvents;



    private IModel channelMail;
    private IModel channelLog;



    private readonly IConfiguration _configuration;
    public Consumer(IConfiguration config)
    {
      this._configuration = config;
    }

    event EventHandler<MailQueueModel> IConsumer.MailQueueEvents
    {
      add
      {
        var hostName = _configuration.GetSection("RabbitMQ:hostName").Value;
        var userName = _configuration.GetSection("RabbitMQ:userName").Value;
        var password = _configuration.GetSection("RabbitMQ:password").Value;

        var factory = new ConnectionFactory
        {
          UserName = userName,
          Password = password,
          VirtualHost = "/mail",
          Port = AmqpTcpEndpoint.UseDefaultPort,
          HostName = hostName
        };

        var connection = factory.CreateConnection();

        channelMail = connection.CreateModel();

        channelMail.QueueDeclare(
            queue: "mail",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(channelMail);
        channelMail.BasicConsume(queue: "mail", autoAck: false, consumer: consumer);


        consumer.Received += (model, ea) =>
        {
          var body = ea.Body.ToArray();
          string message = Encoding.UTF8.GetString(body);
          var messageBody = JsonConvert.DeserializeObject<MailQueueModel>(message);
          value?.Invoke(ea.DeliveryTag, messageBody);
        };
      }

      remove
      {
        throw new NotImplementedException();
      }
    }

    event EventHandler<LogQueueModel> IConsumer.LogQueueEvents
    {
      add
      {
        var hostName = _configuration.GetSection("RabbitMQ:hostName").Value;
        var userName = _configuration.GetSection("RabbitMQ:userName").Value;
        var password = _configuration.GetSection("RabbitMQ:password").Value;

        var factory = new ConnectionFactory
        {
          UserName = userName,
          Password = password,
          VirtualHost = "/log",
          Port = AmqpTcpEndpoint.UseDefaultPort,
          HostName = hostName
        };

        var connection = factory.CreateConnection();

        channelLog = connection.CreateModel();

        channelLog.QueueDeclare(
            queue: "log",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(channelLog);
        channelLog.BasicConsume(queue: "log", autoAck: false, consumer: consumer);


        consumer.Received += (model, ea) =>
        {
          var body = ea.Body.ToArray();
          string message = Encoding.UTF8.GetString(body);
          var messageBody = JsonConvert.DeserializeObject<LogQueueModel>(message);
          value?.Invoke(ea.DeliveryTag, messageBody);
        };
      }

      remove
      {
        throw new NotImplementedException();
      }
    }

    public void DeleteMail(ulong deliveryTag)
    {
      channelMail.BasicAck(deliveryTag, false);
    }
    public void DeleteLog(ulong deliveryTag)
    {
      channelLog.BasicAck(deliveryTag, false);
    }
  }
}