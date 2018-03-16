using FCore.Foundations;

namespace FCore.Interactions.Tagging
{
    public interface ITagging<TTag> : IIdentifiable<long>
        where TTag : class, ITag
    {
        int SystemObjectId { get; set; }
        string RecordId { get; set; }

        long TagId { get; set; }
        TTag Tag { get; set; }

        string TaggerId { get; set; }
    }
}
