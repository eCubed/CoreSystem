using FCore.Interactions.Tagging;
using System.Collections.Generic;

namespace FCore.Interactions.EntityFramework
{
    public class Tag : ITag
    {
        public string Name { get; set; }

        public long Id { get; set; }

        public virtual ICollection<Tagging> Taggings { get; set; }
    }
}
