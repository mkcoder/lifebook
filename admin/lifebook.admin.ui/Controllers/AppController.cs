using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using lifebook.app.apploader.services.Models;
using lifebook.core.database.repository.interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace lifebook.admin.ui.Controllers
{
    public class AppController : Controller
    {
        private readonly IRepository<App> appRepository;

        public AppController(IRepository<App> appRepository)
        {
            this.appRepository = appRepository;
        }

        // GET: /<controller>/
        [Route("App")]
        public IActionResult App()
        {
            return View(appRepository.GetAll());
        }

        [HttpGet]
        [Route("/app/register")]
        public IActionResult Add()
        {
            return View(new lifebook.app.apploader.services.Models.App());
        }

        [Route("/app/register")]
        [HttpPost]
        public IActionResult Add(App app)
        {
            var file = Request.Form.Files[0];
            var sr = new MemoryStream();
            var stream = file.OpenReadStream();
            Image image = Image.FromStream(stream);
            if (image.Width != 128 && image.Height != 128) return Add();
            stream.Position = 0;
            stream.CopyTo(sr);
            app.Icon = sr.ToArray();
            appRepository.Add(app);
            return App();
        }
    }
}
