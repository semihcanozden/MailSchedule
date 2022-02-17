using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace RabbitMQ_Class
{
    public class Publisher : IPublisher
    {
        public void getMessage(MailQueueModel mailQueueModel)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                    queue: "letterbox",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                    //string message = mailQueueModel.Email + " " + mailQueueModel.Password + " " + mailQueueModel.To + " " + mailQueueModel.Subject + " " + mailQueueModel.Body;
                    var jsonMessage = JsonConvert.SerializeObject(mailQueueModel);
                    var encodedMessage = Encoding.UTF8.GetBytes(jsonMessage);
                    channel.BasicPublish("", "letterbox", null, encodedMessage);
                }                
            }            
        }
    }
}