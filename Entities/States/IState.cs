namespace Entities.States
{
    internal interface IState
    {
        void Select(ICharacter character);
        ICharacter SelectedCharacter { get; }
        IPosition SelectedPosition { get; }
        Map Map { get; }
        void Select(IPosition position);
        IState GetNextState();
    }
}