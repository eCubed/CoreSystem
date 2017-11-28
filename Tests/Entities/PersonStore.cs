using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Entities
{
    public class PersonStore : IPersonStore<Person>
    {
        private List<Person> People { get; set; }

        public PersonStore(List<Person> people)
        {
            People = people;
        }

        public async Task CreateAsync(Person entity)
        {
            People.Add(entity);

            await Task.FromResult(true);
        }

        public async Task DeleteAsync(int id)
        {
            Person person = People.SingleOrDefault(p => p.Id == id);

            if (person != null)
                People.Remove(person);

            await Task.FromResult(true);
        }

        public async Task DeleteAsync(Person entity)
        {
            if (People.Contains(entity))
                People.Remove(entity);

            await Task.FromResult(true);
        }

        public async Task<Person> FindByIdAsync(int id)
        {
            return await Task.FromResult<Person>(People.SingleOrDefault(p => p.Id == id));
        }

        public async Task UpdateAsync(Person entity)
        {
            Person person = await FindByIdAsync(entity.Id);

            if (person != null)
            {
                person.FirstName = entity.FirstName;
                person.LastName = entity.LastName;
            }                
        }

        public async Task<Person> FindAsync(string firstName, string lastName)
        {
            return await Task.FromResult<Person>(People.SingleOrDefault(p => p.FirstName == firstName && p.LastName == lastName));
        }
    }
}
