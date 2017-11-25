namespace CoreLibrary
{
    public interface IViewModel<T, TKey>
        where T : class, IIdentifiable<TKey>
    {
        void UpdateValues(T data);
        void SetValues(T data, bool uniqueIdentifierOnly = false);
        TKey GetUniqueIdentifier();
    }

    public interface IViewModel<T, TKey, TArgs>
        where T : class, IIdentifiable<TKey>
        where TArgs : class, IViewModelArgs
    {
        void UpdateValues(T data, TArgs args = null);
        void SetValues(T data, bool uniqueIdentifierOnly = false, TArgs args = null);
        TKey GetUniqueIdentifier();
    }
}
