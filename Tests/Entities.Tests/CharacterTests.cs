using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entities.Tests
{
    [TestClass]
    public class CharacterTests
    {
        [TestMethod]
        public void Charge_CharacterStatusIsCharging()
        {
            var character = new Character();
            character.Charge();

            Assert.AreEqual(CharacterStatus.Charging, character.Status);
        }
    }
}
