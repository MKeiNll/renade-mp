using System;
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
        private const string SelectMaxPassValueByTypeSql = "SELECT MAX(id) FROM character_pass WHERE pass_type = {0};";
        private const string DeletePassByCharacterIdSql = "DELETE FROM character_pass WHERE character_id = {0};";

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private readonly string ConnectionString;

        public PassRepo(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public bool CreatePass(int characterId, PassType passType)
        {
            int id = GetLargestPassValueByType(passType) + 1;
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

        public int GetLargestPassValueByType(PassType passType)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectMaxPassValueByTypeSql, passType), connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return reader.GetInt32(0);
                        else
                            return -1;
                    }
                }
            }
        }

        public bool DeletePassByCharacterId(int characterId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(DeletePassByCharacterIdSql, characterId), connection))
                {
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public void TestRepo()
        {
            Log.Info("Testing PassRepo...");
            Log.Info("Delete existing: " + DeletePassByCharacterId(12) + " " + DeletePassByCharacterId(123)
                + " " + DeletePassByCharacterId(1234) + " " + DeletePassByCharacterId(12345)
                + " " + DeletePassByCharacterId(123456) + " " + DeletePassByCharacterId(1234567)
                + " " + DeletePassByCharacterId(12345678) + " " + DeletePassByCharacterId(123456789));
            Log.Info("Create regular 1: " + CreatePass(12, PassType.Regular));
            Log.Info("Create regular 2: " + CreatePass(123, PassType.Regular));
            Log.Info("Create media 1: " + CreatePass(1234, PassType.Media));
            Log.Info("Create media 2: " + CreatePass(12345, PassType.Media));
            Log.Info("Create admin 1: " + CreatePass(123456, PassType.Admin));
            Log.Info("Create admin 2: " + CreatePass(1234567, PassType.Admin));
            Log.Info("Create dev 1: " + CreatePass(12345678, PassType.Developer));
            Log.Info("Create dev 2: " + CreatePass(123456789, PassType.Developer));
            Log.Info("Get max regular: " + GetLargestPassValueByType(PassType.Regular));
            Log.Info("Get max media: " + GetLargestPassValueByType(PassType.Media));
            Log.Info("Get max admin: " + GetLargestPassValueByType(PassType.Admin));
            Log.Info("Get max dev: " + GetLargestPassValueByType(PassType.Developer));
            Log.Info("Get regular 1: " + GetPassByCharacterId(12));
            Log.Info("Get regular 2: " + GetPassByCharacterId(123));
            Log.Info("Get media 1: " + GetPassByCharacterId(1234));
            Log.Info("Get media 2: " + GetPassByCharacterId(12345));
            Log.Info("Get admin 1: " + GetPassByCharacterId(123456));
            Log.Info("Get admin 2: " + GetPassByCharacterId(1234567));
            Log.Info("Get dev 1: " + GetPassByCharacterId(12345678));
            Log.Info("Get dev 2: " + GetPassByCharacterId(123456789));
            Log.Info("Delete: " + DeletePassByCharacterId(12) + " " + DeletePassByCharacterId(123)
                + " " + DeletePassByCharacterId(1234) + " " + DeletePassByCharacterId(12345)
                + " " + DeletePassByCharacterId(123456) + " " + DeletePassByCharacterId(1234567)
                + " " + DeletePassByCharacterId(12345678) + " " + DeletePassByCharacterId(123456789));
            Log.Info("Done.");
        }
    }
}