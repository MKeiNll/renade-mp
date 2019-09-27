using System;

namespace renade
{
    class PlayerSocialClubNameTooLongException : Exception
    {
        public PlayerSocialClubNameTooLongException(string name)
            : base(string.Format("Social club name '{0}' is too long; Allowed length: {1}", name, PlayerRepo.MaxPlayerSocialClubNameLength)) { }
    }
}