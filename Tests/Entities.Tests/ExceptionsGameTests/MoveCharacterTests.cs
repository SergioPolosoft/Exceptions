using System;
using System.Collections.Generic;
using System.Threading;
using Entities.NullObjects;
using Entities.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security;
using Security.Fakes;

namespace Entities.Tests.ExceptionsGameTests
{
    [TestClass]
    public class MoveCharacterTests
    {
        private ExceptionsGame exceptionsGame;
        private Character characterToMove;
        private IPosition toMovePosition;
        private IPosition currentPosition;
        private Character anotherCharacter;

        [TestInitialize]
        public void TestInitialize()
        {
            characterToMove = new Character { Velocity = 1 };
            anotherCharacter = new Character {Velocity = 4};

            var southPlayerUser = new User("xavier");
            var southPlayer = new Player(southPlayerUser);
            southPlayer.Add(characterToMove);
            southPlayer.Add(anotherCharacter);
            var northPlayer = new Player(new User("xavier"));
            northPlayer.Add(new Character());
            var applicationContext = new StubIApplicationContext();
            applicationContext.GetCurrentUser = () => southPlayerUser;
            exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer, applicationContext);

            exceptionsGame.Select(characterToMove);

            currentPosition = exceptionsGame.GetPosition(characterToMove);

            toMovePosition = exceptionsGame.GetPosition(currentPosition.X, currentPosition.Y - 1);

            exceptionsGame.Select(toMovePosition);

        }

        [TestMethod]
        public void MoveCharacterToPosition_CharacterIsAssignedToNewPosition()
        {
            exceptionsGame.MoveCharacterToPosition(characterToMove, this.toMovePosition);

            var newPosition = exceptionsGame.GetPosition(characterToMove);

            Assert.AreEqual(this.toMovePosition, newPosition);
        }

        [TestMethod]
        public void MoveCharacterToPosition_OldPositionIsFree()
        {
            exceptionsGame.MoveCharacterToPosition(characterToMove, toMovePosition);

            var character = exceptionsGame.GetCharacterAtPosition(currentPosition);

            Assert.IsInstanceOfType(character,typeof(NullCharacter));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MoveCharacterToPosition_CharacterNotExist_ThrowArgumentException()
        {
            exceptionsGame.MoveCharacterToPosition(new Character(), toMovePosition);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MoveCharacterToPosition_CharacterIsNull_ThrowNullArgumentException()
        {
            exceptionsGame.Select(toMovePosition);
            exceptionsGame.MoveCharacterToPosition(null,toMovePosition);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MoveCharacterToPosition_PositionNotExist_ThrowArgumentException()
        {
            exceptionsGame.MoveCharacterToPosition(characterToMove,new Position(15,0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MoveCharacterToPosition_PositionNull_ThrowNullArgumentException()
        {
            exceptionsGame.MoveCharacterToPosition(characterToMove,null);
        }
        
        [TestMethod]
        public void MoveCharacterState_CharacterNotInPosition_StateIsMoveCharacter()
        {
            exceptionsGame.Select(toMovePosition);

            var positionToMove = exceptionsGame.GetPosition(currentPosition.X, currentPosition.Y - characterToMove.Velocity + 1);

            exceptionsGame.MoveCharacterToPosition(characterToMove, positionToMove);

            Assert.IsInstanceOfType(exceptionsGame.State, typeof(MovingCharacter));
        }

        [TestMethod]
        public void MoveCharacterState_CharacterInPosition_StateIsWaitingForActions()
        {
            exceptionsGame.Select(toMovePosition);

            var positionToMove = exceptionsGame.GetPosition(currentPosition.X, currentPosition.Y - characterToMove.Velocity);

            exceptionsGame.MoveCharacterToPosition(characterToMove, positionToMove);

            Assert.IsInstanceOfType(exceptionsGame.State, typeof(WaitingForActions));
        }

        [TestMethod]
        public void MoveCharacterState_CharacterInPosition_SelectedPositionIsCleared()
        {
            exceptionsGame.Select(toMovePosition);

            var positionToMove = exceptionsGame.GetPosition(currentPosition.X, currentPosition.Y - characterToMove.Velocity);

            exceptionsGame.MoveCharacterToPosition(characterToMove, positionToMove);

            Assert.IsInstanceOfType(exceptionsGame.SelectedPosition,typeof(NullPosition));
        }

        [TestMethod]
        public void MoveCharacterState_CharacterInPosition_CharacterIsMarkAsMoved()
        {
            exceptionsGame.Select(toMovePosition);

            var positionToMove = exceptionsGame.GetPosition(currentPosition.X, currentPosition.Y - characterToMove.Velocity);

            exceptionsGame.MoveCharacterToPosition(characterToMove, positionToMove);

            Assert.IsTrue(characterToMove.Moved);
        }

        [TestMethod]
        public void MoveCharacterState_CharactersCannotBeSelected()
        {
            exceptionsGame.Select(toMovePosition);

            exceptionsGame.Select(anotherCharacter);

            Assert.AreNotEqual(anotherCharacter, exceptionsGame.SelectedCharacter);
        }

        [TestMethod]
        public void MoveCharacterState_PositionsCannotBeSelected()
        {
            exceptionsGame.Select(toMovePosition);

            var anotherPosition = exceptionsGame.GetPosition(currentPosition.X, currentPosition.Y);

            exceptionsGame.Select(anotherPosition);

            Assert.AreNotEqual(anotherPosition, exceptionsGame.SelectedPosition);
        }

        [TestMethod]
        public void CharacterMoved_WhenSelected_NoAvailablePositionsAreCalculatd()
        {
            exceptionsGame.MoveCharacterToPosition(characterToMove, toMovePosition);

            exceptionsGame.Select(anotherCharacter);

            exceptionsGame.Select(characterToMove);

            var up = exceptionsGame.GetPosition(toMovePosition.X - characterToMove.Velocity, toMovePosition.Y);
            var left = exceptionsGame.GetPosition(toMovePosition.X, toMovePosition.Y - characterToMove.Velocity);
            var right = exceptionsGame.GetPosition(toMovePosition.X, toMovePosition.Y + characterToMove.Velocity);
            var down = exceptionsGame.GetPosition(toMovePosition.X + characterToMove.Velocity, toMovePosition.Y);

            ValidateIsNotSelectable(up);
            ValidateIsNotSelectable(left);
            ValidateIsNotSelectable(right);
            ValidateIsNotSelectable(down);
        }

        private static void ValidateIsNotSelectable(IPosition position)
        {
            if (position is Position)
            {
                Assert.IsFalse(position.Selectable, "Tile {0} is still selected.",position.Name);
            }
        }
    }
}
