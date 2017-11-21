namespace Tests.Entities
{
    public static class EntitiesTests
    {
        public static void CreateTest()
        {
            PersonManager<Person> personManager = new PersonManager<Person>(new PersonStore(DataSource.People));

            var res = personManager.CreateAsync(new Person { Id = 3, FirstName = "Susan", LastName = "Sample" }).Result;

            var peopleAfterAdding = DataSource.People;

            var dummy = 9;

        }

        public static void DeleteTest()
        {
            PersonManager<Person> personManager = new PersonManager<Person>(new PersonStore(DataSource.People));

            var res = personManager.DeleteAsync(2).Result;

            var peopleAfterAdding = DataSource.People;

            var dummy = 9;
        }

        public static void UpdateTest()
        {
            PersonManager<Person> personManager = new PersonManager<Person>(new PersonStore(DataSource.People));

            Person person = personManager.FindByIdAsync(2).Result;
            person.FirstName = "Eric";
            person.LastName = "Smith";

            var res = personManager.UpdateAsync(person).Result;

            var peopleAfterAdding = DataSource.People;

            var dummy = 2;
        }
    }
}
