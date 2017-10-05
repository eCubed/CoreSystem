using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreSystem.EntityFramework
{
    public class Contact : IContact
    {
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

        public int UserId { get; set; }
        public virtual CoreSystemUser User { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }

        public int Id { get; set; }
    }
}
