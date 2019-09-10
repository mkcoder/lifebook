E﻿using System;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;
using lifebook.core.eventstore.domain.models;
using Microsoft.AspNetCore.Mvc;

namespace lifebook.core.cqrs.tests.Aggregates
{
    [CommandHandlers]
    public class PersonController : AggregateRoot
    {
        [Route("hello"), HttpGet]
        public string Hello() { return "hello"; }

        [CommandHandlerFor("CreatePerson")]
        public AggregateEvent CreatePerson(CreatePersonCommand createPersonCommand)
        {
            return new PersonCreated()
            {
                FirstName = createPersonCommand.FirstName
            };
        }

        public class CreatePersonCommand : Command
        {
            public string FirstName { get; set; }
        }

        public class PersonCreated : AggregateEvent
        {
            public string FirstName { get; set; }
        }
    }
}
