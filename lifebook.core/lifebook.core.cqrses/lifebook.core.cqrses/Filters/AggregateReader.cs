using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;
using lifebook.core.services.interfaces;

namespace lifebook.core.cqrses.Filters
{
    internal class AggregateReader
    {
        private IEventReader _eventReader;
        private IConfiguration _configuration;
        private Command _command;
        private EventHandlersAttribute _eventHandlersAttribute;
        private readonly StreamCategorySpecifier _category;
        private Dictionary<string, MethodInfo> _map = new Dictionary<string, MethodInfo>();

        public AggregateReader(IEventReader eventReader, IConfiguration configuration, Command command, EventHandlersAttribute eventHandlersAttribute)
        {
            _eventReader = eventReader;
            _configuration = configuration;
            _command = command;
            _eventHandlersAttribute = eventHandlersAttribute;
            _category = new StreamCategorySpecifier(_configuration["ServiceName"], _configuration["ServiceInstance"], command.AggregateType, command.AggregateId);
        }

        public async Task<Aggregate> GetAggregate()
        {
            dynamic eventhandlers = (dynamic)Activator.CreateInstance(_eventHandlersAttribute.EventHandlers);
            BuildEventNameToMethodMapper(eventhandlers);
            var events = await _eventReader.ReadAllEventsFromSingleStreamCategoryAsync<AggregateEventCreator, AggregateEvent>(_category);
            foreach (var item in events)
            {
                _map[item.EventName].Invoke(eventhandlers, new object[] { item });
            }
            return eventhandlers.GetAggregate();
        }

        private void BuildEventNameToMethodMapper(dynamic eventhandlers)
        {
            var methods = ((object)eventhandlers).GetType().GetMethods();
            foreach (var method in methods)
            {
                var isEventHandlerFors = (EventHandlerForAttribute[])method.GetCustomAttributes(typeof(EventHandlerForAttribute), false);
                if(isEventHandlerFors.Any())
                {
                    foreach (var eventHandlerFor in isEventHandlerFors)
                    {
                        _map.Add(eventHandlerFor.EventName, method);
                    }
                }
            }
        }
    }
}