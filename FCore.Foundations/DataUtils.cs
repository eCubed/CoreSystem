using System;
using System.Linq;
using System.Threading.Tasks;

namespace FCore.Foundations
{
    public static class DataUtils
    {

        #region Create
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

        public static async Task<ManagerResult<TKey>> CreateAsync<T, TKey, TViewModel>(TViewModel viewModel, IAsyncStore<T, TKey> store,
            Func<TViewModel, Task<T>> findUniqueAsync = null, Func<T, ManagerResult> canCreate = null, Action<T> setNonViewModelData = null)
            where T : class, IIdentifiable<TKey>, new()
            where TViewModel : class, ISaveViewModel<T>
        {
            T newData = new T();
            viewModel.UpdateEntity(newData);

            if (findUniqueAsync != null)
            {
                T duplicate = await findUniqueAsync.Invoke(viewModel);

                if (duplicate != null)
                    return new ManagerResult<TKey>(ManagerErrors.DuplicateOnCreate);
            }

            if (canCreate != null)
            {
                var createRes = canCreate.Invoke(newData);

                if (!createRes.Success)
                    return new ManagerResult<TKey>(createRes.Errors);
            }

            setNonViewModelData?.Invoke(newData);

            await store.CreateAsync(newData);

            return new ManagerResult<TKey>(newData.Id);
        }

        #endregion Create

        #region Delete
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

        #endregion Delete

        #region Update
        public static async Task<ManagerResult> UpdateAsync<T, TKey, TViewModel>(TKey id, TViewModel viewModel, IAsyncStore<T, TKey> store,
            Func<TViewModel, Task<T>> findUniqueAsync = null, Func<T, ManagerResult> canUpdate = null,
            Action<T> setNonViewModelData = null)
            where T : class, IIdentifiable<TKey>
            where TViewModel : class, ISaveViewModel<T>
        {
            T recordToUpdate = await store.FindByIdAsync(id);

            if (recordToUpdate == null)
                return new ManagerResult(ManagerErrors.RecordNotFound);

            if (findUniqueAsync != null)
            {
                T possibleDuplicate = await findUniqueAsync(viewModel);

                if ((possibleDuplicate != null) && (!possibleDuplicate.Id.Equals(recordToUpdate.Id)))
                    return new ManagerResult(ManagerErrors.DuplicateOnUpdate);
            }

            if (canUpdate != null)
            {
                var canUpdateRes = canUpdate.Invoke(recordToUpdate);

                if (!canUpdateRes.Success)
                    return canUpdateRes;
            }

            viewModel.UpdateEntity(recordToUpdate);
            
            setNonViewModelData?.Invoke(recordToUpdate);
                
            await store.UpdateAsync(recordToUpdate);
           
            return new ManagerResult();
        }

        #endregion Update

        #region Get
        public static async Task<ManagerResult<TViewModel>> GetOneRecordAsync<T, TKey, TViewModel>(TKey id, IAsyncStore<T, TKey> store,
            Func<T, ManagerResult> canGet = null)
            where T : class, IIdentifiable<TKey>, new()
            where TViewModel : class, IGetViewModel<T>, new()
        {
            T data = await store.FindByIdAsync(id);

            if (data == null)
                return new ManagerResult<TViewModel>(ManagerErrors.RecordNotFound);

            if (canGet != null)
            {
                var canGetResult = canGet.Invoke(data);
                if (!canGetResult.Success)
                    return new ManagerResult<TViewModel>(canGetResult.Errors);
            }

            TViewModel viewModel = new TViewModel();
            viewModel.FillViewModel(data);

            return new ManagerResult<TViewModel>(viewModel);
        }

        public static ResultSet<TViewModel> GetMany<T, TKey, TViewModel>(IQueryable<T> filteredQueryable, int page = 1, int pageSize = 10)
            where T : class, IIdentifiable<TKey>
            where TViewModel : class, IGetViewModel<T>, new()
        {
            return ResultSetHelper.Convert(ResultSetHelper.GetResults<T, TKey>(filteredQueryable, page, pageSize), data =>
            {
                TViewModel viewModel = new TViewModel();
                viewModel.FillViewModel(data);
                return viewModel;
            });
        }

        #endregion Get
    }
}
