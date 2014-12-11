namespace Entities
{
    public interface IPosition
    {
        int X { get; }
        int Y { get; }
        string Name { get; }
        bool Selectable { get; set; }
    }
}