using CoreLibrary;
using Newtonsoft.Json;

namespace CoreSystem
{
    public class SaveContactViewModel<TContact> : IViewModel<TContact, int>
        where TContact : class, IContact
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        public SaveContactViewModel()
        {
        }

        public SaveContactViewModel(TContact contact)
        {
            SetValues(contact);
        }

        public void UpdateValues(TContact contact)
        {
            contact.Address1 = Address1;
            contact.Address2 = Address2;
            contact.City = City;
            contact.Country = Country;
            contact.Email = Email;
            contact.FirstName = FirstName;
            contact.LastName = LastName;
            contact.Phone = Phone;
            contact.PostalCode = PostalCode;
            contact.Region = Region;
            contact.UserId = UserId;
        }

        public void SetValues(TContact contact, bool uniqueIdentifierOnly = false)
        {
            Id = contact.Id;

            if (!uniqueIdentifierOnly)
            {
                Address1 = contact.Address1;
                Address2 = contact.Address2;
                City = contact.City;
                Country = contact.Country;
                Email = contact.Email;
                FirstName = contact.FirstName;
                LastName = contact.LastName;
                Phone = contact.Phone;
                PostalCode = contact.PostalCode;
                Region = contact.Region;
                UserId = contact.UserId;
            }
        }

        public int GetUniqueIdentifier()
        {
            return this.Id.Value;
        }
    }
}
