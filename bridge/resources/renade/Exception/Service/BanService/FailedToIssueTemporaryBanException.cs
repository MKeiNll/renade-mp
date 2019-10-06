using System;

namespace renade
{
    class FailedToIssueTemporaryBanException : Exception
    {
        public FailedToIssueTemporaryBanException(string socialClubName)
            : base(String.Format("Failed to issue temporary ban to a player with a social club name of {0}", socialClubName)) { }
    }
}