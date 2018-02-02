namespace FCore.Foundations
{
    public interface IViewModel<T, TKey>
        where T : class, IIdentifiable<TKey>
    {
        TKey Id { get; set; }

        void UpdateValues(T data);
        void SetValues(T data);
    }

    public interface IViewModel<T, TKey, TArgs>
        where T : class, IIdentifiable<TKey>
        where TArgs : class, IViewModelArgs
    {
        TKey Id { get; set; }

        void UpdateValues(T data, TArgs args = null);
        void SetValues(T data, TArgs args = null);
    }
}
