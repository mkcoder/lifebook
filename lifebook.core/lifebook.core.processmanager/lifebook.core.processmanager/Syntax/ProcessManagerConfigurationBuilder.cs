using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using lifebook.core.cqrses.Domains;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;
using Newtonsoft.Json;

namespace lifebook.core.processmanager.Syntax
{
    public class ProcessManagerStep
    {
        public EventSpecifier[] EventSpecifier { get; internal set; }
        public string StepDescription { get; internal set; }
        [JsonIgnore]
        public Func<AggregateEvent, Task> StepAction { get; internal set; }
    }

    public class ProcessManagerConfiguration
    {
        private List<ProcessManagerStep> Steps { get; } = new List<ProcessManagerStep>();
        internal ProcessManagerStep CurrentStep { get; private set; } = new ProcessManagerStep();

        public IReadOnlyList<ProcessManagerStep> GetProcessManagerSteps => Steps.AsReadOnly();

        public void ConfigureNextStep()
        {
            if (CurrentStep.EventSpecifier != null && CurrentStep.StepDescription != null)
            {
                Steps.Add(CurrentStep);
                CurrentStep = new ProcessManagerStep();
            }
        }

        internal ProcessManagerConfiguration Build()
        {
            ConfigureNextStep();
            return this;
        }
    }

    public class ProcessManagerConfigurationBuilder
    {
        private readonly ProcessManagerConfiguration _configuration;

        private ProcessManagerConfigurationBuilder(ProcessManagerConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static ProcessManagerConfigurationBuilder Instance => new ProcessManagerConfigurationBuilder(new ProcessManagerConfiguration());


        public ProcessManagerConfigurationBuilder UponEvent(params EventSpecifier[] eventSpecifier)
        {
            _configuration.CurrentStep.EventSpecifier = eventSpecifier;
            return this;
        }

        public ProcessManagerConfigurationBuilder SetStepDescription(string description)
        {
            _configuration.CurrentStep.StepDescription = description;
            return this;
        }

        public ProcessManagerConfigurationBuilder TakeAction(Func<AggregateEvent, Task> action)
        {  
            _configuration.CurrentStep.StepAction = action;
            return this;
        }

        public ProcessManagerConfigurationBuilder AndThen()
        {
            _configuration.ConfigureNextStep();
            return this;
        }

        public ProcessManagerConfiguration Configuration => _configuration.Build();
    }
}
