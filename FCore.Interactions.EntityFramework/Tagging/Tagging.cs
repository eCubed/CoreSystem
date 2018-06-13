using FCore.Interactions.Tagging;

namespace FCore.Interactions.EntityFramework
{
    public class Tagging : ITagging<Tag>       
    {
        public int SystemObjectId { get; set; }
        public virtual SystemObject SystemObject { get; set; }

        public string RecordId { get; set; }

        public long TagId { get; set; }
        public virtual Tag Tag { get; set; }

        public string TaggerId { get; set; }

        public long Id { get; set; }
    }
}
