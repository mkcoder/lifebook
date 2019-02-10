using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using lifebook.authentication.core.repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace lifebook.authentication.web.api.Controllers
{
    [Route("api/[controller]")]
    public class ClientsController : Controller
    {
        private readonly ClientRepository _clientRepository;

        public ClientsController(ClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        // GET: api/values
        [HttpGet]
        public List<Client> Get()
        {
            var c = new Client()
            {
                ClientId = "admin.ui.client",
                Claims = { }
            };
            return _clientRepository.GetAllClients();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Client value)
        {
            _clientRepository.Add(value);
        }
    }
}
