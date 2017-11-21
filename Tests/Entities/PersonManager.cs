using CoreLibrary;
using System;
using System.Threading.Tasks;

namespace Tests.Entities
{
    public class PersonManager<TPerson> : ManagerBase<TPerson, int>
        where TPerson : class, IPerson
    {
        public PersonManager(IPersonStore<TPerson> store) : base(store)
        {
        }

        protected IPersonStore<TPerson> GetPersonStore()
        {
            return (IPersonStore<TPerson>)Store;
        }

        /// <summary>
        /// In the old way, the ManagerBase class had a CreateAsync(T entity) function that we could use as-is. That's great.
        /// However, we found out that there are times where creating an entity may not make sense. So, we always had to override
        /// the CreateAsync to throw a NotSupportedException from it to "disable" it, which was quite goofy. In our new approach,
        /// if we want to be able to create an entity, we will specifically create a CreateAsync function. But now, we have the CreateAsync
        /// routine in a separate class that we could call up instead!
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task<ManagerResult> CreateAsync(TPerson person)
        {
            /* We can perform logic checks here to make sure that the incoming person has legit values.
             * We can even look against other entities here (as the store provides access to them)
             * to se if the values of the incoming person don't violate any we've ever set.
             */

            /* We then call the DataRoutines' CreateAsync function to do the job.
             */
            return await DataRoutines.CreateAsync(person, GetPersonStore(), async (p) =>
            {
                return await GetPersonStore().FindAsync(p.FirstName, p.LastName);
            });
        }
        
        public override void OnUpdatePropertyValues(TPerson original, TPerson entityWithNewValues)
        {
            throw new NotImplementedException();
        }
    }
}
