using System;
using Entities.NullObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entities.Tests
{
    [TestClass]
    public class MapTests
    {
        private Map map;
        const int columns = 2;
        const int rows = 2;
            
        [TestMethod]
        public void New_ColumnsAndRows_CreatesAMapTheseSizes()
        {
            map = new Map(columns, rows);

            const int columnsDimension = 0;
            const int rowsDimension = 1;
            
            var columnsCreated = map.Positions.GetLength(columnsDimension);
            var rowsCreated = map.Positions.GetLength(rowsDimension);

            Assert.AreEqual(columns, columnsCreated);
            Assert.AreEqual(rows, rowsCreated);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void New_ColumnsAndRows_ColumnIsZero_ThrowOutOfRangeException()
        {
            map = new Map(0, rows);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void New_ColumnsAndRows_ColumnIsNegativeNumber_ThrowOutOfRangeException()
        {
            map = new Map(-1, rows);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void New_ColumnsAndRows_RowIsZero_ThrowOutOfRangeException()
        {
            map = new Map(columns, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void New_ColumnsAndRows_RowIsNegative_ThrowOutOfRangeException()
        {
            map = new Map(columns, -1);
        }
        
        [TestMethod]
        public void AddCharacter_XPositionYPosition_LinkCharacterToThatPosition()
        {
            map = new Map();
            var character = new Character();
            map.AddCharacter(character, 1, 1);

            var insertedCharacter = map.GetCharacterAtPosition(1, 1);

            Assert.AreEqual(character, insertedCharacter);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddCharacter_XPositionYPosition_CharacterIsNull_ThrowArgumentNullException()
        {
            map = new Map();
            map.AddCharacter(null, 1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddCharacter_XPositionYPosition_XPositionIsNegative_ThrowOutOfRangeException()
        {
            map = new Map();
            map.AddCharacter(new Character(), -1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddCharacter_XPositionYPosition_YPositionIsNegative_ThrowOutOfRangeException()
        {
            map = new Map();
            map.AddCharacter(new Character(), 1, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddCharacter_XPositionYPosition_PositionNotExists_ThrowArgumentException()
        {
            map = new Map();
            map.AddCharacter(new Character(), 100, 1);
        }

        [TestMethod]
        public void AddCharacter_XPositionYPosition_PositionIsNotFree_NothingIsChanged()
        {
            map = new Map();

            var cyclops = new Character();
            map.AddCharacter(cyclops, 10, 10);

            var magneto = new Character();
            map.AddCharacter(magneto, 10, 10);

            var positionedCharacter = map.GetCharacterAtPosition(10, 10);

            Assert.AreEqual(cyclops, positionedCharacter);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetCharacter_XPositionYPosition_XPositionIsNegative_ThrowOutOfRangeException()
        {
            map = new Map();
            map.GetCharacterAtPosition(-1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetCharacter_XPositionYPosition_YPositionIsNegative_ThrowOutOfRangeException()
        {
            map = new Map();
            map.GetCharacterAtPosition(0, -1);
        }

        [TestMethod]
        public void GetCharacter_XPositionYPosition_PositionNotExist_ReturnNullCharacter()
        {
            map = new Map();
            var character = map.GetCharacterAtPosition(100, 0);
            Assert.IsInstanceOfType(character,typeof(NullCharacter));
        }
        
        [TestMethod]
        public void GetCharacter_XPositionYPosition_NoCharacterAtPosition()
        {
            map = new Map();
            var character = map.GetCharacterAtPosition(0, 0);
            Assert.IsInstanceOfType(character, typeof(NullCharacter));
        }
    }
}