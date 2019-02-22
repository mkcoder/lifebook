using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace lifebook.app.calendar.api.Controllers
{
    [Route("api/[controller]")]
    public class CelendarController : Controller
    {
        // PUT api/values/5
        [HttpPost("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}
