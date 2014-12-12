using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security;
using Security.Fakes;

namespace Entities.Tests.ExceptionsGameTests
{
    [TestClass]
    public class ChargeTests
    {
        private StubIApplicationContext applicationContext;
        private Character beast;
        private Character cyclops;
        private ExceptionsGame exceptionsGame;
        private Character quicksilver;
        private Character scarletWitch;
        private User southPlayerUser;

        [TestInitialize]
        public void TestInitialize()
        {
            applicationContext = new StubIApplicationContext();

            southPlayerUser = new User("xavier");
            var southPlayer = new Player(southPlayerUser);
            beast = new Character {Name = "beast"};
            southPlayer.Add(beast);
            cyclops = new Character {Name = "cyclops", Velocity = 4};
            southPlayer.Add(cyclops);

            var northPlayer = new Player(new User("erik"));
            quicksilver = new Character {Name = "quicksilver"};
            scarletWitch = new Character {Name = "scarletWitch"};
            northPlayer.Add(quicksilver);
            northPlayer.Add(scarletWitch);

            applicationContext.GetCurrentUser = () => southPlayerUser;
            exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer, applicationContext);
        }

        [TestMethod]
        public void SelectCharacter_AnEnemyCharacterOnPlus6Range_IsMarkedAsChargable()
        {
            var cyclopsPosition = exceptionsGame.GetPosition(cyclops);

            var quickSilverPosition = exceptionsGame.GetPosition(cyclopsPosition.X,
                                                                 cyclopsPosition.Y - 6);

            exceptionsGame.State.Map.AddCharacter(quicksilver, quickSilverPosition);

            exceptionsGame.Select(cyclops);

            Assert.IsTrue(quicksilver.CanBeCharged);
        }

        [TestMethod]
        public void SelectCharacter_AFriendCharacterOnPlus6Range_IsNotMarkedAsChargable()
        {
            var cyclopsPosition = exceptionsGame.GetPosition(cyclops);

            var beastPosition = exceptionsGame.GetPosition(cyclopsPosition.X,
                                                           cyclopsPosition.Y - 6);

            exceptionsGame.State.Map.AddCharacter(beast, beastPosition);

            exceptionsGame.Select(cyclops);

            Assert.IsFalse(beast.CanBeCharged);
        }

        [TestMethod]
        public void SelectCharacter_ChangeSelection_ClearChargableCharacters()
        {
            var cyclopsPosition = exceptionsGame.GetPosition(cyclops);

            var quickSilverPosition = exceptionsGame.GetPosition(cyclopsPosition.X,
                                                                 cyclopsPosition.Y - 6);

            exceptionsGame.State.Map.AddCharacter(quicksilver, quickSilverPosition);

            exceptionsGame.Select(cyclops);

            exceptionsGame.Select(beast);

            Assert.IsFalse(quicksilver.CanBeCharged);
        }

        [TestMethod]
        public void SelectCharacter_NotFreePositionInFrontAndNotReachable_IsNotMarkedAsChargeable()
        {
            var cyclopsPosition = exceptionsGame.GetPosition(cyclops);

            var quickSilverPosition = exceptionsGame.GetPosition(cyclopsPosition.X,
                                                                 cyclopsPosition.Y - 10);

            exceptionsGame.State.Map.AddCharacter(quicksilver, quickSilverPosition);

            var scarlteWitchPosition = exceptionsGame.GetPosition(quickSilverPosition.X, quickSilverPosition.Y + 1);

            exceptionsGame.State.Map.AddCharacter(scarletWitch, scarlteWitchPosition);

            exceptionsGame.Select(cyclops);

            Assert.IsTrue(scarletWitch.CanBeCharged);
            Assert.IsFalse(quicksilver.CanBeCharged);
        }

        [TestMethod]
        public void SelectChargableCharacter_GameStateIsMovingCharacters()
        {
            Assert.Inconclusive();
        }
    }
}