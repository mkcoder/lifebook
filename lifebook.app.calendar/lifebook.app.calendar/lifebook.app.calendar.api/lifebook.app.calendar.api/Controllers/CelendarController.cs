using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lifebook.app.calendar.api.Mocks;
using lifebook.app.calendar.api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace lifebook.app.calendar.api.Controllers
{
    [Route("api/[controller]")]
    public class CelendarController : Controller
    {
        // PUT api/values/5
        [HttpGet("/GetEventsReminderForClient/{userId:Guid}")]
        public Calendar GetEventsReminderForClient(Guid userId)
        {
            return UserProjectionMock.GetCalendarByUserID(userId);
        }
    }
}
