using System;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public static class DataUtils
    {
        public static async Task<ManagerResult> CreateAsync<T, TKey>(T entity, IAsyncStore<T, TKey> store,
            Func<T, Task<T>> findUniqueAsync = null, Func<T, ManagerResult> canCreate = null)
            where T : class, IIdentifiable<TKey>
        {
            if (findUniqueAsync != null)
            {
                T duplicate = await findUniqueAsync.Invoke(entity);

                if (duplicate != null)
                    return new ManagerResult(ManagerErrors.DuplicateOnCreate);
            }

            if (canCreate != null)
            {
                var createRes = canCreate.Invoke(entity);

                if (!createRes.Success)
                    return createRes;
            }

            await store.CreateAsync(entity);           

            return new ManagerResult();
        }

        public static async Task<ManagerResult> DeleteAsync<T, TKey>(TKey id, IAsyncStore<T, TKey> store,
            Func<T, ManagerResult> canDelete = null)
             where T : class, IIdentifiable<TKey>
        {
            try
            {
                T found = await store.FindByIdAsync(id);

                if (found == null)
                    return new ManagerResult(ManagerErrors.RecordNotFound);
                
                if (canDelete != null)
                {
                    var canDeleteRes = canDelete.Invoke(found);

                    if (!canDeleteRes.Success)
                        return canDeleteRes;
                }

                await store.DeleteAsync(found);
            }
            catch (Exception e)
            {
                return e.CreateManagerResult();
            }

            return new ManagerResult();
        }

        public static async Task<ManagerResult> UpdateAsync<T, TKey>(TKey id, IAsyncStore<T, TKey> store,
            Func<T, Task<T>> findUniqueAsync = null, Func<T, ManagerResult> canUpdate = null,
            Action<T> fillNewValues = null)
            where T : class, IIdentifiable<TKey>
        {
            T recordToUpdate = await store.FindByIdAsync(id);

            if (recordToUpdate == null)
                return new ManagerResult(ManagerErrors.RecordNotFound);

            if (findUniqueAsync != null)
            {
                T possibleDuplicate = await findUniqueAsync(recordToUpdate);

                if ((possibleDuplicate != null) && (!possibleDuplicate.Id.Equals(recordToUpdate.Id)))
                    return new ManagerResult(ManagerErrors.DuplicateOnUpdate);
            }

            if (canUpdate != null)
            {
                var canUpdateRes = canUpdate.Invoke(recordToUpdate);

                if (!canUpdateRes.Success)
                    return canUpdateRes;
            }

            // Now, we allow updating of the values!
            if (fillNewValues != null)
                fillNewValues.Invoke(recordToUpdate);
                
            await store.UpdateAsync(recordToUpdate);
           
            return new ManagerResult();
        }
    }
}
