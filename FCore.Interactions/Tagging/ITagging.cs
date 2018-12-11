using FCore.Foundations;

namespace FCore.Interactions.Tagging
{
    public interface ITagging<TTag> : IIdentifiable<long>, IInteraction
        where TTag : class, ITag
    {
        long TagId { get; set; }
        TTag Tag { get; set; }

        string TaggerId { get; set; }
    }
}
