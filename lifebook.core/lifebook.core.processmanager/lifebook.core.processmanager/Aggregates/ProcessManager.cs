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
        public dynamic Data { get; private set; } = new JObject();
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
        private Dictionary<string, ProcessStep.ProcessStepBuilder> _processSteps = new Dictionary<string, ProcessStep.ProcessStepBuilder>();

        public ProcessManager(Guid processId)
        {
            _processId = processId;
        }

        internal void Handle(List<AggregateEvent> events)
        {
            BuildEventNameToMethodInfoDictionary();
            events = events.Where(e => e.Data.TransformDataFromString(s => s) != "{}").ToList();
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
            return !_initalized;
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

            var pSteps = CreateStepsForInitalProcess(a, pRequest);
            _processSteps = pSteps
                .Select(s => new { StepName = (string)s["StepName"], Builder = ProcessStep.ProcessStepBuilder.CreateFromJObject(s) })
                .Distinct()
                .ToDictionary(k => k.StepName, v => v.Builder);
            CreateUncommitedEvent(nameof(ProcessInitalized), 1, new JObject()
            {
                ["Initalized"] = true,
                ["ProcessIdentity"] = JObject.FromObject(_processIdentity),
                ["QueueInfo"] = JObject.FromObject(_queueInfo),
                ["Steps"] = pSteps,
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
            _currentStep = _processSteps[action.StepDescription];
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
            _currentStep.AddException(exception)
                        .ChangeProcessStatus(ProcessStepStatus.Failed);
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
        private void ProcessDataChanged(AggregateEvent aggregate)
        {
            var evtData = aggregate.Data.TransformDataFromString(s => JObject.Parse(s));
            Data = evtData["Data"].ToObject<JObject>();
        }

        [UponProcessEvent(nameof(ProcessInitalized))]
        private void ProcessInitalized(AggregateEvent aggregate)
        {
            var evt = aggregate.Data.TransformDataFromString(s => JObject.Parse(s));
            _processId = (Guid)evt["Key"];
            _initalized = (bool)evt["Initalized"];
            _processIdentity = evt["ProcessIdentity"].ToObject<ProcessIdentity>();
            _queueInfo = evt["QueueInfo"].ToObject<MessageQueueInformation>();
            _processSteps = evt["Steps"]
                .Select(s => new { StepName = (string)s["StepName"], Builder = ProcessStep.ProcessStepBuilder.CreateFromJObject(s) })
                .Distinct()
                .ToDictionary(k => k.StepName, v => v.Builder);
        }

        [UponProcessEvent(nameof(ProcessStepInitalized))]
        private void ProcessStepInitalized(AggregateEvent aggregate)
        {            
            var evt = aggregate.Data.TransformDataFromString(s => JObject.Parse(s));
            var stepName = (string)evt["StepName"];
            if (_processSteps.ContainsKey(stepName))
            {
                _processSteps[stepName]
                .CausedBy(evt["CausedBy"].ToObject<AggregateEvent>())
                .Initalized()
                .ChangeProcessStatus(evt["StatusInt"].ToObject<ProcessStepStatus>());
            }         
        }

        [UponProcessEvent(nameof(ProcessStepCompleted))]
        private void ProcessStepCompleted(AggregateEvent aggregate)
        {
            var evt = aggregate.Data.TransformDataFromString(s => JObject.Parse(s));
            var stepName = (string)evt["StepName"];

            if (_processSteps.ContainsKey(stepName))
            {
                _processSteps[stepName]
                .ChangeProcessStatus(evt["StatusInt"].ToObject<ProcessStepStatus>());
            }         
        }

        [UponProcessEvent(nameof(ProcessStepFailed))]
        private void ProcessStepFailed(AggregateEvent aggregate)
        {
            var evt = aggregate.Data.TransformDataFromString(s => JObject.Parse(s));
            var stepName = (string)evt["StepName"];
            if (_processSteps.ContainsKey(stepName))
            {
                _processSteps[stepName]
                .AddException(evt["Exception"].ToObject<Exception>());
            }
        }

        private void BuildEventNameToMethodInfoDictionary()
        {
            if (eventNameToMethods.Count == 0)
            {
                lock (_lock)
                {
                    if (eventNameToMethods.Count == 0)
                    {
                        var mi = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                                        .Where(m => m.GetCustomAttributes(typeof(UponProcessEvent), false).Count() > 0);
                        var mm = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
                        var mi2 = mm.Where(m => m.GetCustomAttribute(typeof(UponProcessEvent)) != null);

                        foreach (var m in mi2)
                        {
                            foreach (UponProcessEvent item in m.GetCustomAttributes(typeof(UponProcessEvent), false))
                            {
                                eventNameToMethods.Add(item.EventName, m);
                            }
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
            var stepNames = pRequest.ProcessManager.EventNameToProcessStepDictionary.Select(p => new { StepName = p.Value.StepDescription, Step = p.Value });
            var steps = stepNames.Select(kv => ProcessStep.ProcessStepBuilder
                .WithRequiredProperties(a.Data.AggregateEvent.CorrelationId, kv.StepName, kv.Step.EventSpecifier).Build().AsJObject()).ToList();
            return JArray.FromObject(
                steps
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
