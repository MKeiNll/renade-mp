using System;

namespace renade
{
    class FailedToCreateCharacterAppearanceException : Exception
    {
        public FailedToCreateCharacterAppearanceException(int characterId)
            : base(String.Format("Failed to create character appearance for a character with an id of {0}", characterId)) { }
    }
}