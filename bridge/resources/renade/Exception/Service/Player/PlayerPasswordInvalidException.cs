using System;

namespace renade
{
    class PlayerPasswordInvalidException : Exception
    {
        public PlayerPasswordInvalidException(string loginOrMail)
            : base(String.Format("Invalid password for player with a login or mail of '{0}'", loginOrMail)) { }
    }
}