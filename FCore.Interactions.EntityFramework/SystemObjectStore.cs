using FCore.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FCore.Interactions.EntityFramework
{
    public class SystemObjectStore : EntityStoreBase<SystemObject, int>, ISystemObjectStore<SystemObject>
    {
        public SystemObjectStore(DbContext context) : base(context)
        {
        }

        public async Task<SystemObject> FindAsync(string name)
        {
            return await db.Set<SystemObject>().SingleOrDefaultAsync(so => so.Name == name);
        }

        public IQueryable<SystemObject> GetQueryableSystemObjects()
        {
            return db.Set<SystemObject>().AsQueryable();
        }
    }
}
