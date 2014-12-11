using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities.NullObjects;

namespace Entities
{
    internal class Map
    {
        public const int Columns = 20;
        public const int Rows = 15;
        private readonly Position[,] positions;
        private readonly IDictionary<IPosition, ICharacter> positionedCharacters;

        public Map()
        {
            this.positions = new Position[Columns,Rows];

            positionedCharacters = new Dictionary<IPosition, ICharacter>();
        }

        public Position[,] Positions
        {
            get { return positions; }
        }

        public bool IsEmpty
        {
            get
            {
                foreach (var position in Positions)
                {
                    if (position!=null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        internal IEnumerable<ICharacter> Characters
        {
            get { return positionedCharacters.Values; }
        }

        public IPosition GetPosition(int x, int y)
        {
            if (x >= positions.GetLength(0) || y >= positions.GetLength(1) || x < 0 || y < 0)
            {
                return new NullPosition();
            }
            return this.positions[x, y];
        }

        public void AddPosition(Position position)
        {
            this.positions[position.X, position.Y] = position;
        }

        internal bool AddCharacterAtPosition(Character character, IPosition position)
        {
            ValidateParameters(character, position);

            bool result = false;

            if (IsFree(position))
            {
                positionedCharacters[position] = character;
                result = true;
            }

            return result;
        }

        public bool IsFree(IPosition position)
        {
            return positionedCharacters.ContainsKey(position) == false;
        }

        private void ValidateParameters(Character character, IPosition position)
        {
            Validate(character);

            Validate(position);
        }

        private static void Validate(ICharacter character)
        {
            if (character == null)
            {
                throw new ArgumentNullException("character");
            }
        }

        private void Validate(IPosition position)
        {
            if (position == null)
            {
                throw new ArgumentNullException("position");
            }
            if (Exists(position) == false)
            {
                throw new ArgumentException(string.Format("The position {0} do not exists", position.Name));
            }
        }

        internal bool Exists(IPosition position)
        {
            foreach (var tile in this.positions)
            {
                if (tile == position)
                {
                    return true;

                }
            }
            return false;
        }

        public ICharacter GetCharacterAtPosition(IPosition position)
        {
            if (IsFree(position))
            {
                return new NullCharacter();
            }
            return positionedCharacters[position];
        }

        public IPosition GetPosition(ICharacter character)
        {
            Validate(character);

            var position = positionedCharacters.SingleOrDefault(x => x.Value == character).Key;
            if (position == null)
            {
                return new NullPosition();
            }

            return position;
        }

        public bool Exists(ICharacter character)
        {
            return positionedCharacters.Values.Contains(character);
        }

        public void ReplaceOldPositionWithNewPosition(Character character, IPosition position)
        {
            var oldPosition = this.GetPosition(character);
            positionedCharacters.Remove(oldPosition);
            positionedCharacters.Add(position, character);
        }
    }
}