using System;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public static class DataUtils
    {
        public static async Task<ManagerResult> CreateAsync<T, TKey>(T entity, IAsyncStore<T, TKey> store,
            Func<T, Task<T>> findUniqueAsync = null)
            where T : class, IIdentifiable<TKey>
        {
            if (findUniqueAsync != null)
            {
                T duplicate = await findUniqueAsync.Invoke(entity);

                if (duplicate != null)
                    return new ManagerResult(ManagerErrors.DuplicateOnCreate);
            }

            await store.CreateAsync(entity);           

            return new ManagerResult();
        }

        public static async Task<ManagerResult> DeleteAsync<T, TKey>(TKey id, IAsyncStore<T, TKey> store)
             where T : class, IIdentifiable<TKey>
        {
            try
            {
                T found = await store.FindByIdAsync(id);

                if (found == null)
                    return new ManagerResult(ManagerErrors.RecordNotFound);
                

                await store.DeleteAsync(found);
            }
            catch (Exception e)
            {
                return e.CreateManagerResult();
            }

            return new ManagerResult();
        }

        public static async Task<ManagerResult> UpdateAsync<T, TKey>(T entity, IAsyncStore<T, TKey> store,
            Func<T, Task<T>> findUniqueAsync = null)
            where T : class, IIdentifiable<TKey>
        {
            T recordToUpdate = await store.FindByIdAsync(entity.Id);

            if (recordToUpdate == null)
                return new ManagerResult(ManagerErrors.RecordNotFound);

            if (findUniqueAsync != null)
            {
                T possibleDuplicate = await findUniqueAsync(entity);

                if ((possibleDuplicate != null) && (!possibleDuplicate.Id.Equals(recordToUpdate.Id)))
                    return new ManagerResult(ManagerErrors.DuplicateOnUpdate);
            }                
                
            await store.UpdateAsync(recordToUpdate);
           
            return new ManagerResult();
        }
    }
}
