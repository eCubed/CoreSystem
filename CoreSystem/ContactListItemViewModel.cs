using CoreLibrary;

namespace CoreSystem
{
    public class ContactListItemViewModel<TContact> : IViewModel<TContact, int>
        where TContact : class, IContact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        public ContactListItemViewModel()
        {
        }

        public ContactListItemViewModel(TContact contact)
        {
            SetValues(contact);
        }

        public void UpdateValues(TContact data)
        {
            throw new System.NotImplementedException();
        }

        public void SetValues(TContact contact)
        {
            Id = contact.Id;
            FirstName = contact.FirstName;
            LastName = contact.LastName;
            Phone = contact.Phone;
        }

        public int GetUniqueIdentifier()
        {
            return Id;
        }
    }
}
