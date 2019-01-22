using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace admin.ui.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/account")]
        public IActionResult Account()
        {
            return View();
        }

        [HttpPost("/account")]
        public IActionResult Account(string username, string password, string claims)
        {
            // Get Authentication using consul
            // post this data to the user end point
            // once it is sent refresh the data on the front page by making another call to an end
            return View();
        }
    }
}
