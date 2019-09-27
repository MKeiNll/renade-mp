using System;

namespace renade
{
    class PlayerPasswordTooLongException : Exception
    {
        public PlayerPasswordTooLongException(int length)
            : base(string.Format("Password too long. Length: {0}; Allowed length: {1}", length, PlayerRepo.MaxPlayerPasswordLength)) { }
    }
}