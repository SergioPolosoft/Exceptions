using System;
using Entities.NullObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security;
using Security.Fakes;

namespace Entities.Tests.ExceptionsGameTests
{
    [TestClass]
    public class SelectCharacterTests
    {
        private StubIApplicationContext applicationContext;
        private Character beast;
        private Character cyclops;
        private ExceptionsGame exceptionsGame;
        private Character quicksilver;
        private User southPlayerUser;
        private Character scarletWitch;

        [TestInitialize]
        public void TestInitialize()
        {
            applicationContext = new StubIApplicationContext();

            southPlayerUser = new User("xavier");
            var southPlayer = new Player(southPlayerUser);
            beast = new Character {Name = "beast"};
            southPlayer.Add(beast);
            cyclops = new Character {Name = "cyclops", Velocity = 4};
            southPlayer.Add(cyclops);

            var northPlayer = new Player(new User("erik"));
            quicksilver = new Character { Name = "quicksilver" };
            scarletWitch = new Character { Name = "scarletWitch" };
            northPlayer.Add(quicksilver);
            northPlayer.Add(scarletWitch);

            applicationContext.GetCurrentUser = () => southPlayerUser;
            exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer, applicationContext);
        }

        [TestMethod]
        public void SelectCharacter_CharacterStatusIsSelected()
        {
            var character = new Character();
            var southPlayer = new Player(southPlayerUser);
            southPlayer.Add(character);
            var northPlayer = new Player(new User("xavier"));
            northPlayer.Add(new Character());
            applicationContext.GetCurrentUser = () => southPlayerUser;
            exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer, applicationContext);

            exceptionsGame.Select(character);

            Assert.AreEqual(CharacterStatus.Selected, character.Status);

            var position = exceptionsGame.GetPosition(character);
            var characterOnEntity = exceptionsGame.GetCharacterAtPosition(position);

            Assert.AreEqual(CharacterStatus.Selected, characterOnEntity.Status);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void SelectCharacter_NotExistingCharacter_ThrowArgumentException()
        {
            exceptionsGame.Select(new Character());
        }

        [TestMethod]
        public void SelectCharacter_SelectAnotherCharacter_NewOneIsSelected()
        {
            var phoenix = new Character();
            var iceman = new Character();
            var southPlayer = new Player(southPlayerUser);
            southPlayer.Add(phoenix);
            southPlayer.Add(iceman);
            var northPlayer = new Player(new User("xavier"));
            northPlayer.Add(new Character());

            exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer, applicationContext);

            exceptionsGame.Select(phoenix);

            exceptionsGame.Select(iceman);

            Assert.AreEqual(CharacterStatus.Selected, iceman.Status);
        }

        [TestMethod]
        public void SelectCharacter_SelectAnotherCharacter_OldOneIsWaiting()
        {
            var phoenix = new Character();
            var iceman = new Character();
            var southPlayer = new Player(new User("xavier"));
            southPlayer.Add(phoenix);
            southPlayer.Add(iceman);
            var northPlayer = new Player(new User("xavier"));
            northPlayer.Add(new Character());
            exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer, applicationContext);

            exceptionsGame.Select(phoenix);

            exceptionsGame.Select(iceman);

            Assert.AreEqual(CharacterStatus.Waiting, phoenix.Status);
        }

        [TestMethod]
        public void SelectCharacter_SelectAnotherCharacter_CleanOldSelectablePositions()
        {
            var phoenix = new Character {Velocity = 5};
            var iceman = new Character {Velocity = 1};

            var southPlayer = new Player(southPlayerUser);
            southPlayer.Add(phoenix);
            southPlayer.Add(iceman);
            var northPlayer = new Player(new User("xavier"));
            northPlayer.Add(new Character());
            exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer, applicationContext);

            exceptionsGame.Select(phoenix);

            var cyclopsPosition = exceptionsGame.GetPosition(phoenix);
            var position = exceptionsGame.GetPosition(cyclopsPosition.X, cyclopsPosition.Y - 5);
            exceptionsGame.MoveCharacterToPosition(phoenix, position);

            exceptionsGame.Select(phoenix);
            var selectablePosition = exceptionsGame.GetPosition(position.X, cyclopsPosition.Y - 1);
            Assert.IsTrue(selectablePosition.Selectable);

            exceptionsGame.Select(iceman);
            Assert.IsFalse(selectablePosition.Selectable);
        }

        [TestMethod]
        public void SelectCharacter_VelocityOf1_UpRightDownAndLeftAreSetAsSelectable()
        {
            var character = new Character {Velocity = 1};

            var southPlayer = new Player(southPlayerUser);
            southPlayer.Add(character);
            var northPlayer = new Player(new User("xavier"));
            northPlayer.Add(new Character());
            exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer, applicationContext);

            exceptionsGame.Select(character);

            var position = exceptionsGame.GetPosition(character);

            for (int x = 0; x < Map.Columns; x++)
            {
                for (int y = 0; y < Map.Rows; y++)
                {
                    var tile = exceptionsGame.GetPosition(x, y);
                    if (x == position.X && (y == position.Y - 1 || y == position.Y + 1))
                    {
                        Assert.IsTrue(tile.Selectable);
                    }
                    else if (y == position.Y && (x == position.X - 1 || x == position.X + 1))
                    {
                        Assert.IsTrue(tile.Selectable);
                    }
                    else
                    {
                        Assert.IsFalse(tile.Selectable);
                    }
                }
            }
        }

        [TestMethod]
        public void SelectCharacter_VelocityOf5_PossiblePositionsAreSelectable()
        {
            var character = new Character {Velocity = 5};

            var southPlayer = new Player(southPlayerUser);
            southPlayer.Add(character);
            var northPlayer = new Player(new User("xavier"));
            northPlayer.Add(new Character());
            exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer, applicationContext);

            exceptionsGame.Select(character);
            var position = exceptionsGame.GetPosition(character);

            for (int x = 1; x <= character.Velocity; x++)
            {
                var restOfVelocity = character.Velocity - x;
                for (int y = 1; y <= restOfVelocity; y++)
                {
                    var tile = exceptionsGame.GetPosition(position.X + x, position.Y + y);
                    if (tile is Position)
                    {
                        Assert.IsTrue(tile.Selectable, "Tile {0} is not selectable.", tile.Name);
                    }
                    tile = exceptionsGame.GetPosition(position.X + x, position.Y - y);
                    if (tile is Position)
                    {
                        Assert.IsTrue(tile.Selectable, "Tile {0} is not selectable.", tile.Name);
                    }
                    tile = exceptionsGame.GetPosition(position.X - x, position.Y - y);
                    if (tile is Position)
                    {
                        Assert.IsTrue(tile.Selectable, "Tile {0} is not selectable.", tile.Name);
                    }
                    tile = exceptionsGame.GetPosition(position.X - x, position.Y + y);
                    if (tile is Position)
                    {
                        Assert.IsTrue(tile.Selectable, "Tile {0} is not selectable.", tile.Name);
                    }
                }
            }
        }

        [TestMethod]
        public void SelectCharacter_SelectOthersPlayersCharacters_DoNotSelectCharacter()
        {
            exceptionsGame.Select(quicksilver);

            Assert.IsInstanceOfType(exceptionsGame.SelectedCharacter, typeof (NullCharacter));
        }

        [TestMethod]
        public void SelectCharacter_UserIsTurnOwner_CharacterIsSelected()
        {
            exceptionsGame.Select(cyclops);

            Assert.AreEqual(cyclops, exceptionsGame.SelectedCharacter);
        }

        [TestMethod]
        public void SelectCharacter_UserIsNotTurnOwner_CharacterIsNotSelected()
        {
            exceptionsGame.EndTurn();

            exceptionsGame.Select(cyclops);

            Assert.IsInstanceOfType(exceptionsGame.SelectedCharacter, typeof (NullCharacter));
        }

        [TestMethod]
        public void SelectCharacter_OnlyPositionsThatCanBeArrived_AreSelectable()
        {
            var southPlayerCharacterPosition = exceptionsGame.GetPosition(cyclops);
            var positionForCyclops =
                exceptionsGame.GetPosition(southPlayerCharacterPosition.X,
                                           southPlayerCharacterPosition.Y - cyclops.Velocity + 1);

            exceptionsGame.State.Map.AddCharacter(new Character(), positionForCyclops);

            exceptionsGame.Select(cyclops);

            var positionToMoveNotSelectable =
                exceptionsGame.GetPosition(southPlayerCharacterPosition.X,
                                           southPlayerCharacterPosition.Y - cyclops.Velocity);

            Assert.IsFalse(positionToMoveNotSelectable.Selectable);
        }
    }
}