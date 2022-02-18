namespace RabbitMQ_Class
{
  public interface IPublisher
  {
    void SendLogQueue(LogQueueModel logQueueModel);
    void SendMailQueue(MailQueueModel mailQueueModel);
  }
}