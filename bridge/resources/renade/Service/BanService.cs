using System;
using GTANetworkAPI;

namespace renade
{
    public class BanService
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private readonly BanRepo BanRepo;

        public BanService(string connectionString)
        {
            BanRepo = new BanRepo(connectionString);
        }

        public bool IsPlayerBanned(Client player)
        {
            Ban socialClubNameBan = BanRepo.GetBanBySocialClubName(player.SocialClubName);
            if (socialClubNameBan != null && socialClubNameBan.BanValue != 0)
                return true;
            Ban hwidBan = BanRepo.GetBanByHwid(player.Serial);
            if (hwidBan != null && hwidBan.BanValue != 0)
                return true;
            return false;
        }

        public void IssuePermanentBan(Client player, string reason, BanCategory category)
        {
            string socialClubName = player.SocialClubName;
            if (!BanRepo.CreateNewBan(player.Serial, reason, category, -1, socialClubName))
                throw new FailedToIssuePermanentBanException(socialClubName);
            Log.Info("{0} has been permanently banned by social club name & hwid. Reason: '{1}'; Category: {2}", socialClubName, reason, category);
            KickBannedPlayer(player, reason, category, -1);

        }

        public void IssueTemporaryBan(Client player, string reason, BanCategory category, long unbanDate)
        {
            string socialClubName = player.SocialClubName;
            if (!BanRepo.CreateNewBan(player.Serial, reason, category, unbanDate, socialClubName))
                throw new FailedToIssueTemporaryBanException(socialClubName);
            Log.Info("{0} has been temporarily banned by social club name & hwid. Unban date: {1}; Reason: '{2}'; Category: {3}",
                socialClubName, DateTimeOffset.FromUnixTimeMilliseconds(unbanDate).ToLocalTime(), reason, category);
            KickBannedPlayer(player, reason, category, unbanDate);
        }

        public void RemovePlayerBanBySocialClubName(string socialClubName)
        {
            if (!BanRepo.RemoveBanBySocialClubName(socialClubName))
                throw new FailedToRemoveTemporaryBanException(socialClubName);
            Log.Info("{0} has been unbanned.", socialClubName);
        }

        public void RemoveTemporaryBans()
        {
            int bansRemoved = 0;
            Log.Info("Removing temporary bans...");
            foreach (Ban ban in BanRepo.GetAllTemporaryBans())
                if (ban.BanValue < DateTimeOffset.Now.ToUnixTimeMilliseconds())
                {
                    Log.Info("Found ban: {0}.", ban);
                    RemovePlayerBanBySocialClubName(ban.SocialClubName);
                    bansRemoved++;
                }
            Log.Info("{0} ban(s) removed.", bansRemoved);
        }

        public void KickPlayer(Client player, string reason)
        {
            player.TriggerEvent("kickBrowser");
            player.Kick(reason);
            Log.Info("{0} has been kicked with the reason: '{1}'", player.SocialClubName, reason);
        }

        private void KickBannedPlayer(Client player, String reason, BanCategory category, long unbanDate)
        {
            player.TriggerEvent("kickBrowser");
            string unbanDateString = unbanDate == -1 ? "never" : DateTimeOffset.FromUnixTimeMilliseconds(unbanDate).ToLocalTime().ToString();
            player.Kick(string.Format("Permanent ban. Reason: {0}; Category: {1}; Unban date: {2}", reason, category, unbanDateString));
            Log.Info("{0} has been kicked with the reason of ban.", player.SocialClubName);
        }
    }
}