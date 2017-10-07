namespace CoreSystem
{
    public class ContactListItemViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        public ContactListItemViewModel()
        {
        }

        public ContactListItemViewModel(IContact contact)
        {
            FirstName = contact.FirstName;
            LastName = contact.LastName;
            Phone = contact.Phone;
        }
    }
}
