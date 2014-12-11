namespace Entities.NullObjects
{
    public class NullPosition : IPosition
    {
        public int X
        {
            get { return -1; }
        }

        public int Y
        {
            get { return -1; }
        }

        public string Name { get; private set; }
        public bool Selectable { get; set; }
        
    }
}