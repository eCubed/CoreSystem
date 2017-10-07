using CoreLibrary.EntityFramework;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoreSystem.EntityFramework
{
    public class ContactStore : EntityStoreBase<Contact, int>, IContactStore<Contact>
    {
        public ContactStore(CoreSystemDbContext context) : base(context)
        {
        }

        public async Task<Contact> FindContactAsync(string firstName, string lastName, int userId)
        {
            return await db.Set<Contact>().SingleOrDefaultAsync(c => c.FirstName == firstName &&
                c.LastName == lastName && c.UserId == userId);
        }

        public IQueryable<Contact> GetQueryableContacts()
        {
            return db.Set<Contact>().AsQueryable();
        }
    }
}
