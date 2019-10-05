using System;

namespace renade
{
    public class PhoneContact
    {
        public readonly int Phone;

        public PhoneContact(int phone)
        {
            Phone = phone;
        }

        public override string ToString()
        {
            return String.Format("Phone contact - {0}", Phone);
        }
    }
}
