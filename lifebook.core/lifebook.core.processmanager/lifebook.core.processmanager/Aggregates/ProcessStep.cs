using System;
using System.Collections.Generic;
using System.ComponentModel;
using lifebook.core.cqrses.Domains;
using lifebook.core.eventstore.domain.models;
using Newtonsoft.Json.Linq;

namespace lifebook.core.processmanager.Aggregates
{
    internal class ProcessStep
    {
        public bool Initiated { get; internal set; }
        public Guid CorrelationId { get; internal set; }
        public Guid? CausationId { get; internal set; }
        public string StepName { get; internal set; }
        public EventSpecifier[] EventSpecifier { get; internal set; }
        public AggregateEvent CausedBy { get; internal set; }
        public List<Exception> Exceptions { get; internal set; } = new List<Exception>();
        public ProcessStepStatus Status { get; internal set; }

        private ProcessStep() { }


        internal JObject AsJObject()
        {
            return new JObject()
            {
                ["StepName"] = StepName,
                ["CorrelationId"] = CorrelationId,
                ["CausationId"] = CausationId,
                ["EventSpecifier"] = JArray.FromObject(EventSpecifier),
                ["CausedBy"] = CausedBy == null ? null : JObject.FromObject(CausedBy),
                ["Exceptions"] = JArray.FromObject(Exceptions),
                ["Status"] = Status.ToString()
            };
        }

        public class ProcessStepBuilder
        {
            private readonly ProcessStep _processStep;

            private ProcessStepBuilder(ProcessStep processStep)
            {
                _processStep = processStep;
            }

            public static ProcessStepBuilder UsingProcessStep(ProcessStep processStep)
            {
                return new ProcessStepBuilder(processStep);
            }

            public static ProcessStepBuilder WithRequiredProperties(Guid correlationId, string stepName, EventSpecifier[] eventSpecifier)
            {
                var processStep = new ProcessStep();
                processStep.CorrelationId = correlationId;
                processStep.StepName = stepName;
                processStep.EventSpecifier = eventSpecifier;
                return new ProcessStepBuilder(processStep);
            }


            public static ProcessStepBuilder CreateFromJObject(JToken o)
            {
                var processStep = new ProcessStep();
                processStep.StepName = (string)o["StepName"];
                processStep.Initiated = (bool)(o["Initiated"] ?? false);
                processStep.CorrelationId = (Guid)o["CorrelationId"];
                processStep.CausationId = (Guid?)o["CausationId"];
                //processStep.EventSpecifier = o["EventSpecifier"].ToObject<EventSpecifier[]>();
                processStep.Exceptions = o["Exceptions"].ToObject<List<Exception>>();
                processStep.Status = o["Status"].ToObject<ProcessStepStatus>();
                return new ProcessStepBuilder(processStep);
            }


            public ProcessStepBuilder Initalized()
            {
                _processStep.Initiated = true;
                return this;
            }

            public ProcessStepBuilder CausedBy(AggregateEvent aggregateEvent)
            {
                _processStep.CausedBy = aggregateEvent;
                _processStep.CausationId = aggregateEvent.CausationId;
                return this;
            }


            public ProcessStepBuilder AddException(Exception exception)
            {
                _processStep.Exceptions.Add(exception);
                return this;
            }

            public ProcessStepBuilder ChangeProcessStatus(ProcessStepStatus status)
            {
                _processStep.Status = status;
                return this;
            }

            internal ProcessStep Build()
            {
                return _processStep;
            }
        }
    }
}
