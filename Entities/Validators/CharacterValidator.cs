using System;

namespace Entities.Validators
{
    internal class CharacterValidator : IValidator<Character>
    {
        public void Validate<T>(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("character");
            }
        }
    }
}