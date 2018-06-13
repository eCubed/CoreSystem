using FCore.EntityFramework;
using FCore.Interactions.Tagging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FCore.Interactions.EntityFramework
{
    public class TaggingStore : EntityStoreBase<Tagging, long>, ITaggingStore<Tagging, Tag>
    {
        public TaggingStore(DbContext context) : base(context)
        {
        }

        public async Task<Tagging> FindAsync(int systemObjectId, string recordId, long tagId, string taggerId)
        {
            return await db.Set<Tagging>().SingleOrDefaultAsync(tg => tg.SystemObjectId == systemObjectId &&
                tg.RecordId == recordId && tg.TagId == tagId && tg.TaggerId == taggerId);
        }

        public IQueryable<ISystemObject> GetQueryableSystemObjects()
        {
            return db.Set<SystemObject>().AsQueryable();
        }

        public IQueryable<Tagging> GetQueryableTaggings()
        {
            return db.Set<Tagging>().Include(tg => tg.Tag).AsQueryable();
        }
    }
}
