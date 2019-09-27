using System;

namespace renade
{
    public enum PassType { Developer, Admin, Media, Regular };
    public enum Gender { Male, Female };

    public class Character
    {
        public readonly string PlayerSocialClubName;
        public readonly int Id;
        public readonly string FirstName;
        public readonly string FamilyName;
        public readonly int Level;
        public readonly int PassId;
        public readonly PassType PassType;
        public readonly Gender Gender;
        public readonly long RegDate;
        public readonly float Pos_X;
        public readonly float Pos_Y;
        public readonly float Pos_Z;

        public Character(string playerSocialClubName, int id, string firstName, string familyName, int level, int passId, PassType passType, Gender gender, long regDate, float pos_x, float pos_y, float pos_z)
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
            Pos_X = pos_x;
            Pos_Y = pos_y;
            Pos_Z = pos_z;
        }

        public override string ToString()
        {
            return String.Format("Character - Id: {0}; Player social club name: {1}; First Name: {2}; Family Name: {3}; Level: {4}; Pass Id: {5}; Pass Type: {6}; Gender: {7}; Registration Date: {8}; X: {9}; Y: {10}; Z: {11}",
                Id, PlayerSocialClubName, FirstName, FamilyName, Level, PassId, PassType, Gender, DateTimeOffset.FromUnixTimeMilliseconds(RegDate).ToLocalTime(), Pos_X, Pos_Y, Pos_Z);
        }
    }
} 
