using System;

namespace renade
{
    class BanReasonTooLongException : Exception
    {
        public BanReasonTooLongException(string reason)
            : base(string.Format("Ban reason '{0}' is too long; Allowed length: {1}", reason, BanRepo.MaxBanReasonLength)) { }
    }
}