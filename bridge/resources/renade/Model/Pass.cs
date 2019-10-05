using System;

namespace renade
{
    public enum PassType { Developer, Admin, Media, Regular };

    public class Pass
    {
        public readonly int Id;
        public readonly PassType PassType;

        public Pass(int id, PassType passType)
        {
            Id = id;
            PassType = passType;
        }

        public override string ToString()
        {
            return String.Format("Pass - Id: {0}; Type: {1}", Id, PassType);
        }
    }
}
