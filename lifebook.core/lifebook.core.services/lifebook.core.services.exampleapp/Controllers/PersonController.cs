using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace lifebook.core.services.exampleapp
{
	public interface IGetPerson
	{
		string Name();
	}

	public class GetPerson : IGetPerson
	{
		public string Name()
		{
			return "Bob";
		}
	}

	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		public IGetPerson Person { get; }

		public ValuesController(IGetPerson person)
		{
			Person = person;
		}

		// GET api/values
		[HttpGet]
		public ActionResult<IEnumerable<string>> Get()
		{
			return new string[] { "value1", "value2", Person.Name() };
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public ActionResult<string> Get(int id)
		{
			return "value";
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
