using System;

namespace renade
{
    class CharacterPassDoesNotExistException : Exception
    {
        public CharacterPassDoesNotExistException(int characterId)
            : base(String.Format("Character pass does not exist for a character with an id of {0}", characterId)) { }
    }
}