using System;
using Entities.NullObjects;

namespace Entities.States
{
    internal class WaitingForActions : StateBase
    {
        private const int ChargeDistance = 6;
        private readonly PathBuilder pathBuilder;

        public WaitingForActions(Map map)
        {
            this.pathBuilder = new PathBuilder(map);
            this.map = map;
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
            if (character == null) throw new ArgumentNullException("character");
            if (map.Exists(character))
            {
                ProcessSelection(character);
            }
            else
            {
                throw new ArgumentException("Character not exist on the game.");
            }
        }

        private void ProcessSelection(ICharacter character)
        {
            Clear();

            selectedCharacter = character;
            selectedCharacter.Select();

            if (character.IsNotMoved())
            {
                RangeAction(selectedCharacter.Velocity, SelectPosition);

                RangeAction(selectedCharacter.Velocity + ChargeDistance, MarkChargableCharacters);
            }
        }

        private void RangeAction(int range, Action<int, int> rangedAction)
        {
            var position = map.GetPosition(selectedCharacter);

            for (int x = 0; x <= range; x++)
            {
                var restOfRange = range - x;
                for (int y = 0; y <= restOfRange; y++)
                {
                    int up = position.X - x;
                    int left = position.Y + y;
                    int down = position.X + x;
                    int right = position.Y - y;

                    rangedAction.Invoke(up, left);
                    rangedAction.Invoke(down, left);
                    rangedAction.Invoke(down, right);
                    rangedAction.Invoke(up, right);
                }
            }
        }

        private void MarkChargableCharacters(int x, int y)
        {
            var possibleEnemyPosition = this.map.GetPosition(x, y);

            var characterAtPosition = this.map.GetCharacterAtPosition(possibleEnemyPosition);

            if (characterAtPosition.IsEnemy(selectedCharacter))
            {
                var chargePosition = this.map.GetPosition(possibleEnemyPosition.X, possibleEnemyPosition.Y + 1);

                var maxPathDistance = selectedCharacter.Velocity + ChargeDistance;

                if (pathBuilder.GetPath(selectedPosition,chargePosition,maxPathDistance).Count>0)
                {
                    characterAtPosition.MarkToBeCharged();
                }
                
            }
        }

        private void Clear()
        {
            selectedCharacter.Unselect();
            ClearPossibleSelectablePositions();
            ClearChargableCharacters();
        }

        private void ClearChargableCharacters()
        {
            foreach (var character in map.Characters)
            {
                character.UnmarkAsToBeCharged();
            }
        }

        private void ClearPossibleSelectablePositions()
        {
            foreach (var position in map.Positions)
            {
                position.Selectable = false;
            }
        }

        private void SelectPosition(int x, int y)
        {
            var selectablePosition = this.map.GetPosition(x, y);

            if (selectablePosition is Position && IsNotSelected(selectablePosition) && map.IsFree(selectablePosition) &&
                CanBeReach(selectablePosition, selectedCharacter.Velocity))
            {
                selectablePosition.Selectable = true;
            }
        }

        private static bool IsNotSelected(IPosition selectablePosition)
        {
            return selectablePosition.Selectable == false;
        }

        private bool CanBeReach(IPosition selectablePosition, int maxPathDistance)
        {
            var position = selectablePosition as Position;
            if (position != null)
            {
                Character character = (Character) SelectedCharacter;
                return pathBuilder.GetPath(character, position, maxPathDistance).Count > 0;
            }
            return false;
        }

        public override void Select(IPosition position)
        {
            if (PositionNotExists(position))
            {
                throw new ArgumentException("Unknown position trying to be selected.");
            }
            if (position.Selectable)
            {
                ClearPossibleSelectablePositions();
                this.selectedPosition = position;
            }
        }

        public override IState GetNextState()
        {
            if (this.selectedCharacter is NullCharacter || this.selectedPosition is NullPosition)
            {
                return this;
            }
            return new MovingCharacter(this.selectedCharacter, this.map, this.selectedPosition);
        }


        private bool PositionNotExists(IPosition position)
        {
            if (this.map.GetPosition(position.X, position.Y) is NullPosition)
            {
                return true;
            }
            return false;
        }
    }
}