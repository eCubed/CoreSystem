using System.Collections.Generic;

namespace Tests.Entities
{
    public static class DataSource
    {
        public static List<Person> People = new List<Person>
        {
            new Person { Id = 1, FirstName = "John", LastName = "Smith"},
            new Person { Id = 2, FirstName = "Susan", LastName = "Sample"}
        };
    }
}
