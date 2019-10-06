using System;

namespace renade
{
    class CharacterAppearanceDoesNotExistException : Exception
    {
        public CharacterAppearanceDoesNotExistException(int characterId)
            : base(String.Format("Character appearance does not exist for a character with an id of {0}", characterId)) { }
    }
}