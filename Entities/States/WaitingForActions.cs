using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Extensions;
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

        private void RangeAction(int range, Action<IPosition> rangedAction)
        {
            var position = map.GetPosition(selectedCharacter);

            var positions = GetPositionsInRange(range, position);

            foreach (var element in positions)
            {
                rangedAction.Invoke(element);
            }
        }

        private IEnumerable<IPosition> GetPositionsInRange(int range, IPosition position)
        {
            var positions = new List<IPosition>();
            for (int x = 0; x <= range; x++)
            {
                var restOfRange = range - x;
                for (int y = 0; y <= restOfRange; y++)
                {
                    int up = position.X - x;
                    int left = position.Y + y;
                    int down = position.X + x;
                    int right = position.Y - y;

                    positions.AddIfNotExists(map.GetPosition(up, left));
                    positions.AddIfNotExists(map.GetPosition(down, left));
                    positions.AddIfNotExists(map.GetPosition(down, right));
                    positions.AddIfNotExists(map.GetPosition(up, right));
                }
            }
            return positions;
        }

        private void MarkChargableCharacters(IPosition possibleEnemyPosition)
        {
            var characterAtPosition = this.map.GetCharacterAtPosition(possibleEnemyPosition);

            if (characterAtPosition.IsEnemy(selectedCharacter))
            {
                var possiblePositions = GetPositionsInRange(1, possibleEnemyPosition);

                if (possiblePositions.Any(ExistsPathToCharge))
                {
                    characterAtPosition.MarkToBeCharged();
                }
            }
        }

        private IEnumerable<IPosition> GetPossibleChargePositions(IPosition possibleEnemyPosition)
        {
            var possiblePositions = new List<IPosition>
                {
                    // Front
                    this.map.GetPosition(possibleEnemyPosition.X, possibleEnemyPosition.Y + 1),
                    // Left
                    this.map.GetPosition(possibleEnemyPosition.X - 1, possibleEnemyPosition.Y),
                    // Right
                    this.map.GetPosition(possibleEnemyPosition.X + 1, possibleEnemyPosition.Y)
                };
            return possiblePositions;
        }

        private bool ExistsPathToCharge(IPosition position)
        {
            bool pathToCharge = false;

            if (map.IsFree(position))
            {
                var maxPathDistance = selectedCharacter.Velocity + ChargeDistance;

                pathToCharge = pathBuilder.GetPath(SelectedCharacter, position, maxPathDistance).Count > 0;
            }
            return pathToCharge;
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

        private void SelectPosition(IPosition selectablePosition)
        {
            if (CanBeSelected(selectablePosition))
            {
                selectablePosition.Selectable = true;
            }
        }

        private bool CanBeSelected(IPosition selectablePosition)
        {
            return selectablePosition is Position && 
                   selectablePosition.IsNotSelected &&
                   map.IsFree(selectablePosition) &&
                   CanBeReach(selectablePosition, selectedCharacter.Velocity);
        }

        private bool CanBeReach(IPosition selectablePosition, int maxPathDistance)
        {
            return pathBuilder.GetPath(SelectedCharacter, selectablePosition, maxPathDistance).Count > 0;
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