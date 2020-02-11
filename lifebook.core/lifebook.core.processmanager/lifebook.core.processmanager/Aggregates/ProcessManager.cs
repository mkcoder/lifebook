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
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json.Linq;

namespace lifebook.core.processmanager.Aggregates
{
    // TODO: initalizing the ProcessManager 
    internal class ProcessManager
    {
        public dynamic Data { get; private set; }
        internal ReadOnlyCollection<ModelEvent> GetUncommitedEvents => _uncommitedEvents.AsReadOnly();

        private static Dictionary<string, MethodInfo> eventNameToMethods = new Dictionary<string, MethodInfo>();
        private static object _lock = new object();
        private List<ModelEvent> _uncommitedEvents = new List<ModelEvent>();
        private Guid _processId;
        private List<ProcessStep> _steps = new List<ProcessStep>();
        private ProcessStep.ProcessStepBuilder _currentStep;
        // aggregate properties
        private bool _initalized = false;
        private ProcessIdentity _processIdentity;
        private MessageQueueInformation _queueInfo;

        public ProcessManager(Guid processId)
        {
            _processId = processId;
        }

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
            _queueInfo = new MessageQueueInformation()
            {
                ExchangeName = a.ExchangeName,
                RoutingKey = a.RoutingKey,
                QueueName = a.MessageName
            };
            CreateUncommitedEvent(nameof(ProcessInitalized), 1, new JObject()
            {
                ["Initalized"] = true,
                ["ProcessIdentity"] = JObject.FromObject(_processIdentity),
                ["QueueInfo"] = JObject.FromObject(_queueInfo),
                ["Steps"] = CreateStepsForInitalProcess(a, pRequest),
                ["Key"] = _processId,
            });
        }

        internal void ChangeProcessData(dynamic getViewBag)
        {
            CreateUncommitedEvent(nameof(ProcessDataChanged), 1, new JObject()
            {
                ["Data"] = JObject.FromObject(getViewBag)
            });
        }

        internal void InitalizeProcessStep(ProcessManagerStep action, AggregateEvent a)
        {
            _currentStep = ProcessStep.ProcessStepBuilder.WithRequiredProperties(a.CorrelationId, action.StepDescription, action.EventSpecifier);
            _currentStep
                .Initalized()
                .CausedBy(a)                
                .ChangeProcessStatus(ProcessStepStatus.Started);
            var step = _currentStep.Build();
            CreateUncommitedEvent(nameof(ProcessStepInitalized), 1, new JObject()
            {
                ["StepName"] = step.StepName,
                ["CausedBy"] = JObject.FromObject(step.CausedBy),
                ["CausationId"] = step.CausationId,
                ["Initiated"] = step.Initiated,
                ["Status"] = step.Status.ToString(),
                ["StatusInt"] = (int)step.Status
            });
        }

        internal void CompleteProcessStep()
        {
            _currentStep.ChangeProcessStatus(ProcessStepStatus.Completed);
            var processStep = _currentStep.Build();

            CreateUncommitedEvent(nameof(ProcessStepCompleted), 1, new JObject()
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
                ["Exception"] = JObject.FromObject(exception),
                ["StatusInt"] = (int)processStep.Status
            });
        }

        [UponProcessEvent("ProcessDataChanged")]
        public void ProcessDataChanged(AggregateEvent aggregate)
        {
            Data = aggregate.Data.TransformDataFromString(s => JObject.FromObject(s));
        }

        [UponProcessEvent(nameof(ProcessInitalized))]
        public void ProcessInitalized(AggregateEvent aggregate)
        {
            var evt = aggregate.Data.TransformDataFromString(s => JObject.Parse(s));
            _processId = (Guid)evt["Key"];
            _initalized = (bool)evt["Initalized"];
            _processIdentity = evt["ProcessIdentity"].ToObject<ProcessIdentity>();
            _queueInfo = evt["QueueInfo"].ToObject<MessageQueueInformation>();
            _steps = evt["Steps"].ToObject<List<ProcessStep>>();
        }

        [UponProcessEvent(nameof(ProcessStepInitalized))]
        public void ProcessStepInitalized(AggregateEvent aggregate)
        {
            var evt = aggregate.Data.TransformDataFromString(s => JObject.Parse(s));
            _currentStep = ProcessStep.ProcessStepBuilder.UsingProcessStep(_steps.First(s => s.StepName == (string)evt["StepName"]));
            _currentStep
                .CausedBy(evt["CausedBy"].ToObject<AggregateEvent>())
                .Initalized()
                .ChangeProcessStatus(evt["StatusInt"].ToObject<ProcessStepStatus>());
        }

        [UponProcessEvent(nameof(ProcessStepCompleted))]
        public void ProcessStepCompleted(AggregateEvent aggregate)
        {
            var evt = aggregate.Data.TransformDataFromString(s => JObject.Parse(s));
            _currentStep = ProcessStep.ProcessStepBuilder.UsingProcessStep(_steps.First(s => s.StepName == (string)evt["StepName"]));
            _currentStep
                .ChangeProcessStatus(evt["StatusInt"].ToObject<ProcessStepStatus>());            
        }

        [UponProcessEvent(nameof(ProcessStepFailed))]
        public void ProcessStepFailed(AggregateEvent aggregate)
        {
            var evt = aggregate.Data.TransformDataFromString(s => JObject.Parse(s));
            _currentStep = ProcessStep.ProcessStepBuilder.UsingProcessStep(_steps.First(s => s.StepName == (string)evt["StepName"]));
            _currentStep
                .AddException(evt["Exception"].ToObject<Exception>());
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

        private JToken CreateStepsForInitalProcess(EventBusMessage<ProcessStateMessageDto> a, SetupProcess pRequest)
        {
            var stepNames = pRequest.ProcessManager.EventNameToProcessStepDictionary.Select(p => new { StepName = p.Key, Step = p.Value });

            return JArray.FromObject(
                stepNames.Select(kv => ProcessStep.ProcessStepBuilder
                .WithRequiredProperties(a.Data.AggregateEvent.CorrelationId, kv.StepName, kv.Step.EventSpecifier))
            );
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
