using System;

namespace RabbitMQ_Class
{
    public interface IConsumer
    {
        public event EventHandler<MailQueueModel> messageEvents;
        void ReciveMessages();
        void Delete(ulong deliveryTag);
    }
}