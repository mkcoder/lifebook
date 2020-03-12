using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using lifebook.core.cqrses.Services;

namespace lifebook.Schoolbook.Application.Aggregate.Handlers.Commands
{
    public class CreateApplicationForStudentV1 : Command
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


    public class Address
    {
        [Required]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        [Required]
        public string Zipcode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
    }

    public class Question
    {
        [Required]
        public int QId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Answer { get; set; }
    }
}
