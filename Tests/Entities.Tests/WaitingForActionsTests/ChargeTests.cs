using Entities.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entities.Tests.WaitingForActionsTests
{
    [TestClass]
    public class ChargeTests
    {
        [TestMethod]
        public void SelectCharacter_ReachableEnemy_MarkAsCanBeCharged()
        {
            const int columns = 7;
            const int rows = 7;
            var simplifiedMap = new Map(columns,rows);

            var cylcops = new Character {Faction = Faction.South};
            simplifiedMap.AddCharacter(cylcops, 3, 6);
            var magneto = new Character { Faction = Faction.North };
            simplifiedMap.AddCharacter(magneto, 3, 0);

            var state = new WaitingForActions(simplifiedMap);

            state.Select(cylcops);

            Assert.IsTrue(magneto.CanBeCharged);
        }

        [TestMethod]
        public void SelectCharacter_ReachableEnemy_NoFreePositionInFront_NOTMarkAsCanBeCharged()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SelectCharacter_ReachableEnemy_FreePositionOnSides_MarkAsCanBeCharged()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SelectCharacter_ReachableEnemy_NoFreePositionOnSides_NotMarkAsCanBeCharged()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SelectCharacter_ReachableEnemy_FreePositionOnBack_MarkAsCanBeCharged()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SelectCharacter_ReachableEnemy_NoFreePositionOnBack_NotMarkAsCanBeCharged()
        {
            Assert.Inconclusive();
        }
    }
}
