using Security;
using Security.Fakes;

namespace Entities.Tests
{
    public class DefaultTestingGame
    {
        private static Player southPlayer;
        private static Player northPlayer;

        public static ExceptionsGame Create()
        {
            var applicationContext = new StubIApplicationContext();
            var southPlayerUser = new User("SouthPlayerUser");
            applicationContext.GetCurrentUser = () => southPlayerUser;
            southPlayer = new Player(southPlayerUser);
            northPlayer = new Player(new User("NorthPlayerUser"));
            for (int i = 0; i < 6; i++)
            {
                southPlayer.Add(new Character());
                northPlayer.Add(new Character());
            }
            
            var exceptionsGame = new GameFactory().NewGame(southPlayer, northPlayer,
                                                           applicationContext);
            return exceptionsGame;
        }

        public static Player SouthPlayer
        {
            get { return southPlayer; }
        }

        public static Player NorthPlayer
        {
            get { return northPlayer; }
        }
    }
}