using System;

namespace renade
{
    class PlayerSocialClubNameIsTakenException : Exception
    {
        public PlayerSocialClubNameIsTakenException(string socialClubName)
            : base(string.Format("Social club name {0} is taken", socialClubName)) { }
    }
}