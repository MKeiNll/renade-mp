using System;
using System.Collections.Generic;

namespace renade
{
    class FailedToUpdatePlayerIpHistoryException : Exception
    {
        public FailedToUpdatePlayerIpHistoryException(string recentIp)
            : base(String.Format("Failed to update player ip history with a recent ip of '{0}'", recentIp)) { }
    }
}