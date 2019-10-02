using System;

namespace renade
{
    public enum PassType { Developer, Admin, Media, Regular };

    public class Character
    {
        public readonly string PlayerSocialClubName;
        public readonly int Id;
        public readonly string FirstName;
        public readonly string FamilyName;
        public readonly int Level;
        public readonly int PassId;
        public readonly PassType PassType;
        public readonly long RegDate;

        public readonly float PosX;
        public readonly float PosY;
        public readonly float PosZ;

        public readonly Appearance Appearance;

        public Character(string playerSocialClubName, int id, string firstName, string familyName, int level, int passId,
            PassType passType, long regDate, float posX, float posY, float posZ, Appearance appearance)
        {
            PlayerSocialClubName = playerSocialClubName;
            Id = id;
            FirstName = firstName;
            FamilyName = familyName;
            Level = level;
            PassId = passId;
            PassType = passType;
            RegDate = regDate;
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
            Appearance = appearance;
        }

        public override string ToString()
        {
            return String.Format("Character - Id: {0}; Player social club name: {1}; First Name: {2}; Family Name: {3}; Level: {4};" +
                "Pass Id: {5}; Pass Type: {6}; Gender: {7}; Registration Date: {8}; X: {9}; Y: {10}; Z: {11}",
                Id, PlayerSocialClubName, FirstName, FamilyName, Level, PassId, PassType, Gender, DateTimeOffset.FromUnixTimeMilliseconds(RegDate).ToLocalTime(), PosX, PosY, PosZ);
        }
    }
}
