namespace Entities.States
{
    internal class MovingCharacter : StateBase
    {
        public MovingCharacter(Map map, ICharacter selectedCharacter, IPosition selectedPosition) : base(map)
        {
            this.selectedCharacter = selectedCharacter;
            this.selectedPosition = selectedPosition;
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
        }

        public override IState GetNextState()
        {
            return this;
        }
    }
}