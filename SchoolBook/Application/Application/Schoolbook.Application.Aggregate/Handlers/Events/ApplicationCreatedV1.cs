using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;

namespace lifebook.Schoolbook.Application.Aggregate.Handlers.Commands
{
    public class ApplicationCreatedV1 : AggregateEvent
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        public Address Address { get; set; }
        [Required]
        public List<Question> Questions { get; set; }
    }
}
