using CoreLibrary;
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
                FirstName = "Richard",
                LastName = "LaCivita",
                Address1 = "1427 Augsburger Drive",
                City = "Attleboro",
                Region = "MA",
                PostalCode = "02703",
                Country = "US",
                Email = "rlacivita@attleboro.gov",
                Phone = "508-222-0101"
            };

            var createRes = contactManager.CreateAsync(scvm, userId: 1).Result;

            var dummy = 3;
        }

        public static void GetAndUpdateTest(ContactManager<Contact> contactManager)
        {
            /*
            Contact contact = contactManager.FindByIdAsync(15).Result;
            contact.Address1 = "3527 Locust St";
            contact.City = "Attleboro";
            contact.FirstName = "Robert";

            SaveContactViewModel<Contact> scvm = new SaveContactViewModel<Contact>(contact);
            */

            var getRes = contactManager.GetContactAsync(15, requestorId: 2).Result;

            if (getRes.Success)
            {
                SaveContactViewModel<Contact> scvm = getRes.Data;
                scvm.FirstName = "Roberto";
                scvm.Address1 = "1000 Park Street";

                var updateRes = contactManager.UpdateAsync(scvm, userId: 1).Result;
            }

            var dummy = 3;
        }

        public static void GetManyTest(ContactManager<Contact> contactManager)
        {
            ResultSet<ContactListItemViewModel<Contact>> contacts = contactManager.SearchContacts("*", 1, 12);

            var dummy = 12;
        }
    }
}
