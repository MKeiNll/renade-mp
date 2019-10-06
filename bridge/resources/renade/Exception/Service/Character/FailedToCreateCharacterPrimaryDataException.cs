using System;

namespace renade
{
    class FailedToCreateCharacterPrimaryDataException : Exception
    {
        public FailedToCreateCharacterPrimaryDataException(string playerSocialClubName)
            : base(String.Format("Failed to create character primary data for a player '{0}'", playerSocialClubName)) { }
    }
}