namespace RabbitMQ_Class
{
    public interface IPublisher
    {
        void getMessage(string MailAdress, string Password, string To, string Subject, string MessageBody);
    }
}