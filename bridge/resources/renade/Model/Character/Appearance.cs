using System;

namespace renade
{
    public enum Gender { Male, Female };

    // TODO - gay parents // not use enums?

    public enum Mother
    {
        Hannah = 21, Audrey = 22, Jasmine = 23, Giselle = 34, Amelia = 25,
        Isabella = 26, Zoe = 27, Ava = 28, Camilla = 29, Violet = 30, Sophia = 31,
        Eveline = 32, Nicole = 33, Ashley = 34, Grace = 35, Brianna = 36, Natalie = 37,
        Olivia = 38, Elizabeth = 39, Charlotte = 40, Emma = 41, Misty = 45
    };
    public enum Father
    {
        Benjamin = 0, Daniel = 1, Joshua = 2, Noah = 3, Andrew = 4, Joan = 5,
        Alex = 6, Isaac = 7, Evan = 8, Ethan = 9, Vincent = 10, Angel = 11, Diego = 12,
        Adrian = 13, Gabriel = 14, Michael = 15, Santiago = 16, Kevin = 17, Louis = 18,
        Samuel = 19, Anthony = 20, John = 42, Niko = 43, Claude = 44
    };
    public enum Hair { None, Buzzcut };
    public enum Eyebrows { None, Balanced };
    public enum Beard { None, LightStubble };
    public enum EyeColor { Green, Emerald };
    public enum HairColor { Red, Blue };

    public class Appearance
    {
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

        public Appearance(Gender gender, Mother mother, Father father, float similarity, float skinColor, float noseHeight, float noseWidth, float noseLength,
            float noseBridge, float noseTip, float noseBridgeTip, float browWidth, float browHeight, float cheekboneWidth, float cheekboneHeight,
            float cheeksWidth, float eyes, float lips, float jawWidth, float jawHeight, float chinLength, float chinPosition, float chinWidth, float chinShape,
            float neckWidth, Hair hair, Eyebrows eyebrows, Beard beard, EyeColor eyeColor, HairColor hairColor)
        {
            Gender = gender;
            Mother = mother;
            Father = father;
            Similarity = similarity;
            SkinColor = skinColor;
            NoseHeight = noseHeight;
            NoseWidth = noseWidth;
            NoseLength = noseLength;
            NoseBridge = noseBridge;
            NoseTip = noseTip;
            NoseBridgeTip = noseBridgeTip;
            BrowWidth = browWidth;
            BrowHeight = browHeight;
            CheekboneWidth = cheekboneWidth;
            CheekboneHeight = cheekboneHeight;
            CheeksWidth = cheeksWidth;
            Eyes = eyes;
            Lips = lips;
            JawWidth = jawWidth;
            JawHeight = jawHeight;
            ChinLength = chinLength;
            ChinPosition = chinPosition;
            ChinWidth = chinWidth;
            ChinShape = chinShape;
            NeckWidth = neckWidth;
            Hair = hair;
            Eyebrows = eyebrows;
            Beard = beard;
            EyeColor = eyeColor;
            HairColor = hairColor;
        }

        public override string ToString()
        {
            return String.Format("Gender: {0}; Father: {1}; Mother: {2}", Gender, Father, Mother);
        }
    }
}
