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

        public async override Task<TContact> FindUniqueAsync(TContact matchAgainst)
        {
            return await GetContactStore().FindContactAsync(matchAgainst.FirstName, matchAgainst.LastName, matchAgainst.UserId);
        }

        #region Create

        public override Task<ManagerResult> CreateAsync(TContact entity)
        {
            throw new NotSupportedException();
        }
        
        public virtual async Task<ManagerResult> CreateAsync(SaveContactViewModel scvm, int userId)
        {
            TContact newContact = new TContact();
            scvm.UpdateValues(newContact);
            newContact.UserId = userId;

            // To do: save the id of the contact back to the scvm!
            var res = await base.CreateAsync(newContact);

            if (!res.Success)
                return res;

            scvm.Id = newContact.Id;

            return new ManagerResult();

        }

        #endregion

        #region Update

        public override Task<ManagerResult> UpdateAsync(TContact entity)
        {
            throw new NotSupportedException();
        }

        public virtual async Task<ManagerResult> UpdateAsync(SaveContactViewModel scvm, int userId)
        {
            if (!scvm.Id.HasValue)
                return new ManagerResult(ManagerErrors.IdNotSpecified);

            TContact contact = await FindByIdAsync(scvm.Id.Value);

            if (contact == null)
                return new ManagerResult(ManagerErrors.RecordNotFound);

            if (contact.UserId != userId)
                return new ManagerResult(ManagerErrors.Unauthorized);

            scvm.UpdateValues(contact);

            return await base.UpdateAsync(contact);
        }

        public override void OnUpdatePropertyValues(TContact original, TContact entityWithNewValues)
        {
            original.Address1 = entityWithNewValues.Address1;
            original.Address2 = entityWithNewValues.Address2;
            original.City = entityWithNewValues.City;
            original.Country = entityWithNewValues.Country;
            original.Email = entityWithNewValues.Email;
            original.FirstName = entityWithNewValues.FirstName;
            original.LastName = entityWithNewValues.LastName;
            original.Phone = entityWithNewValues.Phone;
            original.PostalCode = entityWithNewValues.PostalCode;
            original.Region = entityWithNewValues.Region;
        }

        #endregion

        #region Delete

        public override Task<ManagerResult> DeleteAsync(int id)
        {
            throw new NotSupportedException();
        }

        public async Task<ManagerResult> DeleteAsync(int id, int userId)
        {
            TContact contact = await FindByIdAsync(id);

            if (contact == null)
                return new ManagerResult(ManagerErrors.RecordNotFound);

            if (contact.UserId != userId)
                return new ManagerResult(ManagerErrors.Unauthorized);

            return await base.DeleteAsync(id);
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
