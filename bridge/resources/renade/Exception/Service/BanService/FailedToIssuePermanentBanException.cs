using System;

namespace renade
{
    class FailedToIssuePermanentBanException : Exception
    {
        public FailedToIssuePermanentBanException(string socialClubName)
            : base(String.Format("Failed to issue permanent ban to a player with a social club name of {0}", socialClubName)) { }
    }
}