using System;

namespace renade
{
    class PlayerLoginIsTakenException : Exception
    {
        public PlayerLoginIsTakenException(string login)
            : base(string.Format("Login {0} is taken", login)) { }
    }
}