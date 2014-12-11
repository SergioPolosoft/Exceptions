using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entities.Tests.ExceptionsGameTests
{
    [TestClass]
    public class TurnsTest
    {
        [TestMethod]
        public void TurnIsFinished_TurnOwnerIsChangedToTheOtherPlayer()
        {
            ExceptionsGame exceptionsGame = DefaultTestingGame.Create();

            Assert.AreEqual(DefaultTestingGame.SouthPlayer, exceptionsGame.TurnOwner);

            exceptionsGame.EndTurn();
            
            Assert.AreEqual(DefaultTestingGame.NorthPlayer, exceptionsGame.TurnOwner);

            exceptionsGame.EndTurn();

            Assert.AreEqual(DefaultTestingGame.NorthPlayer, exceptionsGame.TurnOwner);
        }

        [TestMethod]
        public void User_IsTurnOwner_CanFinishTheTurn()
        {
            ExceptionsGame exceptionsGame = DefaultTestingGame.Create();

            var turnOwner = exceptionsGame.TurnOwner;

            exceptionsGame.EndTurn();

            Assert.AreEqual(DefaultTestingGame.NorthPlayer, exceptionsGame.TurnOwner);

            var newTurnOwner = exceptionsGame.TurnOwner;

            Assert.AreNotEqual(newTurnOwner, turnOwner);
        }

        [TestMethod]
        public void User_IsTurnOwner_CanSelectItsCharacters()
        {
            ExceptionsGame exceptionsGame = DefaultTestingGame.Create();

            var character = DefaultTestingGame.SouthPlayer.ChoosenCharacters.First();
            exceptionsGame.Select(character);

            Assert.AreEqual(exceptionsGame.SelectedCharacter, character);
        }

        [TestMethod]
        public void User_IsTurnOwner_CanNOTSelectOtherPlayerCharacters()
        {
            ExceptionsGame exceptionsGame = DefaultTestingGame.Create();

            var character = DefaultTestingGame.NorthPlayer.ChoosenCharacters.First();
            exceptionsGame.Select(character);

            Assert.AreNotEqual(exceptionsGame.SelectedCharacter, character);
        }

        [TestMethod]
        public void User_IsNotTurnOwner_CanNotFinishTheTurn()
        {
            ExceptionsGame exceptionsGame = DefaultTestingGame.Create();

            exceptionsGame.EndTurn();

            Assert.AreEqual(DefaultTestingGame.NorthPlayer, exceptionsGame.TurnOwner);

            exceptionsGame.EndTurn();

            Assert.AreEqual(DefaultTestingGame.NorthPlayer, exceptionsGame.TurnOwner);
        }

        [TestMethod]
        public void User_IsNotTurnOwner_CanNotSelectOtherPlayerCharacters()
        {
            ExceptionsGame exceptionsGame = DefaultTestingGame.Create();

            exceptionsGame.EndTurn();

            var norhtCharacter = DefaultTestingGame.NorthPlayer.ChoosenCharacters.First();

            exceptionsGame.Select(norhtCharacter);

            Assert.AreNotEqual(exceptionsGame.SelectedCharacter, norhtCharacter);
        }
    }
}
