using System;
using System.Collections.Generic;

namespace renade
{
    public class Character
    {
        public readonly PrimaryData PrimaryData;
        public readonly Pass Pass;
        public readonly List<PhoneContact> PhoneContacts;
        public readonly Appearance Appearance;

        public Character(PrimaryData primaryData, Pass pass, List<PhoneContact> phoneContacts, Appearance appearance)
        {
            PrimaryData = primaryData;
            PhoneContacts = phoneContacts;
            Appearance = appearance;
        }

        public override string ToString()
        {
            return String.Format("Character - Id: {0}", PrimaryData.Id);
        }
    }
}
