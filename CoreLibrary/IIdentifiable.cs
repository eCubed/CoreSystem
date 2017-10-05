namespace CoreLibrary
{
    public interface IIdentifiable<out TKey>
    {
        TKey Id { get; }
    }
}
