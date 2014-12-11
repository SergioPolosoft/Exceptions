using System;
using System.Collections.Generic;

namespace Entities.States
{
    internal class MovingCharacter : StateBase
    {
        public MovingCharacter(ICharacter selectedCharacter, Map map, IPosition selectedPosition)
        {
            this.selectedCharacter = selectedCharacter;
            this.map = map;
            this.selectedPosition = selectedPosition;
        }

        public override void Select(ICharacter character)
        {
            
        }

        public override ICharacter SelectedCharacter { get { return this.selectedCharacter; } }
        public override IPosition SelectedPosition { get { return this.selectedPosition; } }

        public override void Select(IPosition position)
        {
            
        }

        public override IState GetNextState()
        {
            return this;
        }
    }
}