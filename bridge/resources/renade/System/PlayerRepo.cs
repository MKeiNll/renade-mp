using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BC = BCrypt.Net.BCrypt;

namespace renade
{
    public class PlayerRepo
    {
        public const int MinPlayerPasswordLength = 8;
        public const int MaxPlayerPasswordLength = 24;
        public const int MaxPlayerLoginLength = 16;
        public const int MaxPlayerSocialClubNameLength = 16;
        public const int MaxPlayerEmailLength = 48;
        public const int MaxIpLength = 48;
        public const int MaxPlayerPromoCodeLength = 16; // TODO

        private const string InsertPlayerSql = "INSERT INTO player (login, social_club_name, mail, pass, reg_date) VALUES ('{0}', '{1}', '{2}', '{3}', {4});";
        private const string SelectPlayerByLoginSql = "SELECT social_club_name, mail, pass, reg_date FROM player WHERE login = '{0}';";
        private const string SelectPlayerByMailSql = "SELECT login, social_club_name, pass, reg_date FROM player WHERE mail = '{0}';";
        private const string SelectPlayerBySocialClubNameSql = "SELECT login, mail, pass, reg_date FROM player WHERE social_club_name = '{0}';";
        private const string SelectPlayerIpHistoryBySocialClubNameSql = "SELECT ip_history_1, ip_history_2, ip_history_3 FROM player WHERE social_club_name = '{0}';";
        private const string UpdatePlayerIpHistory1BySocialClubNameSql = "UPDATE player SET ip_history_1 = '{0}' WHERE social_club_name = '{1}';";
        private const string UpdatePlayerIpHistory2BySocialClubNameSql = "UPDATE player SET ip_history_2 = '{0}' WHERE social_club_name = '{1}';";
        private const string UpdatePlayerIpHistory3BySocialClubNameSql = "UPDATE player SET ip_history_3 = '{0}' WHERE social_club_name = '{1}';";
        private const string UpdatePlayerLatestIpChangeBySocialClubNameSql = "UPDATE player SET last_ip_change = {0} WHERE social_club_name = '{1}';";
        private const string DeletePlayerBySocialClubNameSql = "DELETE FROM player WHERE social_club_name = '{0}';";

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private readonly string ConnectionString;

        public PlayerRepo(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public bool CreateNewPlayer(string login, string socialClubName, string email, string password)
        {
            int passwordLength = password.Length;
            if (login.Length > MaxPlayerLoginLength)
                throw new PlayerLoginTooLongException(login);
            if (socialClubName.Length > MaxPlayerSocialClubNameLength)
                throw new PlayerSocialClubNameTooLongException(socialClubName);
            if (email.Length > MaxPlayerEmailLength)
                throw new PlayerMailTooLongException(email);
            if (passwordLength > MaxPlayerPasswordLength)
                throw new PlayerPasswordTooLongException(passwordLength);
            if (passwordLength < MinPlayerPasswordLength)
                throw new PlayerPasswordTooShortException(passwordLength);

            if (!IsPlayerSocialClubNameTaken(socialClubName))
            {
                if (!IsPlayerLoginTaken(login))
                {
                    if (!IsPlayerMailTaken(email))
                    {
                        string hashedPassword = BC.HashPassword(password);
                        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                        {
                            connection.Open();
                            using (MySqlCommand command = new MySqlCommand(string.Format(InsertPlayerSql,
                                login, socialClubName, email, hashedPassword, (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds), connection))
                            {
                                return command.ExecuteNonQuery() > 0;
                            }
                        }
                    }
                    else
                        throw new PlayerMailIsTakenException(email);
                }
                else
                    throw new PlayerLoginIsTakenException(login);
            }
            else
                throw new PlayerSocialClubNameIsTakenException(socialClubName);
        }

        public bool UpdatePlayerIpHistoryBySocialClubName(string socialClubName, string recentIp)
        {
            if (recentIp.Length > MaxIpLength)
                throw new PlayerIpTooLongException(recentIp);

            List<string> ipHistory = GetPlayerIpHistoryBySocialClubName(socialClubName);
            string ip1 = ipHistory[0];
            string ip2 = ipHistory[1];

            // Set ip what was latest to second
            if (ip1 != null)
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(string.Format(UpdatePlayerIpHistory2BySocialClubNameSql, ip1, socialClubName), connection))
                    {
                        if (command.ExecuteNonQuery() <= 0)
                            return false;
                    }
                }
            }

            // Set ip what was second to third
            if (ip2 != null)
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(string.Format(UpdatePlayerIpHistory3BySocialClubNameSql, ip2, socialClubName), connection))
                    {
                        if (command.ExecuteNonQuery() <= 0)
                            return false;
                    }
                }
            }

            // Set latest ip
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(UpdatePlayerIpHistory1BySocialClubNameSql, recentIp, socialClubName), connection))
                {
                    if (command.ExecuteNonQuery() <= 0)
                        return false;
                }
            }

            // Set latest ip change date
            if (ip1 != recentIp)
            {
                long currentMillis = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(string.Format(UpdatePlayerLatestIpChangeBySocialClubNameSql, currentMillis, socialClubName), connection))
                    {
                        if (command.ExecuteNonQuery() <= 0)
                            return false;
                    }
                }
            }

            return true;
        }

        public bool DeletePlayerBySocialClubName(string socialClubName)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(DeletePlayerBySocialClubNameSql, socialClubName), connection))
                {
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool IsPlayerLoginTaken(string login)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectPlayerByLoginSql, login), connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return true;
                        else
                            return false;
                    }
                }
            }

        }

        public bool IsPlayerMailTaken(string mail)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectPlayerByMailSql, mail), connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return true;
                        else
                            return false;
                    }
                }
            }
        }

        public bool IsPlayerSocialClubNameTaken(string socialClubName)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectPlayerBySocialClubNameSql, socialClubName), connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return true;
                        else
                            return false;
                    }
                }
            }
        }

        public Player GetPlayerByLogin(string login)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectPlayerByLoginSql, login), connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return new Player(login, reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetInt64(3));
                        else
                            return null;
                    }
                }
            }
        }

        public Player GetPlayerBySocialClubName(string socialClubName)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectPlayerBySocialClubNameSql, socialClubName), connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return new Player(reader.GetString(0), socialClubName, reader.GetString(1), reader.GetString(2), reader.GetInt64(3));
                        else
                            return null;
                    }
                }
            }
        }

        public Player GetPlayerByMail(string mail)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectPlayerByMailSql, mail), connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return new Player(reader.GetString(0), reader.GetString(1), mail, reader.GetString(2), reader.GetInt64(3));
                        else
                            return null;
                    }
                }
            }
        }

        public List<string> GetPlayerIpHistoryBySocialClubName(string socialClubName)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectPlayerIpHistoryBySocialClubNameSql, socialClubName), connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            List<string> ipHistory = new List<string>();
                            if (reader.IsDBNull(0))
                                ipHistory.Add(null);
                            else
                                ipHistory.Add(reader.GetString(0));
                            if (reader.IsDBNull(1))
                                ipHistory.Add(null);
                            else
                                ipHistory.Add(reader.GetString(1));
                            if (reader.IsDBNull(2))
                                ipHistory.Add(null);
                            else
                                ipHistory.Add(reader.GetString(2));
                            return ipHistory;
                        }
                        else
                            return new List<string>();
                    }
                }
            }
        }

        public bool IsPlayerPasswordValid(Player player, string password)
        {
            return BC.Verify(password, player.Password);
        }

        public void TestRepo() {
            Log.Info("Testing PlayerRepo...");
            Log.Info("Delete existing: " + DeletePlayerBySocialClubName("1234"));
            Log.Info("Create: " + CreateNewPlayer("1234", "1234", "1234", "12341234"));
            Log.Info("Get: " + GetPlayerByLogin("1234"));
            Log.Info("Get: " + GetPlayerByMail("1234"));
            Log.Info("Get: " + GetPlayerBySocialClubName("1234"));
            Log.Info("Update: " + UpdatePlayerIpHistoryBySocialClubName("1234", "ip"));
            Log.Info("Get: " + String.Join(", ", GetPlayerIpHistoryBySocialClubName("1234")));
            Log.Info("Taken: " + IsPlayerSocialClubNameTaken("1234"));
            Log.Info("Taken: " + IsPlayerLoginTaken("1234"));
            Log.Info("Taken: " + IsPlayerMailTaken("1234"));
            Log.Info("Valid: " + IsPlayerPasswordValid(GetPlayerByLogin("1234"), "12341234"));
            Log.Info("Delete: " + DeletePlayerBySocialClubName("1234"));
            Log.Info("Done.");
        }
    }
}