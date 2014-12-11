using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security;
using Security.Fakes;

namespace Entities.Tests
{
    [TestClass]
    public class GameFactoryTests
    {
        private ExceptionsGame exceptionsGame;
        private readonly GameFactory gameFactory = new GameFactory();
        private Player southPlayer;
        private Character southPlayerCharacter;
        private Player northPlayer;
        private Character northPlayerCharacter;

        [TestInitialize]
        public void TestInitialize()
        {
            southPlayer = new Player(new User("xavier"));
            southPlayerCharacter = new Character();
            southPlayer.Add(southPlayerCharacter);

            northPlayer = new Player(new User("xavier"));
            northPlayerCharacter = new Character();
            northPlayer.Add(northPlayerCharacter);
        }

        [TestMethod]
        public void NewGame_CreatesA11x12TiledMatrix()
        {
            exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer, new StubIApplicationContext());

            for (int x = 0; x < Map.Columns; x++)
            {
                for (int y = 0; y < Map.Rows; y++)
                {
                    var tile = exceptionsGame.GetPosition(x, y);
                    Assert.IsNotNull(tile);
                }
            }
        }

        [TestMethod]
        public void NewGame_SouthPlayersCharacters_OnLowestRow()
        {

            exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer, new StubIApplicationContext());

            ICharacter characterOnTile = null;
            for (int i = 0; i < Map.Columns; i++)
            {
                var tileOnLowestRow = exceptionsGame.GetPosition(i, Map.Rows - 1);

                characterOnTile = exceptionsGame.GetCharacterAtPosition(tileOnLowestRow);
                if (characterOnTile is Character)
                {
                    break;
                }
            }

            Assert.IsNotNull(characterOnTile);
            Assert.AreEqual(southPlayerCharacter, characterOnTile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewGame_SouthPlayerCharactersNull_ThrowArgumentException()
        {
            exceptionsGame = gameFactory.NewGame(null, northPlayer, new StubIApplicationContext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NewGame_SouthPlayerCharactersEmpty_ThrowArgumentException()
        {
            exceptionsGame = gameFactory.NewGame(new Player(new User("xavier")), northPlayer, new StubIApplicationContext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NewGame_SouthPlayerCharacters_BiggerThan6ThrowArgumentExceptions()
        {
            var player = new Player(new User("xavier"));
            for (int i = 0; i < 7; i++)
            {
                player.Add(new Character());
            }
            exceptionsGame = gameFactory.NewGame(player, northPlayer, new StubIApplicationContext());
        }

        [TestMethod]
        public void NewGame_SouthPlayerCharacters_OneCharacterArePositionedRandomlyOnAPairColumn()
        {
            var characters = new Player(new User("xavier"));
            var character = new Character();
            characters.Add(character);

            this.exceptionsGame = gameFactory.NewGame(characters, northPlayer, new StubIApplicationContext());

            var usedColumn = GetUsedColumn(exceptionsGame);

            // Sleep for a second in order to have a random value.
            Thread.Sleep(1000);

            var anotherGame = gameFactory.NewGame(characters, northPlayer, new StubIApplicationContext());
            var newColumn = GetUsedColumn(anotherGame);

            Assert.AreNotEqual(usedColumn, newColumn);
            Assert.AreEqual(0, usedColumn % 2);
            Assert.AreEqual(0, newColumn % 2);
        }

        private static int GetUsedColumn(ExceptionsGame game)
        {
            for (int column = 0; column < Map.Columns; column++)
            {
                var tileOnLowestRow = game.GetPosition(column, Map.Rows - 1);

                ICharacter characterOnTile = game.GetCharacterAtPosition(tileOnLowestRow);
                if (characterOnTile is Character)
                {
                    return column;
                }
            }
            return 0;
        }

        [TestMethod]
        public void NewGame_SouthPlayerCharacters_ThreeCharactersArePositionedRandomlyOnAPairColumn()
        {
            var player = new Player(new User("xavier"));
            player.Add(new Character());
            player.Add(new Character());
            player.Add(new Character());

            ValidateLocalPositionedCharacters(player);
        }

        private void ValidateLocalPositionedCharacters(Player player)
        {
            exceptionsGame = gameFactory.NewGame(player, northPlayer, new StubIApplicationContext());

            foreach (var localCharacter in player.ChoosenCharacters)
            {
                var tile = exceptionsGame.GetPosition(localCharacter);
                Assert.AreEqual(tile.X%2, 0);
                Assert.AreEqual(tile.Y, Map.Rows - 1);
            }
        }

        [TestMethod]
        public void NewGame_SouthPlayerCharacters_SixCharactersArePositionedRandomlyOnAPairColumn()
        {
            var player = new Player(new User("xavier"));
            player.Add(new Character());
            player.Add(new Character());
            player.Add(new Character());
            player.Add(new Character());
            player.Add(new Character());
            player.Add(new Character());

            ValidateLocalPositionedCharacters(player);
        }

        [TestMethod]
        public void NewGame_SouthPlayer_StartsTheTurn()
        {
            var game = gameFactory.NewGame(southPlayer, northPlayer, new StubIApplicationContext());

            Assert.AreEqual(southPlayer, game.TurnOwner);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewGame_SouthPlayerNull_ThrowNullArgumentException()
        {
            gameFactory.NewGame(null, northPlayer, new StubIApplicationContext());
        }

        [TestMethod]
        public void NewGame_NorthPlayerCharacters_PositionedOnTheFirstRow()
        {
            var game = gameFactory.NewGame(southPlayer, northPlayer, new StubIApplicationContext());

            var northPlayerPosition = game.GetPosition(northPlayerCharacter);

            Assert.AreEqual(0, northPlayerPosition.Y);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NewGame_NorthPlayerNoCharacters_ThrowArgumentException()
        {
            gameFactory.NewGame(southPlayer, new Player(new User("xavier")), new StubIApplicationContext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NewGame_NorthPlayerMoreThan6Characters_ThrowArgumentException()
        {
            northPlayer.Add(new Character());
            northPlayer.Add(new Character());
            northPlayer.Add(new Character());
            northPlayer.Add(new Character());
            northPlayer.Add(new Character());
            northPlayer.Add(new Character());
            northPlayer.Add(new Character());

            gameFactory.NewGame(southPlayer, northPlayer, new StubIApplicationContext());
        }

        [TestMethod]
        public void NewGame_SouthCharacters_OnSouthFaction()
        {
            gameFactory.NewGame(southPlayer, northPlayer, new StubIApplicationContext());

            Assert.AreEqual(Faction.South, southPlayerCharacter.Faction);
        }

        [TestMethod]
        public void NewGame_NorthCharacters_OnNorthFaction()
        {
            gameFactory.NewGame(southPlayer, northPlayer, new StubIApplicationContext());

            Assert.AreEqual(Faction.North, northPlayerCharacter.Faction);
        }
    }
}
