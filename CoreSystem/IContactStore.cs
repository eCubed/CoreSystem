using CoreLibrary;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSystem
{
    public interface IContactStore<TContact> : IAsyncStore<TContact, int>
        where TContact : class, IContact
    {
        Task<TContact> FindContactAsync(string firstName, string lastName, int userId);
        IQueryable<TContact> GetQueryableContacts();
    }
}
