using System;

namespace renade
{
    class CharacterFirstNameTooLongException : Exception
    {
        public CharacterFirstNameTooLongException(string name)
            : base(string.Format("First name '{0}' is too long; Allowed length: {1}", name, CharacterRepo.MaxCharacterFirstNameLength)) { }
    }
}