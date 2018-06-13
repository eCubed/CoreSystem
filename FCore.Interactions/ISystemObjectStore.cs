using FCore.Foundations;
using System.Linq;
using System.Threading.Tasks;

namespace FCore.Interactions
{
    public interface ISystemObjectStore<TSystemObject> : IAsyncStore<TSystemObject, int>
        where TSystemObject : class, ISystemObject
    {
        Task<TSystemObject> FindAsync(string name);
        IQueryable<TSystemObject> GetQueryableSystemObjects();
    }
}
