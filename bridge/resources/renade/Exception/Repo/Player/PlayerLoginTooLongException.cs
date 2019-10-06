using System;

namespace renade
{
    class PlayerLoginTooLongException : Exception
    {
        public PlayerLoginTooLongException(string login)
            : base(string.Format("Login '{0}' is too long; Allowed length: {1}", login, PlayerRepo.MaxPlayerLoginLength)) { }
    }
}