using System;

namespace renade
{
    class PassValueTooBigException : Exception
    {
        public PassValueTooBigException(PassType passType, int value)
            : base(string.Format("Pass of type {0} can not have the value of {1}", passType, value)) { }
    }
}