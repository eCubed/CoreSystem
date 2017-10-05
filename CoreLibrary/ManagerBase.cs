using System;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public abstract class ManagerBase<T, TKey>
        where T : class, IIdentifiable<TKey>
    {
        protected IAsyncStore<T, TKey> Store { get; set; }

        public ManagerBase(IAsyncStore<T, TKey> store)
        {
            Store = store;
        }

        public virtual async Task<T> FindByIdAsync(TKey id)
        {
            return await Store.FindByIdAsync(id);
        }

        public virtual Task<T> FindUniqueAsync(T matchAgainst)
        {
            throw new NotImplementedException();
        }

        public virtual ManagerResult OnCreateLogicCheck(T entity)
        {
            return new ManagerResult();
        }

        public virtual async Task<ManagerResult> CreateAsync(T entity)
        {
            try
            {
                T duplicate = await FindUniqueAsync(entity);

                if (duplicate != null)
                    return new ManagerResult(ManagerErrors.DuplicateOnCreate);

                ManagerResult logicCheckResult = OnCreateLogicCheck(entity);

                if (!logicCheckResult.Success)
                    return logicCheckResult;

                await Store.CreateAsync(entity);
            }
            catch (NotImplementedException)
            {
                ManagerResult logicCheckResult = OnCreateLogicCheck(entity);

                if (!logicCheckResult.Success)
                    return logicCheckResult;

                await Store.CreateAsync(entity);
            }
            catch (Exception e)
            {
                return e.CreateManagerResult();
            }

            return new ManagerResult();
        }

        /// <summary>
        /// This function is called only after the record to update was found
        /// AND there is no duplicate violation.
        /// </summary>
        /// <param name="entity">The incoming object that contains the new values.</param>
        /// <returns></returns>
        public virtual ManagerResult OnUpdateLogicCheck(T entity)
        {
            return new ManagerResult();
        }

        /// <summary>
        /// Function is called after all logic for updating is checked, right before
        /// handing over the original (then with the updated values) to the store.
        /// This is set virtual because not all managers will require editing (though
        /// most will).
        /// </summary>
        /// <param name="original"></param>
        /// <param name="entityWithNewValues"></param>
        public abstract void OnUpdatePropertyValues(T original, T entityWithNewValues);

        public virtual async Task<ManagerResult> UpdateAsync(T entity)
        {
            T recordToUpdate = await FindByIdAsync(entity.Id);

            if (recordToUpdate == null)
                return new ManagerResult(ManagerErrors.RecordNotFound);

            try
            {
                T possibleDuplicate = await FindUniqueAsync(entity);
                
                if ((possibleDuplicate != null) && (!possibleDuplicate.Id.Equals(recordToUpdate.Id)))
                    return new ManagerResult(ManagerErrors.DuplicateOnUpdate);

                ManagerResult logicCheckResult = OnUpdateLogicCheck(entity);

                if (!logicCheckResult.Success)
                    return logicCheckResult;

                OnUpdatePropertyValues(recordToUpdate, entity);

                await Store.UpdateAsync(recordToUpdate);
            }
            catch (NotImplementedException)
            {
                ManagerResult logicCheckResult = OnUpdateLogicCheck(entity);

                if (!logicCheckResult.Success)
                    return logicCheckResult;

                OnUpdatePropertyValues(recordToUpdate, entity);

                await Store.UpdateAsync(recordToUpdate);
            }
            catch (Exception e)
            {
                return e.CreateManagerResult();
            }

            return new ManagerResult();
        }

        public virtual ManagerResult OnDeleteLogicCheck(T entity)
        {
            return new ManagerResult();
        }

        public virtual async Task<ManagerResult> DeleteAsync(TKey id)
        {
            try
            {
                T found = await Store.FindByIdAsync(id);

                if (found == null)
                    return new ManagerResult(ManagerErrors.RecordNotFound);

                ManagerResult logicCheckResult = OnDeleteLogicCheck(found);

                if (!logicCheckResult.Success)
                    return logicCheckResult;

                await Store.DeleteAsync(found);
            }
            catch (Exception e)
            {
                return e.CreateManagerResult();
            }

            return new ManagerResult();
        }

        public virtual async Task<ManagerResult> DeleteAsync(T entity)
        {
            return await DeleteAsync(entity.Id);
        }
    }
}
