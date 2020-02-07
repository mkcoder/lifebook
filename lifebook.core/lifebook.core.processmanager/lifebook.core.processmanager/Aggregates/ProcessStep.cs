using System;
using System.Collections.Generic;
using lifebook.core.cqrses.Domains;
using lifebook.core.eventstore.domain.models;

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
        public int Hash { get => HashCode.ToHashCode(); }
        public HashCode HashCode
        {
            get
            {
                var hashCode = new HashCode();
                hashCode.Add($"{DateTime.Now.ToLongTimeString()}");
                hashCode.Add($"{CorrelationId}");
                hashCode.Add($"{Initiated}");                
                return HashCode;
            }
        }

        private ProcessStep() { }

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
