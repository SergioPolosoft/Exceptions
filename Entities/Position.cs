namespace Entities
{
    public class Position : IPosition
    {
        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public string Name
        {
            get { return string.Format("Tile_{0}_{1}", X, Y); }
        }

        public bool Selectable { get; set; }
    }
}