using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCore.Foundations.Tests
{
    public class PeopleStore : IAsyncStore<Person, int>, IPersonStore<Person>
    {
        private List<Person> People { get; set; }

        public PeopleStore()
        {
            People = new List<Person>
            {
                new Person() { Id = 1, FirstName = "Elvira", LastName = "Caylaluad", Age = 60 },
                new Person() { Id = 2, FirstName = "Jocelyn", LastName = "Villar", Age = 60 },
                new Person() { Id = 3, FirstName = "Edna", LastName = "Cabaya", Age = 60 },
                new Person() { Id = 4, FirstName = "Maria", LastName = "Santos", Age = 60 },
                new Person() { Id = 5, FirstName = "Rufina", LastName = "Francisco", Age = 60 },
                new Person() { Id = 6, FirstName = "Tammy", LastName = "Blackwell", Age = 60 },
                new Person() { Id = 7, FirstName = "Robert", LastName = "Dean", Age = 60 },
                new Person() { Id = 8, FirstName = "Cheryl", LastName = "Carter", Age = 60 },
                new Person() { Id = 9, FirstName = "John", LastName = "Jepson", Age = 60 },
                new Person() { Id = 10, FirstName = "Katherine", LastName = "Kressley", Age = 60 },
                new Person() { Id = 11, FirstName = "Lawrence", LastName = "Walinski", Age = 60 },
                new Person() { Id = 12, FirstName = "Marcie", LastName = "Chamberlain", Age = 60 },
                new Person() { Id = 13, FirstName = "Peter", LastName = "Pereira", Age = 60 },
            };
        }
        public async Task CreateAsync(Person entity)
        {
            entity.Id = People.Max(p => p.Id) + 1;
            People.Add(entity);

            await Task.FromResult(0);
        }

        public async Task DeleteAsync(int id)
        {
            Person person = await FindByIdAsync(id);

            if (person != null)
            {
                People.Remove(person);
            }
        }

        public async Task DeleteAsync(Person entity)
        {
            await DeleteAsync(entity.Id);
        }

        public async Task<Person> FindByIdAsync(int id)
        {
            Person person = People.SingleOrDefault(p => p.Id == id);

            return await Task.FromResult(person);
        }

        public async Task UpdateAsync(Person entity)
        {
            Person master = await FindByIdAsync(entity.Id);

            if (master != null)
            {
                master.Age = entity.Age;
                master.FirstName = entity.FirstName;
                entity.LastName = entity.LastName;
            }
        }

        public IQueryable<Person> GetQueryablePeople()
        {
            return People.AsQueryable();
        }

        public async Task<Person> FindAsync(string firstName, string lastName)
        {
            Person person = People.SingleOrDefault(p => p.FirstName == firstName & p.LastName == lastName);

            return await Task.FromResult(person);
        }
    }
}
