using System;

namespace FCore.Foundations.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            PersonManager<Person> PersonManager = new PersonManager<Person>(new PeopleStore());

            // CreateTestNotDuplicate(PersonManager);
            CreateTestDuplicate(PersonManager);
            // UpdateDuplicate(PersonManager);

            Console.ReadLine();
        }

        private static void UpdateDuplicate(PersonManager<Person> personManager)
        {
            SavePersonViewModel<Person> personViewModel = new SavePersonViewModel<Person>
            {
                Age = 41,
                FirstName = "Rufina",
                LastName = "Francisco"
            };

            var res = personManager.UpdateAsync(1, personViewModel).Result;

            // Should disallow updating if going to duplicate another existing record... PASS
            var dummy = 1;
        }

        private static void UpdateNotDuplicate(PersonManager<Person> personManager)
        {
            SavePersonViewModel<Person> personViewModel = new SavePersonViewModel<Person>
            {
                Age = 41,
                FirstName = "Elvira",
                LastName = "Paningbatan"
            };

            var res = personManager.UpdateAsync(1, personViewModel).Result;

            // Should allow updating if not going to duplicate another existing record... PASS
        }

        private static void CreateTestDuplicate(PersonManager<Person> personManager)
        {
            SavePersonViewModel<Person> personViewModel = new SavePersonViewModel<Person>
            {
                Age = 41,
                FirstName = "Elvira",
                LastName = "Caylaluad"
            };

            var res = personManager.CreateAsync(personViewModel).Result;

            // Should have denied creation.... Pass
            var dummy = 0;
        }

        private static void CreateTestNotDuplicate(PersonManager<Person> personManager)
        {
            SavePersonViewModel<Person> personViewModel = new SavePersonViewModel<Person>
            {
                Age = 41,
                FirstName = "Israel",
                LastName = "Fernando"
            };

            var res = personManager.CreateAsync(personViewModel).Result;

            // Should have added the person who is surely not a duplicate... PASS
            var dummy = 0;
        }
    }
}
