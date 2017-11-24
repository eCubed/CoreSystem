using CoreSystem;
using CoreSystem.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public static class ContactsTests
    {
        public static void CreateContactTest(ContactManager<Contact> contactManager)
        {
            SaveContactViewModel scvm = new SaveContactViewModel()
            {
                FirstName = "Amanda",
                LastName = "Surgens",
                Address1 = "12 Huckleberry Ln",
                City = "Bridgewater",
                Region = "MA",
                PostalCode = "03157",
                Country = "US",
                Email = "amanda@bwstate.edu",
                Phone = "508-768-9771"
            };

            var createRes = contactManager.CreateAsync(scvm, userId: 1).Result;

            var dummy = 3;
        }

        public static void GetAndUpdateTest(ContactManager<Contact> contactManager)
        {
            Contact contact = contactManager.FindByIdAsync(10).Result;
            contact.Address1 = "227 Oak Hill Avenue";
            contact.City = "Attleboro";
            contact.Phone = "508-222-7297";

            SaveContactViewModel scvm = new SaveContactViewModel(contact);

            var updateRes = contactManager.UpdateAsync(scvm, userId: 2).Result;

            var dummy = 3;
        }
    }
}
