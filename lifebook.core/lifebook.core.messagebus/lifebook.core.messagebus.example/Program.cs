using System;
using System.Reflection;
using System.Text.Json.Serialization;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.messagebus.Interfaces;
using lifebook.core.messagebus.ioc;
using lifebook.core.messagebus.Services;
using lifebook.core.services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

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
            bus.Subscribe<PersonCreated>(info, act =>
            {
                Console.WriteLine(act);
                Console.WriteLine(JObject.FromObject(act).ToString());
            });

            bus = broker.TryConnectingDirectlyToQueue(info);


            while (Console.ReadLine() != "Exit")
            {
                bus.Publish(new PersonCreated() { Name = "John doe", Age = 12 });
            }


            Console.WriteLine("exit...");
        }
    }

    public class PersonCreated
    {
        [JsonIgnore]
        public Guid CorrelationId { get; set; } = Guid.Parse("fae85273-6a13-4634-8409-8e0dcfb0043e");
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
