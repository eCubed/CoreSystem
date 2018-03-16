using FCore.Foundations;
using System.Linq;
using System.Threading.Tasks;

namespace FCore.Interactions.Tagging
{
    public interface ITagStore<TTag> : IAsyncStore<TTag, long>
        where TTag : class, ITag
    {
        Task<TTag> FindAsync(string name);
        IQueryable<TTag> GetQueryableTags();
    }
}
