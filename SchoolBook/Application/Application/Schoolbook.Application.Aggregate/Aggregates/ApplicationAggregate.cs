using System;
using System.Collections.Generic;
using System.Text.Json;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;
using lifebook.Schoolbook.Application.Aggregate.Handlers.Commands;

namespace Schoolbook.Application
{
    public class ApplicationAggregate : Aggregate
    {
        public ApplicationAggregate GetAggregate() => this;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public Address Address { get; set; }
        public List<Question> Questions { get; set; }


        [EventHandlerFor("ApplicationCreated")]
        public void ApplicationCreated(AggregateEvent e)
        {
            var application = e.Data.TransformDataFromString(j => JsonSerializer.Deserialize<ApplicationCreatedV1>(j));
            FirstName = application.FirstName;
            LastName = application.LastName;
            DOB = application.DOB;
            Address = application.Address;
            Questions = application.Questions;
        }
    }
}
