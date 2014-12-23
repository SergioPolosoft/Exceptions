using System;
using Entities.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entities.Tests
{
    [TestClass]
    public class CharacterSelectedStateTests
    {
        private CharacterSelectedState state;

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void New_MapIsNull_ExceptionIsThrown()
        {
            state = new CharacterSelectedState(null);
        }

    }
}
