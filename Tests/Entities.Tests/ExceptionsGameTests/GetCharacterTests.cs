using System.Collections.Generic;
using Entities.NullObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security;
using Security.Fakes;

namespace Entities.Tests.ExceptionsGameTests
{
    [TestClass]
    public class GetCharacterTests
    {
        private ExceptionsGame exceptionsGame;
        private Character character;

        [TestInitialize]
        public void TestInitialize()
        {
            character = new Character();
            var southPlayer = new Player(new User("xavier"));
            southPlayer.Add(character);
            var northPlayer = new Player(new User("xavier"));
            northPlayer.Add(new Character());
            IApplicationContext applicationContext = new StubIApplicationContext();
            exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer, applicationContext);
        }

        [TestMethod]
        public void GetCharacterAtPosition_ReturnsCharacterAtSpecifiedPosision()
        {
            var tile = exceptionsGame.GetPosition(character);

            var characterAtPosition = exceptionsGame.GetCharacterAtPosition(tile);

            Assert.AreEqual(character, characterAtPosition);
        }

        [TestMethod]
        public void GetCharacterAtPosition_Nothing_ReturnNullCharacter()
        {
            var tile = new Position(0, 0);

            var characterAtPosition = exceptionsGame.GetCharacterAtPosition(tile);

            Assert.IsInstanceOfType(characterAtPosition, typeof (NullCharacter));
        }

        [TestMethod]
        public void GetCharacterAtPosition_PositionNotExists_ReturnNullCharacter()
        {
            var tile = new Position(13, 14);

            var characterAtPosition = exceptionsGame.GetCharacterAtPosition(tile);

            Assert.IsInstanceOfType(characterAtPosition, typeof (NullCharacter));
        }
    }
}