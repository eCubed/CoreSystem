using System;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public static class DataRoutines
    {
        public static async Task<ManagerResult> CreateAsync<T, TKey>(T entity, IAsyncStore<T, TKey> store,
            Func<T, Task<T>> findUniqueAsync)
            where T : class, IIdentifiable<TKey>
        {
            try
            {
                T duplicate = await findUniqueAsync.Invoke(entity);

                if (duplicate != null)
                    return new ManagerResult(ManagerErrors.DuplicateOnCreate);

                /*
                ManagerResult logicCheckResult = onCreateLogicCheck.Invoke(entity);

                if (!logicCheckResult.Success)
                    return logicCheckResult;
                */
                await store.CreateAsync(entity);
            }
            catch (NotImplementedException)
            {
                /*
                ManagerResult logicCheckResult = onCreateLogicCheck.Invoke(entity);

                if (!logicCheckResult.Success)
                    return logicCheckResult;
                */
                await store.CreateAsync(entity);
            }
            catch (Exception e)
            {
                return e.CreateManagerResult();
            }

            return new ManagerResult();
        }
    }
}
