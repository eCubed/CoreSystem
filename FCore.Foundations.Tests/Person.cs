namespace FCore.Foundations.Tests
{
    public class Person : IPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public int Id { get; set; }
    }
}
