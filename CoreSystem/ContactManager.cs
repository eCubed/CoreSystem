using CoreLibrary;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSystem
{
    public class ContactManager<TContact> : ManagerBase<TContact, int>
        where TContact : class, IContact, new()
    {
        public ContactManager(IContactStore<TContact> store) : base(store)
        {
        }

        protected IContactStore<TContact> GetContactStore()
        {
            return (IContactStore<TContact>)Store;
        }

        public async Task<TContact> FindUniqueAsync(TContact matchAgainst)
        {
            return await GetContactStore().FindContactAsync(matchAgainst.FirstName, matchAgainst.LastName, matchAgainst.UserId);
        }

        #region Create
        
        public virtual async Task<ManagerResult> CreateAsync(SaveContactViewModel<TContact> scvm, int userId)
        {
            scvm.UserId = userId;
            return await DataUtils.CreateAsync(scvm, GetContactStore(), FindUniqueAsync);
        }

        #endregion

        #region Update
        
        public virtual async Task<ManagerResult> UpdateAsync(SaveContactViewModel<TContact> scvm, int userId)
        {
            return await DataUtils.UpdateAsync(scvm.Id.Value, GetContactStore(), FindUniqueAsync, contact =>
            {
                if (contact.UserId != userId)
                    return new ManagerResult(ManagerErrors.Unauthorized);

                return new ManagerResult();
            }, contact => scvm.UpdateValues(contact));
        }

        #endregion

        #region Delete
        
        public async Task<ManagerResult> DeleteAsync(int id, int userId)
        {
            return await DataUtils.DeleteAsync(id, GetContactStore(), c =>
            {
                if (c.UserId != userId)
                    return new ManagerResult(ManagerErrors.Unauthorized);

                return new ManagerResult();
            }); // base.DeleteAsync(id);
        }

        #endregion

        #region Get

        public ResultSet<ContactListItemViewModel<TContact>> SearchContacts(string startsWith, int page = 1, int pageSize = 10)
        {
            IQueryable<TContact> qContacts = GetContactStore().GetQueryableContacts();

            if (!string.IsNullOrEmpty(startsWith) && (startsWith != "*"))
                qContacts = qContacts.Where(c => c.FirstName.StartsWith(startsWith) || c.LastName.StartsWith(startsWith));
            
            return DataUtils.GetMany<TContact, int, ContactListItemViewModel<TContact>>(qContacts, page, pageSize);

        }

        /* This function already hardcodes the exact view model we want to return! If we wanted a different view model
         * to return a different set of data, we would create a separate function with nearly the same code, except a
         * different third parameter for the specific view model we want.
         */
        public async Task<ManagerResult<SaveContactViewModel<TContact>>> GetContactAsync(int id, int requestorId)
        {           
            return await DataUtils.GetOneRecordAsync<TContact, int, SaveContactViewModel<TContact>>(id, GetContactStore(),
                canGet: (contact) =>
                {
                    if (contact.UserId != requestorId)
                        return new ManagerResult(ManagerErrors.Unauthorized);
                    else
                        return new ManagerResult();
                });
        }

        #endregion
    }
}
