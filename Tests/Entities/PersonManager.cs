using CoreLibrary;
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
        /// In the old way, we had a virtual FindUniqueAsync function right at the ManagerBase. Though virtual, it automatically
        /// returned an exception, and that exception was read by the old CreateAsync and UpdateAsync routines as a bypass, that is,
        /// uniqueness was not going to be checked against. We always had to override this function when our entity had uniqueness
        /// rules.
        /// 
        /// We now have to explicitly create a find unique function if we truly have uniqueness-by-property-values rules, and then pass
        /// that to the utility CreateAsync and UpdateAsync functions that we call from this manager's own CreateAsync and UpdateAsync
        /// functions.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        protected async Task<TPerson> FindUniqueAsync(TPerson person)
        {
            return await GetPersonStore().FindAsync(person.FirstName, person.LastName);
        }

        /// <summary>
        /// In the old way, the ManagerBase class had a CreateAsync(T entity) function that we could use as-is. That's great.
        /// However, we found out that there are times where creating an entity may not make sense. So, we always had to override
        /// the CreateAsync to throw a NotSupportedException from it to "disable" it, which was quite goofy. In our new approach,
        /// if we want to be able to create an entity, we will specifically create a CreateAsync function. But now, we have the CreateAsync
        /// routine in a separate class that we could call up instead from inside our manager's CreateAsync function!
        /// 
        /// We could also create a custom CreateAsync function that takes in a view model instead, and possibly additional parameters.
        /// In this CreateAsync variant that we could write, we will need to convert that view model to the actual entity's type so
        /// we can pass it to the utility CreateAsync function. 
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task<ManagerResult> CreateAsync(TPerson person)
        {
            /* We can perform logic checks here to make sure that the incoming person has legit values.
             * We can even look against other entities here (as the store provides access to them)
             * to se if the values of the incoming person don't violate any we've ever set.
             * If we have decided that the incoming person cannot be added, then we immediately return
             * a ManagerResult with the appropriate message.
             */

            /* We then call the utility CreateAsync function to do the job.
             */
            return await DataUtils.CreateAsync(person, GetPersonStore(), FindUniqueAsync);
        }

        /// <summary>
        /// In the old way, we automatically assumed that every entity's record ought to be deleted, which was why we put the delete routine
        /// right onto the ManagerBase. As with not wanting to create entities, there are also times that records of an entity type should not
        /// be deleted!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManagerResult> DeleteAsync(int id)
        {
            /* At this point, we can check whether the record can be deleted.
             * If not, then we would throw a ManagerResult with the appropriate
             * error message.
            */
            
            /* We then call up the utility DeleteAsync function to finally delete the entity.
             */
            return await DataUtils.DeleteAsync(id, GetPersonStore());
        }

        /// <summary>
        /// In the old way, we automatically assumed that every entity's record ought to be updated, which was why we put the update routine
        /// right on to the ManagerBase. Also, our update routine was very complicated and at times, resulted in setting values twice and looked up
        /// the entity by id twice, which was very inefficient and even cumbersome to implement.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ManagerResult> UpdateAsync(TPerson entity)
        {
            /* We would perform any validation (besides uniqueness, since the update routine takes care of it) here to ensure that the
             * incoming entity can be updated.
             */

            return await DataUtils.UpdateAsync(entity, GetPersonStore(), FindUniqueAsync);
        }
        
    }
}
