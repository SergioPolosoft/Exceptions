namespace Entities.NullObjects
{
    public class NullCharacter:ICharacter
    {
        public string Id { get; private set; }
        public CharacterStatus Status { get; private set; }
        public int Velocity { get; private set; }
        public Faction Faction { get; private set; }
        public bool CanBeCharged { get; private set; }

        public void Select()
        {
        }

        public void Unselect()
        {
        }

        public void MarkToBeCharged()
        {
        }

        public bool IsNotMoved()
        {
            return false;
        }

        public bool IsEnemy(ICharacter character)
        {
            return false;
        }

        public void UnmarkAsToBeCharged()
        {
            
        }
    }
}