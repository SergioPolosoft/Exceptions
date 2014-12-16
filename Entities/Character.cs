using System;

namespace Entities
{
    public class Character : ICharacter
    {
        private CharacterStatus status;

        public Character()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; private set; }

        public CharacterStatus Status
        {
            get { return status; }
        }

        public int Velocity { get; set; }
        
        public Faction Faction { get; internal set; }

        public bool Moved { get; private set; }

        public bool CanBeCharged { get; private set; }

        public string Name { get; set; }

        public void Select()
        {
            this.status = CharacterStatus.Selected;
        }

        public void Unselect()
        {
            this.status = CharacterStatus.Waiting;
        }

        public void MarkToBeCharged()
        {
            this.CanBeCharged = true;
        }

        internal void SetAsMoved()
        {
            this.Moved = true;
        }

        public bool IsNotMoved()
        {
            return Moved == false;
        }

        public bool IsEnemy(ICharacter character)
        {
            return this.Faction != character.Faction;
        }

        public void UnmarkAsToBeCharged()
        {
            this.CanBeCharged = false;
        }

        public void Charge()
        {
            this.status = CharacterStatus.Charging;
        }
    }
}