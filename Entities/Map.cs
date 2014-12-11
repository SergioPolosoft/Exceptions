using System;
using System.Collections.Generic;
using System.Linq;
using Entities.NullObjects;

namespace Entities
{
    internal class Map
    {
        public const int Columns = 20;
        public const int Rows = 15;
        private readonly IDictionary<IPosition, ICharacter> positionedCharacters;
        private Position[,] positions;

        public Map():this(Columns,Rows)
        {
        }

        private void InitializePositions(int columns, int rows)
        {
            this.positions = new Position[columns,rows];
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    this.AddPosition(new Position(x,y));
                }
            }
        }

        public Map(int columns, int rows)
        {
            Validate(columns, rows);
            InitializePositions(columns, rows);

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
                    if (position != null)
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

        private static void Validate(int columns, int rows)
        {
            if (columns <= 0)
            {
                throw new ArgumentOutOfRangeException("columns");
            }
            if (rows <= 0)
            {
                throw new ArgumentOutOfRangeException("rows");
            }
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

        internal bool AddCharacter(Character character, IPosition position)
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

        public void AddCharacter(Character character, int x, int y)
        {
            ValidateAddCharacterParameters(character, x, y);

            var position = this.GetPosition(x, y);
            AddCharacter(character, position);
        }

        private static void ValidateAddCharacterParameters(Character character, int x, int y)
        {
            Validate(character);
            ValidateIsNotNegative(x);
            ValidateIsNotNegative(y);
        }

        private static void ValidateIsNotNegative(int parameter)
        {
            if (parameter < 0)
            {
                throw new ArgumentOutOfRangeException("parameter");
            }
        }

        public ICharacter GetCharacterAtPosition(int x, int y)
        {
            ValidateIsNotNegative(x);
            ValidateIsNotNegative(y);
            var position = this.GetPosition(x, y);

            return GetCharacterAtPosition(position);
        }
    }
}