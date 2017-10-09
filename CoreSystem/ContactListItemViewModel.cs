namespace CoreSystem
{
    public class ContactListItemViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        public ContactListItemViewModel()
        {
        }

        public ContactListItemViewModel(IContact contact)
        {
            Id = contact.Id;
            FirstName = contact.FirstName;
            LastName = contact.LastName;
            Phone = contact.Phone;
        }
    }
}
