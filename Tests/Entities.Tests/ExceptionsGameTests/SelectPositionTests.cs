using System;
using Entities.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security;
using Security.Fakes;

namespace Entities.Tests.ExceptionsGameTests
{
    [TestClass]
    public class SelectPositionTests
    {
        private IPosition currentPosition;
        private ExceptionsGame exceptionsGame;

        [TestInitialize]
        public void TestInitialize()
        {
            var character = new Character { Velocity = 4 };

            var soutPlayerUser = new User("xavier");
            var southPlayer = new Player(soutPlayerUser);
            southPlayer.Add(character);
            var northPlayer = new Player(new User("xavier"));
            northPlayer.Add(new Character());
            var applicationContext = new StubIApplicationContext();
            
            applicationContext.GetCurrentUser = () => soutPlayerUser;
            exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer, applicationContext);

            exceptionsGame.Select(character);

            currentPosition = exceptionsGame.GetPosition(character);
        }

        [TestMethod]
        public void SelectPosition_PositionIsSelectable_GameStateIsMovingCharacter()
        {
            var newPosition = exceptionsGame.GetPosition(currentPosition.X, currentPosition.Y - 2);

            exceptionsGame.Select(newPosition);

            Assert.IsInstanceOfType(exceptionsGame.State, typeof(MovingCharacter));
        }
        
        [TestMethod]
        public void SelectPosition_PositionIsNotSelectable_StateIsWaitingForActions()
        {
            var newPosition = exceptionsGame.GetPosition(0, 0);

            exceptionsGame.Select(newPosition);

            Assert.IsInstanceOfType(exceptionsGame.State, typeof(WaitingForActions));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SelectPosition_PositionDoesNotExist_ArgumentExceptionIsThrown()
        {
            var newPosition = new Position(17, 40);

            exceptionsGame.Select(newPosition);
        }

        [TestMethod]
        public void SelectPosition_PositionIsSelectable_PositionsIsSelected()
        {
            var newPosition = exceptionsGame.GetPosition(currentPosition.X, currentPosition.Y - 2);

            exceptionsGame.Select(newPosition);

            Assert.AreEqual(newPosition, exceptionsGame.SelectedPosition);
        }

        [TestMethod]
        public void SelectPosition_PositionIsSelectable_RestOfPositionsAreNotSelectable()
        {
            var newPosition = exceptionsGame.GetPosition(currentPosition.X, currentPosition.Y - 2);

            exceptionsGame.Select(newPosition);

            for (int x = 0; x < Map.Columns; x++)
            {
                for (int y = 0; y < Map.Rows; y++)
                {
                    var position = exceptionsGame.GetPosition(x, y);
                    Assert.IsFalse(position.Selectable);
                }
            }
        }
    }
}
