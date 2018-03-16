using FCore.Foundations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCore.Interactions.Tagging
{
    public interface ITaggingStore<TTagging, TTag> : IAsyncStore<TTagging, long>
        where TTagging : class, ITagging<TTag>
        where TTag : class, ITag
    {
        Task<TTagging> FindAsync(int systemObjectId, string recordId, long tagId, string taggerId);
        IQueryable<TTagging> GetQueryableTaggings();
        IQueryable<ISystemObject> GetQueryableSystemObjects();
    }
}
