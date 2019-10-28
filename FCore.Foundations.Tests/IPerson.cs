namespace FCore.Foundations.Tests
{
    public interface IPerson : IIdentifiable<int>
    {        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }
    }
}
