namespace FCore.Foundations
{
    public interface IViewModel<T, TKey>
        where T : class, IIdentifiable<TKey>
    {
        void UpdateValues(T data);
        void SetValues(T data);
        TKey GetUniqueIdentifier();
    }

    public interface IViewModel<T, TKey, TArgs>
        where T : class, IIdentifiable<TKey>
        where TArgs : class, IViewModelArgs
    {
        void UpdateValues(T data, TArgs args = null);
        void SetValues(T data, TArgs args = null);
        TKey GetUniqueIdentifier();
    }
}
