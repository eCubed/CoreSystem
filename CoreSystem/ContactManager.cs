using CoreLibrary;
using System;
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
        
        public virtual async Task<ManagerResult> CreateAsync(SaveContactViewModel scvm, int userId)
        {
            TContact newContact = new TContact();
            scvm.UpdateValues(newContact);
            newContact.UserId = userId;

            // To do: save the id of the contact back to the scvm!
            var res = await DataUtils.CreateAsync(newContact, GetContactStore(), FindUniqueAsync);

            if (!res.Success)
                return res;

            scvm.Id = newContact.Id;

            return new ManagerResult();
        }

        #endregion

        #region Update
        
        public virtual async Task<ManagerResult> UpdateAsync(SaveContactViewModel scvm, int userId)
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

        public ResultSet<ContactListItemViewModel> SearchContacts(string startsWith, int page = 1, int pageSize = 10)
        {
            IQueryable<TContact> qContacts = GetContactStore().GetQueryableContacts();

            if (!string.IsNullOrEmpty(startsWith) && (startsWith != "*"))
                qContacts = qContacts.Where(c => c.FirstName.StartsWith(startsWith) || c.LastName.StartsWith(startsWith));

            return ResultSetHelper.Convert(ResultSetHelper.GetResults<TContact, int>(qContacts, page, pageSize), contact =>
            {
                return new ContactListItemViewModel(contact);
            });
        }

        public async Task<ManagerResult<SaveContactViewModel>> GetContactAsync(int id, int requestorId)
        {
            TContact contact = await FindByIdAsync(id);

            if (contact == null)
                return new ManagerResult<SaveContactViewModel>(ManagerErrors.RecordNotFound);

            if (contact.UserId != requestorId)
                return new ManagerResult<SaveContactViewModel>(ManagerErrors.Unauthorized);

            return new ManagerResult<SaveContactViewModel>(new SaveContactViewModel(contact));
        }

        #endregion
    }
}
