using System;

namespace renade
{
    class FailedToRemovePlayerFromOnlineListException : Exception
    {
        public FailedToRemovePlayerFromOnlineListException(string socialClubName, int foundPlayers)
            : base(string.Format("Couldn't remove {0} from online list. Found players in the list: {1}", socialClubName, foundPlayers)) { }
    }
}