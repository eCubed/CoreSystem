using CoreSystem;
using CoreSystem.EntityFramework;

namespace Tests
{
    public static class ContactsTests
    {
        public static void CreateContactTest(ContactManager<Contact> contactManager)
        {
            SaveContactViewModel<Contact> scvm = new SaveContactViewModel<Contact>()
            {
                FirstName = "Rob",
                LastName = "Geddes",
                Address1 = "120 Maple Street",
                City = "Attleboro",
                Region = "MA",
                PostalCode = "02703",
                Country = "US",
                Email = "rgeddes@attleboro.gov",
                Phone = "508-222-1005"
            };

            var createRes = contactManager.CreateAsync(scvm, userId: 1).Result;

            var dummy = 3;
        }

        public static void GetAndUpdateTest(ContactManager<Contact> contactManager)
        {
            Contact contact = contactManager.FindByIdAsync(15).Result;
            contact.Address1 = "1527 Locust St";
            contact.City = "Attleboro";
            contact.FirstName = "Amanda";
            contact.LastName = "Surgens";

            SaveContactViewModel<Contact> scvm = new SaveContactViewModel<Contact>(contact);

            var updateRes = contactManager.UpdateAsync(scvm, userId: 1).Result;

            var dummy = 3;
        }
    }
}
