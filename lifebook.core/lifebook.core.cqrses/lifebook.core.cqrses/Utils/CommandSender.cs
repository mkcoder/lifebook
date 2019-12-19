using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using Consul;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace lifebook.core.cqrses.Utils
{
    class CommandSender
    {
        private IConsulClient _client = new ConsulClient();
        private HttpClient _webClient = new HttpClient();

        private Guid CommandId = Guid.NewGuid();
        public string CommandName { get; internal set; }
        public string ServiceName { get; internal set; }
        public string InstanceName { get; internal set; }
        public JObject Data { get; internal set; }
        public Guid AggregateId { get; internal set; }
        public Action<JObject> Override { get; internal set; }
        public string AggregateType { get; internal set; }

        internal async Task<T> Send<T>()
        {
            var services = (await _client.Agent.Services()).Response;
            if(services.ContainsKey($"{ServiceName}_{InstanceName}"))
            {
                Data["CommandId"] = CommandId;
                Data["AggregateType"] = AggregateType;
                Data["AggregateId"] = AggregateId;
                Data["CommandName"] = CommandName;
                Override?.Invoke(Data);

                string json = JsonConvert.SerializeObject(Data, Formatting.Indented);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var service = services[$"{ServiceName}_{InstanceName}"];
                var response = await _webClient.PostAsync($"http://{service.Address}:{service.Port}/{CommandName}", httpContent);
                var responseContent = await response.Content.ReadAsStringAsync();
                return JObject.Parse(responseContent).ToObject<T>();
            }
            throw new NotImplementedException();
        }
    }

    public class CommandSenderSyntax
    {
        private CommandSender _commandSender = null;

        public CommandSenderSyntax()
        {

        }

        private CommandSenderSyntax(CommandSender commandSender)
        {
            _commandSender = commandSender;
        }

        public static CommandSenderSyntax WithCommandName(string name)
        {
            return new CommandSenderSyntax(new CommandSender() { CommandName = name });
        }

        public CommandSenderSyntax WithAggregateId(string aggregateType, Guid aggregateId)
        {
            _commandSender.AggregateType = aggregateType;
            _commandSender.AggregateId = aggregateId;
            return this;
        }

        public CommandSenderSyntax ToService(string serviceName)
        {
            _commandSender.ServiceName = serviceName;
            return this;
        }

        public CommandSenderSyntax ToInstance(string instanceName)
        {
            _commandSender.InstanceName = instanceName;
            return this;
        }

        public CommandSenderSyntax WithCommandData(JObject data)
        {
            _commandSender.Data = data;
            return this;
        }

        public CommandSenderSyntax OverrideData(Action<JObject> data)
        {
            _commandSender.Override = data;
            return this;
        }

        public async Task<T> Send<T>()
        {
            return await _commandSender.Send<T>();
        }
    }
}
