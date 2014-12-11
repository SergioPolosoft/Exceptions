using System;
using Entities.States;
using Entities.Validators;
using Security;

namespace Entities
{
    public class ExceptionsGame
    {
        private readonly IApplicationContext applicationContext;
        private readonly Map map;
        private readonly Player northPlayer;
        private readonly Player southPlayer;
        private Player turnOwner;
        private IState state;

        internal ExceptionsGame(Player southPlayer, Player northPlayer, IState initialState,
                              IApplicationContext applicationContext)
        {
            this.state = initialState;
            this.map = initialState.Map;
            turnOwner = southPlayer;
            this.applicationContext = applicationContext;
            this.southPlayer = southPlayer;
            this.northPlayer = northPlayer;
        }

        internal IState State
        {
            get { return state; }
        }

        public IPosition SelectedPosition
        {
            get { return state.SelectedPosition; }
        }

        public ICharacter SelectedCharacter
        {
            get { return state.SelectedCharacter; }
        }

        public Player TurnOwner
        {
            get { return turnOwner; }
        }

        public int MapColumns
        {
            get { return Map.Columns; }
        }

        public int MapRows
        {
            get { return Map.Rows; }
        }

        public ICharacter GetCharacterAtPosition(IPosition position)
        {
            return map.GetCharacterAtPosition(position);
        }

        public IPosition GetPosition(int x, int y)
        {
            return map.GetPosition(x, y);
        }

        public IPosition GetPosition(ICharacter character)
        {
            return map.GetPosition(character);
        }

        public void Select(Character character)
        {
            if (map.Exists(character) == false)
            {
                throw new ArgumentException("The character do not exists on the game");
            }
            if (turnOwner.User == applicationContext.GetCurrentUser() && turnOwner.ChoosenCharacters.Contains(character))
            {
                this.state.Select(character);
            }
        }

        public void MoveCharacterToPosition(Character character, IPosition newPosition)
        {
            ValidatorFacade.Validate(character);
            Validate(newPosition);
            ValidateIsOnTheGame(character);

            map.ReplaceOldPositionWithNewPosition(character, newPosition);

            if (IsSelectedPosition(newPosition))
            {
                character.SetAsMoved();
                state = new WaitingForActions(this.map);
            }
        }

        private bool IsSelectedPosition(IPosition position)
        {
            return position == this.SelectedPosition;
        }

        private void ValidateIsOnTheGame(Character character)
        {
            if (map.Exists(character) == false)
            {
                throw new ArgumentException("The character is not on the game");
            }
        }

        private void Validate(IPosition position)
        {
            if (position == null)
            {
                throw new ArgumentNullException("position");
            }
            if (map.Exists(position) == false)
            {
                throw new ArgumentException(string.Format("The position {0} do not exists", position.Name));
            }
        }
        

        public void Select(IPosition position)
        {
            this.state.Select(position);
            this.state = this.state.GetNextState();
        }

        public void EndTurn()
        {
            if (TurnCanBeFinished())
            {
                SwapTurnOwner();
            }
        }

        private void SwapTurnOwner()
        {
            if (SouthPlayerIsTurnOwner())
            {
                this.turnOwner = this.northPlayer;
            }
            else
            {
                this.turnOwner = this.southPlayer;
            }
        }

        private bool SouthPlayerIsTurnOwner()
        {
            return this.TurnOwner == this.southPlayer;
        }

        private bool TurnCanBeFinished()
        {
            return turnOwner.User == applicationContext.GetCurrentUser();
        }
    }
}