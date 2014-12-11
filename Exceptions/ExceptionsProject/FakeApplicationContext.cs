using Security;

namespace ExceptionsProject
{
    public class FakeApplicationContext : IApplicationContext
    {
        private readonly User currentUser = new User("Sergio");

        public User GetCurrentUser()
        {
            return currentUser;
        }
    }
}