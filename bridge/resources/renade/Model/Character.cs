using System;
using System.Collections.Generic;

namespace renade
{
    public class Character
    {
        public readonly string PlayerSocialClubName;
        public readonly int Id;
        public readonly string FirstName;
        public readonly string FamilyName;
        public readonly int Level;
        public readonly long RegDate;
        public readonly int PhoneNumber;
        public readonly Pass Pass;
        public readonly List<PhoneContact> PhoneContacts;
        public readonly Appearance Appearance;

        public readonly float PosX;
        public readonly float PosY;
        public readonly float PosZ;

        public Character(string playerSocialClubName, int id, string firstName, string familyName, int level,
            Pass pass, List<PhoneContact> phoneContacts, Appearance appearance, long regDate, int phoneNumber, float posX, float posY, float posZ)
        {
            PlayerSocialClubName = playerSocialClubName;
            Id = id;
            FirstName = firstName;
            FamilyName = familyName;
            Level = level;
            Pass = pass;
            PhoneContacts = phoneContacts;
            Appearance = appearance;
            RegDate = regDate;
            PhoneNumber = phoneNumber;
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
        }

        public override string ToString()
        {
            return String.Format("Character - Id: {0}; Player social club name: {1}; First Name: {2}; Family Name: {3}; Level: {4};" +
                "Registration Date: {5}; X: {6}; Y: {7}; Z: {8}", Id, PlayerSocialClubName, FirstName, FamilyName, Level,
                DateTimeOffset.FromUnixTimeMilliseconds(RegDate).ToLocalTime(), PosX, PosY, PosZ);
        }
    }
}
