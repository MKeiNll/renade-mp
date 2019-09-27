using System;

namespace renade
{
    class PlayerPasswordTooShortException : Exception
    {
        public PlayerPasswordTooShortException(int length)
            : base(string.Format("Password too short. Length: {0}; Allowed minimum length: {1}", length, PlayerRepo.MinPlayerPasswordLength)) { }
    }
}