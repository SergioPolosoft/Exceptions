namespace Entities
{
    public interface ICharacter
    {
        string Id { get; }
        CharacterStatus Status { get;  }
        int Velocity { get; }
        Faction Faction { get; }
        bool CanBeCharged { get; }
        void Select();
        void Unselect();
        void MarkToBeCharged();
        bool IsNotMoved();
        bool IsEnemy(ICharacter character);
        void UnmarkAsToBeCharged();
        void Charge();
    }
}