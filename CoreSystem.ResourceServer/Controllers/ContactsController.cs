using CoreSystem.EntityFramework;
using CoreSystem.ResourceServer.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreSystem.ResourceServer.Controllers
{
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private CoreSystemDbContext db { get; set; }
        private ContactManager<Contact> contactManager { get; set; }
        private UserManager<CoreSystemUser> userManager { get; set; }

        public ContactsController(CoreSystemDbContext context, UserManager<CoreSystemUser> userManager)
        {
            db = context;
            contactManager = new ContactManager<Contact>(new ContactStore(db));
            this.userManager = userManager;
        }

        // GET: api/values
        [HttpGet]
        [Route("search/{startsWith}/{page}/{pageSize}")]
        public IActionResult Search(string startsWith, int page, int pageSize)
        {
            var resultSet = contactManager.SearchContacts(startsWith, page, pageSize);
            return Ok(resultSet);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]SaveContactViewModel scvm)
        {
            AuthenticatedInfo authInfo = await this.ResolveAuthenticatedEntitiesAsync(db, userManager);

            var res = await contactManager.CreateAsync(scvm, authInfo.UserId);

            if (!res.Success)
                return BadRequest(res.Errors);

            return StatusCode(201, new { Id = scvm.Id });
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody]SaveContactViewModel scvm)
        {
            AuthenticatedInfo authInfo = await this.ResolveAuthenticatedEntitiesAsync(db, userManager);

            var res = await contactManager.UpdateAsync(scvm, authInfo.UserId);

            if (!res.Success)
                return BadRequest(res.Errors);

            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            AuthenticatedInfo authInfo = await this.ResolveAuthenticatedEntitiesAsync(db, userManager);

            var res = await contactManager.DeleteAsync(id, authInfo.UserId);

            if (!res.Success)
                return BadRequest(res.Errors);

            return NoContent();
        }
    }
}
