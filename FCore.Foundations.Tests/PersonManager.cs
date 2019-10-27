using System.Threading.Tasks;

namespace FCore.Foundations.Tests
{
    public class PersonManager<TPerson> : ManagerBase<TPerson, int>
        where TPerson : class, IPerson, new()
    {
        public PersonManager(IPersonStore<TPerson> store) : base(store)
        {
        }

        private IPersonStore<TPerson> GetPersonStore()
        {
            return (IPersonStore<TPerson>)Store;
        }
        
        private async Task<TPerson> FindUniqueAsync(SavePersonViewModel<TPerson> personViewModel)
        {
            return await GetPersonStore().FindAsync(personViewModel.FirstName, personViewModel.LastName);
        }

        public async Task<ManagerResult<int>> CreateAsync(SavePersonViewModel<TPerson> personViewModel)
        {
            return await DataUtils.CreateAsync(
                viewModel: personViewModel,
                store: GetPersonStore(),
                findUniqueAsync: FindUniqueAsync);
        }

        public async Task<ManagerResult> UpdateAsync(int id, SavePersonViewModel<TPerson> personViewModel)
        {
            return await DataUtils.UpdateAsync(
                id: id,
                viewModel: personViewModel,
                store: GetPersonStore(),
                findUniqueAsync: FindUniqueAsync);
        }

    }
}
