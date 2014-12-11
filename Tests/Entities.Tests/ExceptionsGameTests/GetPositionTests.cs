using System;
using System.Collections.Generic;
using Entities.NullObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security;
using Security.Fakes;

namespace Entities.Tests.ExceptionsGameTests
{
    [TestClass]
    public class GetPositionTests
    {
        private ExceptionsGame exceptionsGame;

        [TestInitialize]
        public void TestInitialize()
        {
            var character = new Character();
            var southPlayer = new Player(new User("xavier"));
            southPlayer.Add(character);
            var northPlayer = new Player(new User("xavier"));
            northPlayer.Add(new Character());
            IApplicationContext applicationContext = new StubIApplicationContext();
            exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer, applicationContext);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GetPosition_Null_ThrowException()
        {
            exceptionsGame.GetPosition(null);
        }


        [TestMethod]
        public void GetPosition_NotExistingCharacter_NullPosition()
        {
            var position = exceptionsGame.GetPosition(new Character());
            Assert.IsInstanceOfType(position, typeof (NullPosition));
        }

        [TestMethod]
        public void GetPosition_ReturnPositionOfTheCharacter()
        {
            var character = new Character();
            var southPlayer = new Player(new User("xavier"));
            southPlayer.Add(character);
            var northPlayer = new Player(new User("xavier"));
            northPlayer.Add(new Character());
            IApplicationContext applicationContext = new StubIApplicationContext();
            exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer, applicationContext);

            var position = exceptionsGame.GetPosition(character);

            Assert.IsNotInstanceOfType(position, typeof (NullPosition));
            Assert.AreEqual(0, position.X%2);
            Assert.AreEqual(Map.Rows - 1, position.Y);
        }

        [TestMethod]
        public void GetPosition_PositionNotExist_ReturnNullPosition()
        {
            var position = exceptionsGame.GetPosition(Map.Columns, Map.Rows);
            Assert.IsInstanceOfType(position, typeof (NullPosition));
        }
    }
}