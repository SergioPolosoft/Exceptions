using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entities.Tests
{
    [TestClass]
    public class PathBuilderTests
    {
        private PathBuilder builder;
        private Character cyclops;
        private Map map;

        [TestInitialize]
        public void TestInitialize()
        {
            map = InitializeMap();
            cyclops = new Character {Velocity = 4};
            map.AddCharacterAtPosition(cyclops, map.GetPosition(5, 5));

            builder = new PathBuilder(map);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void PathBuilder_NullMap_ThrowException()
        {
            new PathBuilder(null);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void PathBuilder_EmptyMap_ThrowException()
        {
            new PathBuilder(new Map());
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void BuildPath_CharacterIsNull_ThrowException()
        {
            builder.GetPath(null, new Position(0, 0), 0);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void BuildPath_CharacterIsNotOnTheMap_ThrowException()
        {
            var character = new Character();
            builder.GetPath(character, new Position(0, 0), character.Velocity);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void BuildPath_PositionIsNull_ThrowException()
        {
            builder.GetPath(cyclops, null, cyclops.Velocity);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void BuildPath_PositionIsNotOnTheMap_ThrowException()
        {
            builder.GetPath(cyclops, new Position(0, 0), cyclops.Velocity);
        }

        [TestMethod]
        public void BuildPath_PositionIsCharacterPosition_ReturnsAnEmtpyList()
        {
            var position = map.GetPosition(cyclops);
            var path = builder.GetPath(cyclops, position as Position, cyclops.Velocity);

            Assert.IsNotNull(path);
            Assert.AreEqual(0, path.Count);
        }

        [TestMethod]
        public void BuildPath_PositionIsLongAwayFromCharacterVelocity_ReturnsAnEmptyList()
        {
            var cyclopsPosition = map.GetPosition(cyclops);
            var destinationPosition = map.GetPosition(cyclopsPosition.X + cyclops.Velocity,
                                                      cyclopsPosition.Y + cyclops.Velocity);

            var path = builder.GetPath(cyclops, destinationPosition as Position, cyclops.Velocity);

            Assert.IsNotNull(path);
            Assert.AreEqual(0, path.Count);
        }

        [TestMethod]
        public void BuildPath_ReturnsAPathToAnSpecificPosition()
        {
            var cyclopsPosition = map.GetPosition(cyclops);
            var destinationPosition = map.GetPosition(cyclopsPosition.X + 2,
                                                      cyclopsPosition.Y + 2);

            var path = builder.GetPath(cyclops, destinationPosition as Position, cyclops.Velocity);

            Assert.IsNotNull(path);
            Assert.AreEqual(4, path.Count);

            Assert.AreEqual(cyclopsPosition.Y + 1, path[0].Y);
            Assert.AreEqual(cyclopsPosition.X, path[0].X);

            Assert.AreEqual(cyclopsPosition.Y + 2, path[1].Y);
            Assert.AreEqual(cyclopsPosition.X, path[1].X);

            Assert.AreEqual(cyclopsPosition.Y + 2, path[2].Y);
            Assert.AreEqual(cyclopsPosition.X + 1, path[2].X);

            Assert.AreEqual(cyclopsPosition.Y + 2, path[3].Y);
            Assert.AreEqual(cyclopsPosition.X + 2, path[3].X);
        }

        [TestMethod]
        public void PathBuild_UnableToArrive_ReturnEmptyList()
        {
            var cyclopsPosition = map.GetPosition(cyclops);
            var destinationPosition = map.GetPosition(cyclopsPosition.X - 4,
                                                      cyclopsPosition.Y);

            var icemanPosition = map.GetPosition(cyclopsPosition.X - 3, cyclopsPosition.Y);
            map.AddCharacterAtPosition(new Character(), icemanPosition);

            var path = builder.GetPath(cyclops, destinationPosition as Position, cyclops.Velocity);

            Assert.AreEqual(0, path.Count);
        }

        private static Map InitializeMap()
        {
            var map = new Map();
            for (int x = 0; x < Map.Columns; x++)
            {
                for (int y = 0; y < Map.Rows; y++)
                {
                    map.AddPosition(new Position(x, y));
                }
            }
            return map;
        }
    }
}