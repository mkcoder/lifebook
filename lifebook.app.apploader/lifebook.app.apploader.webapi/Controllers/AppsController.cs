using System.Collections.Generic;
using lifebook.app.apploader.services.Models;
using lifebook.app.apploader.services.Repository;
using lifebook.core.database.repository.interfaces;
using Microsoft.AspNetCore.Mvc;


namespace lifebook.app.apploader.webapi.Controllers
{
    [Route("api/[controller]")]
    public class AppsController : Controller
    {
        private readonly IRepository<App> appRepository;

        public AppsController(IRepository<App> appRepository)
        {
            this.appRepository = appRepository;
        }

        // GET: api/values
        [HttpGet]
        public List<App> Get()
        {
            return appRepository.GetAll();
        }
    }
}
