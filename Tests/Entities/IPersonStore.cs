using CoreLibrary;
using System.Threading.Tasks;

namespace Tests.Entities
{
    public interface IPersonStore<TPerson> : IAsyncStore<TPerson, int>
        where TPerson : class, IPerson
    {
        Task<TPerson> FindAsync(string firstName, string lastName);
    }
}
