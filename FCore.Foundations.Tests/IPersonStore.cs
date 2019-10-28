using System.Linq;
using System.Threading.Tasks;

namespace FCore.Foundations.Tests
{
    public interface IPersonStore<TPerson> : IAsyncStore<TPerson, int>
        where TPerson : class, IPerson
    {
        IQueryable<TPerson> GetQueryablePeople();
        Task<TPerson> FindAsync(string firstName, string lastName);
    }
}
