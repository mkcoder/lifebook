using System;
namespace lifebook.app.calendar.api.Models
{
    public class GetEventsReminderForClient
    {
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Increments { get; set; }
    }
}
