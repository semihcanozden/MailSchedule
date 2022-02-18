using System;

namespace RabbitMQ_Class
{
    public interface IConsumer
    {
    event EventHandler<MailQueueModel> MailQueueEvents;
    event EventHandler<LogQueueModel> LogQueueEvents;
    void DeleteMail(ulong deliveryTag);
    void DeleteLog(ulong deliveryTag);
    }
}