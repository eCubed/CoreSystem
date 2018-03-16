using FCore.Foundations;

namespace FCore.Interactions
{
    public interface ISystemObject : IIdentifiable<int>
    {
        string Name { get; set; }
    }
}
