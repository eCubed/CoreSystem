using FCore.Foundations;

namespace FCore.Interactions.Tagging
{
    public interface ITag : IIdentifiable<long>
    {
        string Name { get; set; }
    }
}
