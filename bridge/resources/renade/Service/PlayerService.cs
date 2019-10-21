using System;
using GTANetworkAPI;

namespace renade
{
    public class PlayerService
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private readonly PlayerRepo PlayerRepo;

        public PlayerService(string connectionString)
        {
            PlayerRepo = new PlayerRepo(connectionString);
        }

        // TODO - authorized / unauthorized players // all logic of player lists to here // disallow null values in those lists

        public Player GetPlayer(string socialClubName, string loginOrMail, string password)
        {
            Player player = PlayerRepo.GetPlayerByLogin(loginOrMail);
            if (player == null)
                player = PlayerRepo.GetPlayerByMail(loginOrMail);
            if (player == null)
                throw new PlayerDoesNotExistException(loginOrMail);
            if (!PlayerRepo.IsPlayerPasswordValid(player, password))
                throw new PlayerPasswordInvalidException(loginOrMail);
            string playerSocialClubName = player.SocialClubName;
            if (playerSocialClubName != socialClubName)
                throw new PlayerSocialClubNameInvalidException(socialClubName, playerSocialClubName);
            return player;
        }

        public Player GetPlayer(string socialClubName)
        {
            return PlayerRepo.GetPlayerBySocialClubName(socialClubName);
        }

        public void CreatePlayer(string socialClubName, string login, string mail, string password1, string password2)
        {
            if (password1 != password2)
                throw new PlayerPasswordsDoNotMatchException(socialClubName);
            if (!PlayerRepo.CreateNewPlayer(login, socialClubName, mail, password1))
                throw new PlayerCouldNotBeCreatedException(login, socialClubName, mail);
        }

        public void UpdatePlayerIpHistory(string socialClubName, string ip)
        {
            if (!PlayerRepo.UpdatePlayerIpHistoryBySocialClubName(socialClubName, ip))
                throw new FailedToUpdatePlayerIpHistoryException(ip);
        }
    }
}