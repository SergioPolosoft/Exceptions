using System.Collections.Generic;
using Security;

namespace Entities
{
    public class Player
    {
        private readonly IList<Character> choosenCharacters;
        private readonly User user;

        public Player(User user)
        {
            this.choosenCharacters = new List<Character>();
            this.user = user;
        }

        public IList<Character> ChoosenCharacters
        {
            get { return choosenCharacters; }
        }

        public User User
        {
            get { return user; }
        }

        public void Add(Character character)
        {
            this.choosenCharacters.Add(character);
        }
    }
}