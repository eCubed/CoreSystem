namespace FCore.Foundations
{
    public class BasicListingViewModel<TKey>
        where TKey : struct
    {
        public TKey Id { get; set; }
        public string Name { get; set; }
    }

    public class BasicListingViewModel : BasicListingViewModel<int>
    {
    }
}
