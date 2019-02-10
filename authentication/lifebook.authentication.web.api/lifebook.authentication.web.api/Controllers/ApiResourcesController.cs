using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using lifebook.authentication.core.dbcontext;
using lifebook.authentication.core.repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace lifebook.authentication.web.api.Controllers
{
    [Route("api/[controller]")]
    public class ApiResourcesController : Controller
    {
        public ApiRepository ApiRepository { get; }

        public ApiResourcesController(ApiRepository apiRepository)
        {
            ApiRepository = apiRepository;
        }

        // GET: api/values
        [HttpGet]
        public List<ApiResource> Get()
        {
            return ApiRepository.GetAllApiResources();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]ApiResource value)
        {
            ApiRepository.AddNewApiResource(value);
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
