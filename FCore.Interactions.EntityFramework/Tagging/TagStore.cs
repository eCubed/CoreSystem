using FCore.Interactions.Tagging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FCore.EntityFramework;

namespace FCore.Interactions.EntityFramework
{
    public class TagStore : EntityStoreBase<Tag, long>, ITagStore<Tag>
    {
        public TagStore(DbContext context) : base(context)
        {
        }

        public async Task<Tag> FindAsync(string name)
        {
            return await db.Set<Tag>().SingleOrDefaultAsync(t => t.Name == name);
        }

        public IQueryable<Tag> GetQueryableTags()
        {
            return db.Set<Tag>().AsQueryable();
        }
    }
}
