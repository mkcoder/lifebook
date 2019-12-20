using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;

namespace lifebook.core.processmanager.Syntax
{
    public class ProcessManagerStep
    {
        public EventSpecifier[] EventSpecifier { get; internal set; }
        public string StepName { get; internal set; }
        public Func<EntityEvent, Task> StepAction { get; internal set; }
    }

    public class ProcessManagerConfiguration
    {
        private List<ProcessManagerStep> Steps { get; } = new List<ProcessManagerStep>();
        public ProcessManagerStep CurrentStep { get; private set; } = new ProcessManagerStep();
        public void ConfigureNextStep()
        {
            Steps.Add(CurrentStep);
            CurrentStep = new ProcessManagerStep();
        }

        internal ProcessManagerConfiguration Build()
        {
            throw new NotImplementedException();
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

        public ProcessManagerConfigurationBuilder TakeStepName(string name)
        {
            _configuration.CurrentStep.StepName = name;
            return this;
        }

        public ProcessManagerConfigurationBuilder TakeAction(Func<EntityEvent, Task> action)
        {
            EntityEvent e = new EntityEvent();
            _configuration.CurrentStep.StepAction = action;
            return this;
        }

        public ProcessManagerConfigurationBuilder AndThen()
        {
            _configuration.ConfigureNextStep();
            return this;
        }

        public ProcessManagerConfiguration Configuration() => _configuration.Build();
    }
}
