using System;

namespace renade
{
    class FailedToCreateCharacterPassException : Exception
    {
        public FailedToCreateCharacterPassException(int characterId)
            : base(String.Format("Failed to create character pass for a character with an id of {0}", characterId)) { }
    }
}