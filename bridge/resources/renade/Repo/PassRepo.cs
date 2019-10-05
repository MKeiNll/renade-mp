using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace renade
{
    public class PassRepo
    {
        public const int DeveloperPassMaxValue = 10;
        public const int AdminPassMaxValue = 100;
        public const int MediaPassMaxValue = 1000;

        private const string InsertPassSql = "INSERT INTO character_pass (character_id, pass_type, id) VALUES ({0}, {1}, {2});";
        private const string SelectPassByCharacterIdSql = "SELECT id, pass_type FROM character_pass WHERE character_id = {0};";

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private readonly string ConnectionString;

        public PassRepo(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public bool CreatePass(int characterId, PassType passType, int id)
        {
            if ((passType == PassType.Developer && id > DeveloperPassMaxValue) ||
                (passType == PassType.Admin && id > AdminPassMaxValue) ||
                (passType == PassType.Media && id > MediaPassMaxValue))
                throw new PassValueTooBigException(passType, id);

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(InsertPassSql, characterId, passType, id), connection))
                {
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public Pass GetPassByCharacterId(int characterId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectPassByCharacterIdSql, characterId), connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return new Pass(reader.GetInt32(0), (PassType)reader.GetInt16(1));
                        else
                            return null;
                    }
                }
            }
        }
    }
}