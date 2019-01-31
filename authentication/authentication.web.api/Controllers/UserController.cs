using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lifebook.core.database.repository.interfaces;
using lifebook.core.database.repository.repositories;
using Microsoft.AspNetCore.Mvc;

namespace authentication.web.api.Controllers
{
    [Route("api/[contoller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRepository = null;

        [HttpGet("/test")]
        public string test() => "hello";

        [HttpGet("/all")]
        public List<User> GetUsers() => _userRepository.GetAllUsers();

        [HttpPost("/add")]
        public User Add(User user) => _userRepository.AddNewUser(user);
    }
}
