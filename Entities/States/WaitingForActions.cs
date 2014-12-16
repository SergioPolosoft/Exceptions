using System;
using Entities.NullObjects;

namespace Entities.States
{
    internal class WaitingForActions : StateBase
    {
        public WaitingForActions(Map map) : base(map)
        {
            selectedCharacter = new NullCharacter();
            selectedPosition = new NullPosition();
        }

        public override ICharacter SelectedCharacter
        {
            get { return this.selectedCharacter; }
        }

        public override IPosition SelectedPosition
        {
            get { return this.selectedPosition; }
        }

        public override void Select(ICharacter character)
        {
        }


        public override void Select(IPosition position)
        {
            throw new NotImplementedException();
        }

        public override IState GetNextState()
        {
            return this;
        }
    }
}