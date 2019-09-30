using System;
using System.Collections.Generic;

namespace renade
{
    public enum BanType { SocialClubName, Hwid };
    public enum BanCategory
    {
        None, HackCheat, BugUse, SteamOverlay, SpawnKill, DriveByKill, DeathMatch, 
        TeamKill, RevengeKill, Powergaming, Metagaming, Afk, CharacterKill, Custom
    }

    public class Ban
    {
        public readonly BanCategory Category;
        public readonly string SocialClubName;
        public readonly string Hwid;
        public readonly long BanValue;
        public readonly string Reason;

        public Ban(BanCategory category, string socialClubName, string hwid, long banValue, string reason)
        {
            Category = category;
            SocialClubName = socialClubName;
            Hwid = hwid;
            BanValue = banValue;
            Reason = reason;
        }

        public override string ToString()
        {
            return string.Format("Ban - Category: {0}; Social club name: {1}; Hwid: {2}; Ban value: {3}; Reason: {4}",
                Category, SocialClubName, Hwid, DateTimeOffset.FromUnixTimeMilliseconds(BanValue).ToLocalTime(), Reason); ;
        }
    }
}
