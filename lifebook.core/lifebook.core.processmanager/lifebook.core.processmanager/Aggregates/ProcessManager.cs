using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using lifebook.core.cqrses.Domains;
using lifebook.core.messagebus.Models;
using lifebook.core.processmanager.Attributes;
using lifebook.core.processmanager.Models;
using lifebook.core.processmanager.ProcessStates;
using lifebook.core.processmanager.ProcessStates.ProcessSetup;
using lifebook.core.processmanager.Syntax;
using Newtonsoft.Json.Linq;

namespace lifebook.core.processmanager.Aggregates
{
    // TODO: initalizing the ProcessManager 
    internal class ProcessManager
    {
        private static Dictionary<string, MethodInfo> eventNameToMethods = new Dictionary<string, MethodInfo>();
        private static object _lock = new object();
        private List<ModelEvent> _uncommitedEvents = new List<ModelEvent>();
        internal ReadOnlyCollection<ModelEvent> GetUncommitedEvents => _uncommitedEvents.AsReadOnly();
        private readonly Guid processId;
        private List<ProcessStep> _steps = new List<ProcessStep>();
        private ProcessStep.ProcessStepBuilder _currentStep;

        public ProcessManager(Guid processId)
        {
            this.processId = processId;
        }
        // aggregate properties
        private bool _initalized = false;
        private ProcessIdentity _processIdentity;
        private dynamic _queueInfo;

        internal void Handle(List<AggregateEvent> events)
        {
            BuildEventNameToMethodInfoDictionary();
            foreach (var evt in events)
            {
                if (eventNameToMethods.ContainsKey(evt.EventName))
                {
                    eventNameToMethods[evt.EventName].Invoke(this, new object[] { evt });
                }
            }
        }

        internal bool AmIFirstStep()
        {
            return _initalized;
        }

        internal void InitalizeProcess(EventBusMessage<ProcessStateMessageDto> a, SetupProcess pRequest)
        {
            _initalized = true;
            _processIdentity = pRequest.ProcessIdentity;
            _queueInfo = new
            {
                a.ExchangeName,
                a.RoutingKey,
                a.MessageName                
            };
            CreateUncommitedEvent("ProcessInitalized", 1, new JObject()
            {
                ["Initalized"] = true,
                ["ProcessIdentity"] = JObject.FromObject(_processIdentity),
                ["QueueInfo"] = JObject.FromObject(_queueInfo),
                ["Steps"] = CreateStepsForInitalProcess(a, pRequest),
                ["Key"] = processId,
            });
        }

        internal void ChangeProcessData(dynamic getViewBag)
        {
            CreateUncommitedEvent("ProcessDataChanged", 1, new JObject()
            {
                ["Data"] = JObject.FromObject(getViewBag)
            });
        }

        private JToken CreateStepsForInitalProcess(EventBusMessage<ProcessStateMessageDto> a, SetupProcess pRequest)
        {
            var stepNames = pRequest.ProcessManager.EventNameToProcessStepDictionary.Select(p => new { StepName = p.Key, Step = p.Value});
            
            return JArray.FromObject(
                stepNames.Select(kv => ProcessStep.ProcessStepBuilder
                .WithRequiredProperties(a.Data.AggregateEvent.CorrelationId, kv.StepName, kv.Step.EventSpecifier))
            );
        }

        internal void InitalizeProcessStep(ProcessManagerStep action, AggregateEvent a)
        {
            _currentStep = ProcessStep.ProcessStepBuilder.WithRequiredProperties(a.CorrelationId, action.StepDescription, action.EventSpecifier);
            _currentStep
                .Initalized()
                .CausedBy(a)                
                .ChangeProcessStatus(ProcessStepStatus.Started);
            CreateUncommitedEvent("ProcessStepInitalized", 1, JObject.FromObject(_currentStep.Build()));
        }

        internal void CompleteProcessStep()
        {
            _currentStep.ChangeProcessStatus(ProcessStepStatus.Completed);
            var processStep = _currentStep.Build();

            CreateUncommitedEvent("ProcessStepCompleted", 1, new JObject()
            {
                ["StepName"] = processStep.StepName,
                ["Status"] = processStep.Status.ToString(),
                ["StatusInt"] = (int)processStep.Status
            });
        }

        internal void FailProcessStep(Exception exception)
        {
            _currentStep.AddException(exception);
            _currentStep.ChangeProcessStatus(ProcessStepStatus.Failed);
            var processStep = _currentStep.Build();
            CreateUncommitedEvent("ProcessStepFailed", 1, new JObject()
            {
                ["StepName"] = processStep.StepName,
                ["Status"] = processStep.Status.ToString(),
                ["StatusInt"] = (int)processStep.Status
            });
        }

        private void CreateUncommitedEvent(string eventName, int eventVersion, JObject data)
        {
            _uncommitedEvents.Add(
                new ModelEvent()
                {
                    EventName = eventName,
                    EventVersion = eventVersion,
                    Data = data
                }
            );
        }

        private void BuildEventNameToMethodInfoDictionary()
        {
            if (eventNameToMethods.Count == 0)
            {
                lock (_lock)
                {
                    if (eventNameToMethods.Count == 0)
                    {
                        var mi = GetType().GetMethods(BindingFlags.NonPublic)
                                        .Where(m => m.GetCustomAttributes(typeof(UponProcessEvent), false).Count() > 0);

                        foreach (var m in mi)
                        {
                            eventNameToMethods = eventNameToMethods.MergeDictionaries(m.GetCustomAttributes(typeof(UponProcessEvent), false)
                             .Select(a => (UponProcessEvent)a)
                             .Select(a => a.EventName)
                             .ToDictionary(value => value, key => m));
                        }
                    }
                }
            }
        }
    }

    public static class CollectionExtensions
    {
        public static Dictionary<TKey, TValue> MergeDictionaries<TKey, TValue>(this Dictionary<TKey, TValue> t, Dictionary<TKey, TValue> o)
        {
            return t.SelectMany(a => o)
                    .ToLookup(pair => pair.Key, pair => pair.Value)
                    .ToDictionary(group => group.Key, group => group.First());
        }
    }
}
