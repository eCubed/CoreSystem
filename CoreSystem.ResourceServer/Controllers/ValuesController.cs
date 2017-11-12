using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CoreLibrary.ResourceServer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreSystem.ResourceServer.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            //return new string[] { "value1", "value2" };
            //return BadRequest(new { Message = "Just because" });
            //return NotFound(new { Message = "Just not found" });
            return Ok(new OkApiResponse<string[]>(new string[] { "value1", "value2" }));
        }

        // GET api/values/5
        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(new OkApiResponse<object>(new { Name = "Los Lobos", Song = "Macarena" }));
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            return StatusCode(201, new CreatedApiResponse<int>(2500));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            return Ok(new OkApiResponse<object>(new { Id = 2700 }));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return StatusCode(204, new NoContentApiResponse<int>(2800));
        }
    }
}
