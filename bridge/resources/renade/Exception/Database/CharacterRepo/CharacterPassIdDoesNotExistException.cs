using System;

namespace renade
{
    class CharacterPassIdDoesNotExistException : Exception
    {
        public CharacterPassIdDoesNotExistException(int id) 
            : base(String.Format("No pass id for character with an id of {0}", id)) { }
    }
}