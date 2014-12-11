namespace Entities.Validators
{
    public class ValidatorFacade
    {
        private static readonly IValidator<Character> characterValidator = new CharacterValidator();

        public static void Validate(ICharacter character)
        {
            characterValidator.Validate(character);
        }
    }
}