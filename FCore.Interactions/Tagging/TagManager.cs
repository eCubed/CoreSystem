using FCore.Foundations;
using System.Threading.Tasks;

namespace FCore.Interactions.Tagging
{
    public class TagManager<TTag> : ManagerBase<TTag, long>
        where TTag : class, ITag
    {
        public TagManager(ITagStore<TTag> store) : base(store)
        {
        }

        protected ITagStore<TTag> GetTagStore()
        {
            return (ITagStore<TTag>)Store;
        }

        protected async Task<TTag> FindAsync(TTag systemObject)
        {
            return await GetTagStore().FindAsync(systemObject.Name);
        }

        public virtual async Task<ManagerResult> CreateAsync(TTag systemObject)
        {
            return await DataUtils.CreateAsync(entity: systemObject,
                store: GetTagStore(),
                findUniqueAsync: FindAsync);
        }

        public virtual async Task<ManagerResult> UpdateAsync(TTag systemObject)
        {
            return await DataUtils.UpdateAsync(id: systemObject.Id,
                store: GetTagStore(),
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
                store: GetTagStore());
        }

        public ResultSet<TTag> GetTags(int page, int pageSize)
        {
            var qTags = GetTagStore().GetQueryableTags();

            return ResultSetHelper.GetResults<TTag, int>(qTags, page, pageSize);
        }
    }
}
