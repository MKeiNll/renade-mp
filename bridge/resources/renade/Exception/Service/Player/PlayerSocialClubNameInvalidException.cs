using System;

namespace renade
{
    class PlayerSocialClubNameInvalidException : Exception
    {
        public PlayerSocialClubNameInvalidException(string socialClubName, string playerSocialClubName)
            : base(String.Format("Invalid social club name of '{0}' for a player with social club name of '{1}'", socialClubName, playerSocialClubName)) { }
    }
}