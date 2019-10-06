using System;

namespace renade
{
    class BanHwidTooLongException : Exception
    {
        public BanHwidTooLongException(string hwid)
            : base(string.Format("Hwid '{0}' is too long; Allowed length: {1}", hwid, BanRepo.MaxHwidLength)) { }
    }
}