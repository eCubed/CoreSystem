using FCore.Foundations;
using System.Threading.Tasks;

namespace FCore.Interactions
{
    public class SystemObjectManager<TSystemObject> : ManagerBase<TSystemObject, int>
        where TSystemObject : class, ISystemObject
    {
        public SystemObjectManager(ISystemObjectStore<TSystemObject> store) : base(store)
        {
        }

        protected ISystemObjectStore<TSystemObject> GetSystemObjectStore()
        {
            return (ISystemObjectStore<TSystemObject>)Store;
        }

        protected async Task<TSystemObject> FindAsync(TSystemObject systemObject)
        {
            return await GetSystemObjectStore().FindAsync(systemObject.Name);
        }

        public virtual async Task<ManagerResult> CreateAsync(TSystemObject systemObject)
        {
            return await DataUtils.CreateAsync(entity: systemObject,
                store: GetSystemObjectStore(),
                findUniqueAsync: FindAsync);
        }

        public virtual async Task<ManagerResult> UpdateAsync(TSystemObject systemObject)
        {
            return await DataUtils.UpdateAsync(id: systemObject.Id,
                store: GetSystemObjectStore(),
                findUniqueAsync: FindAsync,
                canUpdate: null,
                fillNewValues: (so) =>
                {
                    so.Name = systemObject.Name;
                });            
        }

        public virtual async Task<ManagerResult> DeleteAsync(int id)
        {
            return await DataUtils.DeleteAsync(id: id,
                store: GetSystemObjectStore());
        }

        public ResultSet<TSystemObject> GetSystemObjects(int page, int pageSize)
        {
            var qSystemObjects = GetSystemObjectStore().GetQueryableSystemObjects();

            return ResultSetHelper.GetResults<TSystemObject, int>(qSystemObjects, page, pageSize);
        }
    }
}
