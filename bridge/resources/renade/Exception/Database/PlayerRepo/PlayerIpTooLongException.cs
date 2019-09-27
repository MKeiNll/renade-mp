using System;

namespace renade
{
    class PlayerIpTooLongException : Exception
    {
        public PlayerIpTooLongException(string ip)
            : base(string.Format("Ip '{0}' is too long; Allowed length: {1}", ip, PlayerRepo.MaxIpLength)) { }
    }
}