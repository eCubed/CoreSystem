using FCore.Foundations;
using System.Collections.Generic;

namespace FCore.Interactions.Tagging
{
    public interface ITaggable
    {
        List<BasicListingViewModel<long>> Tags { get; set; }
    }
}
