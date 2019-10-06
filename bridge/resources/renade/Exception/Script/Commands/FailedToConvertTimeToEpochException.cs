using System;

namespace renade
{
    class FailedToConvertTimeToEpochException : Exception
    {
        public FailedToConvertTimeToEpochException(string input)
            : base(string.Format("Failed to convert time to epoch. Input: {0}", input)) { }
    }
}