using System;

namespace renade
{
    class PlayerPasswordsDoNotMatchException : Exception
    {
        public PlayerPasswordsDoNotMatchException(string socialClubName)
            : base(String.Format("Passwords do not match for a player with a social club name of '{0}'", socialClubName)) { }
    }
}