using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace RabbitMQ_Class
{
  public class Publisher : IPublisher
  {
    private IConfiguration _configuration;
    public Publisher(IConfiguration configuration)
    {
      this._configuration = configuration;
    }
    private void SendQueue(string queue, object model)
    {
      var hostName = _configuration.GetSection("RabbitMQ:hostName").Value;
      var userName = _configuration.GetSection("RabbitMQ:userName").Value;
      var password = _configuration.GetSection("RabbitMQ:password").Value;
      var factory = new ConnectionFactory
      {
        UserName = userName,
        Password =password,
        VirtualHost = "/" + queue,
        Port = AmqpTcpEndpoint.UseDefaultPort,
        HostName = hostName
      };

      using (var connection = factory.CreateConnection())
      {
        using (var channel = connection.CreateModel())
        {
          channel.QueueDeclare(
          queue: queue,
          durable: false,
          exclusive: false,
          autoDelete: false,
          arguments: null);

          var jsonMessage = JsonConvert.SerializeObject(model);
          var encodedMessage = Encoding.UTF8.GetBytes(jsonMessage);
          channel.BasicPublish("", queue, null, encodedMessage);
        }
      }
    }
    public void SendLogQueue(LogQueueModel logQueueModel)
    {
      SendQueue("log", logQueueModel);
    }
    public void SendMailQueue(MailQueueModel mailQueueModel)
    {
      SendQueue("mail", mailQueueModel);
    }
  }
}