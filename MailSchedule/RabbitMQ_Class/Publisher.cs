using System.Text;
using RabbitMQ.Client;

namespace RabbitMQ_Class
{
    public class Publisher : IPublisher
    {
        public void getMessage(string MailAdress, string Password, string To, string Subject, string MessageBody)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            IConnection connection = factory.CreateConnection();

            IModel channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "letterbox",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);


            string message = MailAdress + " " + Password + " " + To + " " + Subject + " " + MessageBody;
            var encodedMessage = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("", "letterbox", null, encodedMessage);

        }
    }
}