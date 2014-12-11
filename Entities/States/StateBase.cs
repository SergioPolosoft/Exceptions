namespace Entities.States
{
    internal abstract class StateBase : IState
    {
        protected Map map;
        protected ICharacter selectedCharacter;
        protected IPosition selectedPosition;
        public abstract void Select(ICharacter character);

        public virtual ICharacter SelectedCharacter
        {
            get { return selectedCharacter; }
        }

        public virtual IPosition SelectedPosition
        {
            get { return selectedPosition; }
        }

        public Map Map
        {
            get { return this.map; }
        }

        public abstract void Select(IPosition position);
        public abstract IState GetNextState();
    }
}