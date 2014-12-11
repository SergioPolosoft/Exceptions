using System;

namespace Security
{
    public class User
    {
        private string username;
        private Guid id;

        public User(string username)
        {
            this.username = username;
            this.id = Guid.NewGuid();
        }
    }
}