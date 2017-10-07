using CoreLibrary;
using System;

namespace CoreSystem
{
    public interface IContact : IIdentifiable<int>
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        string Region { get; set; }
        string Country { get; set; }
        string PostalCode { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        int UserId { get; set; }
        DateTime? CreatedDate { get; set; }
    }
}
