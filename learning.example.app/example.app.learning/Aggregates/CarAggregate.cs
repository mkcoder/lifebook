using System;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Services;
using lifebook.core.eventstore.domain.api;
using lifebook.core.logging.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace example.app.learning.Aggregates
{
    [Aggregate]
    public class CarAggregate : Controller
    {
        private readonly ILogger _logger;

        public CarAggregate(ILogger logger)
        {
            _logger = logger;
        }

        [CommandHandlerFor("CreateCar")]
        public AggregateEvent Command(CreateCarCommand createCarCommand)
        {
            return AggregateEvent.Create
        }

    }


    public class CreateCarCommand : Command
    {
        public Guid Id { get; set; }
        public string CarDoorsWheelType { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string WheelType { get; set; }
        public string VehicleType { get; set; }
        public string Color { get; set; }
        public int CarDoors { get; set; }
        public bool Sunroof { get; set; }
        public string WindowType { get; set; }
    }
}
