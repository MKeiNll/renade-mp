using System;

namespace renade
{
    class FailedToRemoveTemporaryBanException : Exception
    {
        public FailedToRemoveTemporaryBanException(string socialClubName)
            : base(String.Format("Failed remove ban from a player with social club name of {0}", socialClubName)) { }
    }
}