using System;
using System.Collections.Generic;

namespace lifebook.app.calendar.api.Mocks
{
    public static class CalendarProjectionMock
    {
        public static Calendar Projection(DateTime start, DateTime end)
        {
            var projection = new Calendar();

            for (int i = 0; i < (end.Date-start.Date).Days; i++)
            {
                var @event = Event.GetRandomEvent($"Event{i}", start, 5);
                var reminder = new Reminder
                {
                    EventGuid = @event.EventId,
                    MintuesBefore = 30,
                    Recurring = false
                };
                projection.Project(@event, reminder);
            }

            return projection;
        }
    }

    public class Calendar
    {
        public List<Event> Events { get; set; } = new List<Event>();
        public List<Reminder> Reminders { get; set; } = new List<Reminder>();

        public void Project(Event e, Reminder r=null)
        {
            Events.Add(e);
            if (r != null) Reminders.Add(r);
        }

    }

    public class Reminder
    {
        public Guid ReminderId { get; } = Guid.NewGuid();
        public Guid EventGuid { get; set; } 
        public int MintuesBefore { get; set; }
        public bool Recurring { get; set; }
    }

    public class Event
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public string EventName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool ReminderSet { get; set; }

        public static Event GetRandomEvent(string eName, DateTime s, int off, bool reminder = true)
        {
            return new Event
            {
                EventName = eName,
                StartDateTime = s,
                EndDateTime = s.AddHours(off),
                ReminderSet = reminder
            };
        }
    }
}
