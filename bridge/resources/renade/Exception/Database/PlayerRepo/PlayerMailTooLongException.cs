using System;

namespace renade
{
    class PlayerMailTooLongException : Exception
    {
        public PlayerMailTooLongException(string mail)
            : base(string.Format("Mail '{0}' is too long; Allowed length: {1}", mail, PlayerRepo.MaxPlayerEmailLength)) { }
    }
}