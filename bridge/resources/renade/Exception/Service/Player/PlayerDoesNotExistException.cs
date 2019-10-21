using System;

namespace renade
{
    class PlayerDoesNotExistException : Exception
    {
        public PlayerDoesNotExistException(string loginOrMail)
            : base(String.Format("Player with a login or mail of '{0}' does not exist", loginOrMail)) { }
    }
}