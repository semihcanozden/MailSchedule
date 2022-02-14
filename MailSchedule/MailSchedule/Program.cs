using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ_Class;
using MailSend_Class;

namespace MailSchedule
{
    public class Program
    {
        public static void Main(string[] args)
        {
            createHostBuilder(args).Build().Run();
            Console.WriteLine("Merhaba");
            Console.ReadLine();
        }

        public static IHostBuilder createHostBuilder(string[] args) =>

           Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
           {
               services.AddSingleton<IConsumer, Consumer>();
               services.AddSingleton<IPublisher, Publisher>();
               services.AddSingleton<IMailSend,MailSend>();
               services.AddHostedService<Worker>();
           });


    }
}