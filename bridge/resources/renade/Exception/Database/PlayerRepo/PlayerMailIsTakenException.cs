using System;

namespace renade
{
    class PlayerMailIsTakenException : Exception
    {
        public PlayerMailIsTakenException(string mail)
            : base(string.Format("Mail {0} is taken", mail)) { }
    }
}