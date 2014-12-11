using System;
using System.Collections.Generic;
using System.Linq;
using Entities.States;
using Security;

namespace Entities
{
    public class GameFactory
    {
        private const int FirstRowIndex = 0;

        private const int LastRowIndex = Map.Rows - 1;
        private readonly Map map;

        public GameFactory()
        {
            map = new Map();
        }

        public ExceptionsGame NewGame(Player southPlayer, Player northPlayer, IApplicationContext applicationContext)
        {
            Validate(southPlayer);
            Validate(northPlayer);

            CreateMap();

            PositionateOn(southPlayer, Faction.South);

            PositionateOn(northPlayer, Faction.North);

            var initialState = new WaitingForActions(map);
            return new ExceptionsGame(southPlayer, northPlayer, initialState, applicationContext);
        }

        private void PositionateOn(Player player, Faction faction)
        {
            foreach (var character in player.ChoosenCharacters)
            {
                character.Faction = faction;
            }
            this.PositionateOnRow(player,GetRowIndex(faction));
        }

        private int GetRowIndex(Faction faction)
        {
            switch (faction)
            {
                case Faction.South:
                    return LastRowIndex;
                case Faction.North:
                    return FirstRowIndex;
                default:
                    throw new ArgumentOutOfRangeException("faction");
            }
        }

        private void PositionateOnRow(Player player, int rowIndex)
        {
            foreach (var character in player.ChoosenCharacters)
            {
                PositionateOnRow(character, rowIndex);
            }
        }

        private void PositionateOnRow(Character character, int rowIndex)
        {
            var tile = GetFreeTileOnRow(rowIndex);

            var result = map.AddCharacterAtPosition(character, tile);

            if (result == false)
            {
                throw new ApplicationException("Error adding characters into the map");
            }
        }

        private static void Validate(Player southPlayer)
        {
            if (southPlayer == null) throw new ArgumentNullException("southPlayer");
            Validate(southPlayer.ChoosenCharacters);
        }

        private IPosition GetFreeTileOnRow(int rowIndex)
        {
            IPosition position;
            var random = new Random();
            do
            {
                var index = random.NextPair(0, Map.Columns);

                position = map.GetPosition(index, rowIndex);
            } while (IsNotFree(position));

            return position;
        }

        private bool IsNotFree(IPosition position)
        {
            return map.IsFree(position) == false;
        }

        private void CreateMap()
        {
            for (int x = 0; x < Map.Columns; x++)
            {
                for (int y = 0; y < Map.Rows; y++)
                {
                    map.AddPosition(new Position(x, y));
                }
            }
        }

        private static void Validate(IList<Character> localCharacters)
        {
            if (localCharacters == null) throw new ArgumentNullException("localCharacters");

            var localCharactersSize = localCharacters.Count;
            if (localCharactersSize <= 0 || localCharactersSize > 6)
            {
                throw new ArgumentOutOfRangeException("localCharacters");
            }
        }
    }
}