using System;
using Microsoft.AspNetCore.Mvc;

namespace lifebook.core.cqrs.example
{
    [ApiController]
    public class TestClass : ControllerBase
    {
        [HttpGet("/hello")]
        public string hello()
        {
            return "hello";
        }
    }
}
