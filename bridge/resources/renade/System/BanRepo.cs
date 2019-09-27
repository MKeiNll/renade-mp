using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace renade
{
    public class BanRepo
    {
        public const int MaxBanReasonLength = 128;
        public const int MaxHwidLength = 128;
        
        private const string SelectBanBySocialClubNameSql = "SELECT ban, reason, category, hwid FROM ban WHERE social_club_name = '{0}';";
        private const string SelectBanByHwidSql = "SELECT ban, reason, category, social_club_name FROM ban WHERE hwid = '{0}';";
        private const string InsertBanSql = "INSERT INTO ban (social_club_name, hwid, reason, category, ban) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');";
        private const string SelectAllTemporaryBansSql = "SELECT social_club_name, hwid, reason, category, ban FROM ban WHERE ban > 0;";
        private const string DeleteBanBySocialClubNameSql = "DELETE FROM ban WHERE social_club_name = '{0}';";

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private readonly string ConnectionString;

        public BanRepo(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public bool CreateNewBan(string hwid, string reason, BanCategory category, long ban, string socialClubName)
        {
            if (hwid.Length > MaxHwidLength)
                throw new BanHwidTooLongException(hwid);
            if (reason.Length > MaxBanReasonLength)
                throw new BanReasonTooLongException(reason);

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(InsertBanSql, socialClubName, hwid, reason, (int)category, ban), connection))
                {
                    try
                    {
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch (MySqlException e)
                    {
                        if (e.Number == 1062)
                        {
                            Log.Warn("Cannot ban {0} - he is already banned.", socialClubName);
                            return true;
                        }
                        throw;
                    }
                }
            }
        }

        public bool RemoveBanBySocialClubName(string socialClubName)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(DeleteBanBySocialClubNameSql, socialClubName), connection))
                {
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Ban> GetAllTemporaryBans()
        {
            List<Ban> bans = new List<Ban>();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(SelectAllTemporaryBansSql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bans.Add(new Ban((BanCategory)reader.GetInt16(3), reader.GetString(0), reader.GetString(1), reader.GetInt64(4), reader.GetString(2)));
                        }
                    }
                }
            }

            return bans;
        }

        public Ban GetBanBySocialClubName(string socialClubName)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectBanBySocialClubNameSql, socialClubName), connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Ban((BanCategory)reader.GetInt16(2), socialClubName, reader.GetString(3), reader.GetInt64(0), reader.GetString(1));
                        }
                        else
                            return null;
                    }
                }
            }
        }

        public Ban GetBanByHwid(string hwid)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectBanByHwidSql, hwid), connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Ban((BanCategory)reader.GetInt16(2), hwid, reader.GetString(3), reader.GetInt64(0), reader.GetString(1));
                        }
                        else
                            return null;
                    }
                }
            }
        }

        public void TestRepo()
        {
            Log.Info("Testing BanRepo...");
            Log.Info("Delete existing: " + RemoveBanBySocialClubName("1234"));
            long currentTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            Log.Info("Create: " + CreateNewBan("1234", "1234", 0, currentTime + 100000, "1234"));
            Log.Info("Get temporary: ");
            GetAllTemporaryBans().ForEach((e) => Log.Info(e));
            Log.Info("Get: " + GetBanBySocialClubName("1234"));
            Log.Info("Get: " + GetBanByHwid("1234"));
            Log.Info("Delete: " + RemoveBanBySocialClubName("1234"));
            Log.Info("Done.");
        }
    }
}