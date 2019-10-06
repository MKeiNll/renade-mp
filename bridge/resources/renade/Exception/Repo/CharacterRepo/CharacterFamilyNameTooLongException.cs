using System;

namespace renade
{
    class CharacterFamilyNameTooLongException : Exception
    {
        public CharacterFamilyNameTooLongException(string name)
            : base(string.Format("Family name '{0}' is too long; Allowed length: {1}", name, CharacterRepo.MaxCharacterFamilyNameLength)) { }
    }
}