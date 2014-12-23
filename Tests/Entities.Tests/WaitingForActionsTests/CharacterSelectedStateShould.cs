﻿using Entities.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entities.Tests.WaitingForActionsTests
{
    [TestClass]
    public class CharacterSelectedStateShould
    {
        private const int HorizontalCenter = 3;
        private const int FirstRow = 0;
        private const int LastRow = 6;
        
        private Map simplifiedMap;
        private Character cylcops;
        private CharacterSelectedState state;

        [TestInitialize]
        public void TestInitialize()
        {
            const int columns = 7;
            const int rows = 7;
            simplifiedMap = new Map(columns, rows);

            cylcops = new Character { Faction = Faction.South };
            simplifiedMap.AddCharacter(cylcops, HorizontalCenter, LastRow);

            state = new CharacterSelectedState(simplifiedMap);
        }

        [TestMethod]
        public void MarkAReachableEnemy_AsCanBeCharged_WhenItIsSelected()
        {
            var magneto = new Character { Faction = Faction.North };
            simplifiedMap.AddCharacter(magneto, HorizontalCenter, FirstRow);

            state.Select(cylcops);

            Assert.IsTrue(magneto.CanBeCharged);
        }

        [TestMethod]
        public void DoesNotMarkAReachableEnemy_AsCanBeCharged_WhenItIsSelected_IfThereIsNoFreePositionInFront()
        {
            var magneto = new Character { Faction = Faction.North };
            simplifiedMap.AddCharacter(magneto, HorizontalCenter, FirstRow);
            
            var beast = new Character { Faction = Faction.South };
            simplifiedMap.AddCharacter(beast, HorizontalCenter, FirstRow + 1);

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

            state.Select(cylcops);

            Assert.IsFalse(magneto.CanBeCharged);
        }

        [TestMethod]
        public void SetTheStateOfTheSelectedCharacterToChargingWhenAChargableEnemyIsSelected()
        {
            var magneto = new Character { Faction = Faction.North, Name = "Magneto" };
            simplifiedMap.AddCharacter(magneto, HorizontalCenter, FirstRow + 1);
            
            state.Select(cylcops);

            state.Select(magneto);

            Assert.AreEqual(CharacterStatus.Charging, cylcops.Status);
        }

        [TestMethod]
        public void SelectEnemyNotChargableCharacter_SelectedCharacterIsOnSelectedState()
        {
            var magneto = new Character { Faction = Faction.North, Name = "Magneto" };
            simplifiedMap.AddCharacter(magneto, HorizontalCenter, FirstRow);

            var beast = new Character { Faction = Faction.South };
            simplifiedMap.AddCharacter(beast, HorizontalCenter, FirstRow + 1);

            state.Select(cylcops);

            state.Select(magneto);

            Assert.AreEqual(CharacterStatus.Selected, cylcops.Status);
        }
        
        [TestMethod]
        public void SelectThePositionWithLowerCostAsTheSelectedPositionWhenAnEnemyCharacterIsSelected()
        {
            var magneto = new Character { Faction = Faction.North, Name = "Magneto" };
            simplifiedMap.AddCharacter(magneto, HorizontalCenter, FirstRow + 4);

            var beast = new Character { Faction = Faction.South, Name = "Beast" };
            simplifiedMap.AddCharacter(beast, HorizontalCenter - 3, FirstRow + 4);

            state.Select(beast);

            state.Select(magneto);

            var positionToCharge = state.SelectedPosition;

            Assert.AreEqual(FirstRow + 4, positionToCharge.Y);
            Assert.AreEqual(HorizontalCenter - 1, positionToCharge.X);
        }
    }
}