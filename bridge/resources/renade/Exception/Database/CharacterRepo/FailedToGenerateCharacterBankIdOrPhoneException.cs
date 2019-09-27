using System;

namespace renade
{
    class FailedToGenerateCharacterBankIdOrPhoneException : Exception
    {
        public FailedToGenerateCharacterBankIdOrPhoneException(string resultString) 
            : base(String.Format("Failed to generate password or phone, resultString: {0}", resultString)) { }
    }
}