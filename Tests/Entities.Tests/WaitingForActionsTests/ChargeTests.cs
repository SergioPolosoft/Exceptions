using Entities.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entities.Tests.WaitingForActionsTests
{
    [TestClass]
    public class ChargeTests
    {
        private const int HorizontalCenter = 3;
        private const int FirstRow = 0;
        private const int LastRow = 6;
        
        private Map simplifiedMap;
        private Character cylcops;

        [TestInitialize]
        public void TestInitialize()
        {
            const int columns = 7;
            const int rows = 7;
            simplifiedMap = new Map(columns, rows);

            cylcops = new Character { Faction = Faction.South };
            simplifiedMap.AddCharacter(cylcops, HorizontalCenter, LastRow);
        }

        [TestMethod]
        public void SelectCharacter_ReachableEnemy_MarkAsCanBeCharged()
        {
            var magneto = new Character { Faction = Faction.North };
            simplifiedMap.AddCharacter(magneto, HorizontalCenter, FirstRow);

            var state = new WaitingForActions(simplifiedMap);

            state.Select(cylcops);

            Assert.IsTrue(magneto.CanBeCharged);
        }

        [TestMethod]
        public void SelectCharacter_ReachableEnemy_NoFreePositionInFront_NOTMarkAsCanBeCharged()
        {
            var magneto = new Character { Faction = Faction.North };
            simplifiedMap.AddCharacter(magneto, HorizontalCenter, FirstRow);
            
            var beast = new Character { Faction = Faction.South };
            simplifiedMap.AddCharacter(beast, HorizontalCenter, FirstRow + 1);

            var state = new WaitingForActions(simplifiedMap);

            state.Select(cylcops);

            Assert.IsFalse(magneto.CanBeCharged);
        }

        [TestMethod]
        public void SelectCharacter_ReachableEnemy_FreePositionOnLeftSide_MarkAsCanBeCharged()
        {
            var magneto = new Character { Faction = Faction.North, Name = "Magneto"};
            simplifiedMap.AddCharacter(magneto, HorizontalCenter, FirstRow + 1);

            var beast = new Character { Faction = Faction.South, Name = "Beast" };
            simplifiedMap.AddCharacter(beast, HorizontalCenter, FirstRow + 2);

            var blob = new Character { Faction = Faction.North, Name = "Blob" };
            simplifiedMap.AddCharacter(blob, HorizontalCenter + 1, FirstRow + 1);

            var state = new WaitingForActions(simplifiedMap);

            state.Select(cylcops);

            Assert.IsTrue(magneto.CanBeCharged);
        }

        [TestMethod]
        public void SelectCharacter_ReachableEnemy_NoFreePositionOnSides_NotMarkAsCanBeCharged()
        {
            var magneto = new Character { Faction = Faction.North, Name = "Magneto" };
            simplifiedMap.AddCharacter(magneto, HorizontalCenter, FirstRow + 1);

            var beast = new Character { Faction = Faction.South, Name = "Beast" };
            simplifiedMap.AddCharacter(beast, HorizontalCenter, FirstRow + 2);

            var blob = new Character { Faction = Faction.North, Name = "Blob" };
            simplifiedMap.AddCharacter(blob, HorizontalCenter + 1, FirstRow + 1);

            var toad = new Character { Faction = Faction.North, Name = "Toad" };
            simplifiedMap.AddCharacter(toad, HorizontalCenter - 1, FirstRow + 1);

            var state = new WaitingForActions(simplifiedMap);

            state.Select(cylcops);

            Assert.IsFalse(magneto.CanBeCharged);
        }

        [TestMethod]
        public void SelectCharacter_ReachableEnemy_FreePositionOnRightSide_MarkAsCanBeCharged()
        {
            var magneto = new Character { Faction = Faction.North, Name = "Magneto" };
            simplifiedMap.AddCharacter(magneto, HorizontalCenter, FirstRow + 1);

            var beast = new Character { Faction = Faction.South, Name = "Beast" };
            simplifiedMap.AddCharacter(beast, HorizontalCenter, FirstRow + 2);

            var blob = new Character { Faction = Faction.North, Name = "Blob" };
            simplifiedMap.AddCharacter(blob, HorizontalCenter - 1, FirstRow + 1);

            var state = new WaitingForActions(simplifiedMap);

            state.Select(cylcops);

            Assert.IsTrue(magneto.CanBeCharged);
        }

        [TestMethod]
        public void SelectCharacter_ReachableEnemy_FreePositionOnBack_MarkAsCanBeCharged()
        {
            // Increase velocity to reach the objective
            cylcops.Velocity = 10;
            
            var magneto = new Character { Faction = Faction.North, Name = "Magneto" };
            simplifiedMap.AddCharacter(magneto, HorizontalCenter, FirstRow + 4);

            var beast = new Character { Faction = Faction.South, Name = "Beast" };
            simplifiedMap.AddCharacter(beast, HorizontalCenter, FirstRow + 5);

            var blob = new Character { Faction = Faction.North, Name = "Blob" };
            simplifiedMap.AddCharacter(blob, HorizontalCenter + 1, FirstRow + 4);

            var toad = new Character { Faction = Faction.North, Name = "Toad" };
            simplifiedMap.AddCharacter(toad, HorizontalCenter - 1, FirstRow + 4);

            var state = new WaitingForActions(simplifiedMap);

            state.Select(cylcops);

            Assert.IsTrue(magneto.CanBeCharged);
        }

        [TestMethod]
        public void SelectCharacter_ReachableEnemy_NoFreePositionOnBack_NotMarkAsCanBeCharged()
        {
            // Increase velocity to reach the objective
            cylcops.Velocity = 10;

            var magneto = new Character { Faction = Faction.North, Name = "Magneto" };
            simplifiedMap.AddCharacter(magneto, HorizontalCenter, FirstRow + 4);

            var beast = new Character { Faction = Faction.South, Name = "Beast" };
            simplifiedMap.AddCharacter(beast, HorizontalCenter, FirstRow + 5);

            var blob = new Character { Faction = Faction.North, Name = "Blob" };
            simplifiedMap.AddCharacter(blob, HorizontalCenter + 1, FirstRow + 4);

            var toad = new Character { Faction = Faction.North, Name = "Toad" };
            simplifiedMap.AddCharacter(toad, HorizontalCenter - 1, FirstRow + 4);

            var angel = new Character { Faction = Faction.South, Name = "Angel" };
            simplifiedMap.AddCharacter(angel, HorizontalCenter, FirstRow + 3);

            var state = new WaitingForActions(simplifiedMap);

            state.Select(cylcops);

            Assert.IsFalse(magneto.CanBeCharged);
        }

        [TestMethod]
        public void SelectEnemyChargableCharacter_SelectedCharacterIsOnChargingState()
        {
            var magneto = new Character { Faction = Faction.North, Name = "Magneto" };
            simplifiedMap.AddCharacter(magneto, HorizontalCenter, FirstRow);
            
            var state = new WaitingForActions(simplifiedMap);

            state.Select(cylcops);

            state.Select(magneto);

            Assert.AreEqual(CharacterStatus.Charging, cylcops.Status);
        }

        [TestMethod]
        public void SelectEnemyNotChargableCharacter_SelectedCharacterIsOnSelectedState()
        {
            Assert.Inconclusive();
        }


        [TestMethod]
        public void SelectFriendCharacter_SelectedCharacterIsOnSelectedState()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SelectChargableCharacter_SelectedPositionIsTheChargablePositionWithLowerCost()
        {
            Assert.Inconclusive();
        }
    }
}
