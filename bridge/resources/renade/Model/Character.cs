using System;

namespace renade
{
    public enum PassType { Developer, Admin, Media, Regular };
    public enum Gender { Male, Female };
    public enum Mother { Hannah, Aubrey };
    public enum Father { Benjamin, Daniel };
    public enum Hair { None, Buzzcut };
    public enum Eyebrows { None, Balanced };
    public enum Beard { None, LightStubble };
    public enum EyeColor { Green, Emerald };
    public enum HairColor { Red, Blue };

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

        public readonly Gender Gender;
        public readonly Mother Mother;
        public readonly Father Father;
        public readonly float Similarity;
        public readonly float SkinColor;
        public readonly float NoseHeight;
        public readonly float NoseWidth;
        public readonly float NoseLength;
        public readonly float NoseBridge;
        public readonly float NoseTip;
        public readonly float NoseBridgeTip;
        public readonly float BrowWidth;
        public readonly float BrowHeight;
        public readonly float CheekboneWidth;
        public readonly float CheekboneHeight;
        public readonly float CheeksWidth;
        public readonly float Eyes;
        public readonly float Lips;
        public readonly float JawWidth;
        public readonly float JawHeight;
        public readonly float ChinLength;
        public readonly float ChinPosition;
        public readonly float ChinWidth;
        public readonly float ChinShape;
        public readonly float NeckWidth;
        public readonly Hair Hair;
        public readonly Eyebrows Eyebrows;
        public readonly Beard Beard;
        public readonly EyeColor EyeColor;
        public readonly HairColor HairColor;

        public readonly float PosX;
        public readonly float PosY;
        public readonly float PosZ;

        public Character(string playerSocialClubName, int id, string firstName, string familyName, int level, int passId,
            PassType passType, Gender gender, long regDate, float posX, float posY, float posZ)
        {
            PlayerSocialClubName = playerSocialClubName;
            Id = id;
            FirstName = firstName;
            FamilyName = familyName;
            Level = level;
            PassId = passId;
            PassType = passType;
            Gender = gender;
            RegDate = regDate;
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
        }

        public override string ToString()
        {
            return String.Format("Character - Id: {0}; Player social club name: {1}; First Name: {2}; Family Name: {3}; Level: {4};" +
                "Pass Id: {5}; Pass Type: {6}; Gender: {7}; Registration Date: {8}; X: {9}; Y: {10}; Z: {11}",
                Id, PlayerSocialClubName, FirstName, FamilyName, Level, PassId, PassType, Gender, DateTimeOffset.FromUnixTimeMilliseconds(RegDate).ToLocalTime(), PosX, PosY, PosZ);
        }
    }
}
