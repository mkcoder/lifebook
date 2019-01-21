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
    }
}
