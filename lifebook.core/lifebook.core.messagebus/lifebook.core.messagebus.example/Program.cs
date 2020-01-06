using System;
using System.Reflection;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.messagebus.Interfaces;
using lifebook.core.messagebus.ioc;
using lifebook.core.messagebus.Services;
using lifebook.core.services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace lifebook.core.messagebus.example
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.Instance(typeof(ServiceInstaller).Assembly));
            container.Install(FromAssembly.Instance(typeof(RabbitMqInstaller).Assembly));
            var broker = container.Resolve<IMessageBroker>();
            broker.Connect();
            var info = new Models.MessageQueueInformation()
            {
                ExchangeName = "rabbitmq_example",
                QueueName = "rabbitmq_example",
                RoutingKey = "rabbitmq_example"
            };
            var bus = broker.TryConnectingDirectlyToQueue(info);
            bus.Publish("Testing 123");
            bus = broker.TryConnectingDirectlyToQueue(info);
            bus.Subscribe<string>(info, act =>
            {
                Console.WriteLine(act);
                Console.WriteLine(JObject.FromObject(act).ToString());
            });

            Console.WriteLine("waiting...");
            Console.ReadKey();
        }
    }

}
