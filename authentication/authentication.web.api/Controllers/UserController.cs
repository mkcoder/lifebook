using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace authentication.web.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController()
        {
            // Injects the IUserRespository
            /* 
             *  IUserRepository
             *  -> GetUsers
             *  -> AddNewUsers(UserObject());
            */
        }
    }
}
