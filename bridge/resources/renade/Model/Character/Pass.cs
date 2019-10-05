using System;

namespace renade
{
    public enum PassType { Developer, Admin, Media, Regular };

    public class Pass
    {
        public const int DeveloperPassOffset = 0;
        public const int AdminPassOffset = 11;
        public const int MediaPassOffset = 101;
        public const int RegularPassOffset = 1001;

        public readonly int Id;
        public readonly PassType PassType;

        public Pass(int id, PassType passType)
        {
            switch (passType)
            {
                case PassType.Developer:
                    Id = id + DeveloperPassOffset;
                    break;
                case PassType.Admin:
                    Id = id + AdminPassOffset;
                    break;
                case PassType.Media:
                    Id = id + MediaPassOffset;
                    break;
                case PassType.Regular:
                    Id = id + RegularPassOffset;
                    break;
            }
            PassType = passType;
        }

        public override string ToString()
        {
            return String.Format("Pass - Id: {0}; Type: {1}", Id, PassType);
        }
    }
}
