namespace FCore.Foundations
{
    public interface IIdentifiable<out TKey>
    {
        TKey Id { get; }
    }
}
