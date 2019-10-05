using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace renade
{
    public class CharacterPrimaryDataRepo
    {
        public const int MaxCharacterFirstNameLength = 16;
        public const int MaxCharacterFamilyNameLength = 16;

        public const int DuplicateKeyOnRandomValueMaxRetryCount = 10;

        private const string SelectCharacterPrimaryDataByPlayerSocialClubNameSql = "SELECT character_id, first_name, family_name, character_level, reg_date, " +
            "phone_number, pos_x, pos_y, pos_z FROM player_character_primary_data WHERE player_social_club_name = '{0}';";
        private const string InsertCharacterPrimaryDataSql = "INSERT INTO player_character_primary_data (player_social_club_name, first_name, family_name, " +
            "reg_date, phone_number, bank_id) VALUES ('{0}', '{1}', '{2}', {3}, {4}, {5});";
        private const string DeleteCharacterPrimaryDataByIdSql = "DELETE FROM player_character_primary_data WHERE character_id = '{0}';";

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private readonly string ConnectionString;
        private Random Random = new Random();

        public CharacterPrimaryDataRepo(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public bool CreateNewCharacterPrimaryData(string playerSocialClubName, string firstName, string familyName)
        {
            return CreateNewCharacterPrimaryData(playerSocialClubName, firstName, familyName, 0);
        }

        private bool CreateNewCharacterPrimaryData(string playerSocialClubName, string firstName, string familyName, int repeatCount)
        {
            if (firstName.Length > MaxCharacterFirstNameLength)
                throw new CharacterFirstNameTooLongException(firstName);
            if (familyName.Length > MaxCharacterFamilyNameLength)
                throw new CharacterFamilyNameTooLongException(familyName);

            long regDate = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            int phoneNumber = GenerateCharacterBankIdOrPhone();
            int bankId = GenerateCharacterBankIdOrPhone();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(InsertCharacterPrimaryDataSql, playerSocialClubName, firstName, familyName,
                    regDate, phoneNumber, bankId), connection))
                {
                    try
                    {
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch (MySqlException e)
                    {
                        if (e.Number == 1062 && repeatCount <= DuplicateKeyOnRandomValueMaxRetryCount)
                        {
                            Log.Warn("Duplicate entry exception suppressed, repeatCount: {0}", repeatCount);
                            return CreateNewCharacterPrimaryData(playerSocialClubName, firstName, familyName, repeatCount + 1);
                        }
                        throw;
                    }
                }
            }
        }

        public List<PrimaryData> GetCharacterPrimaryDataByPlayerSocialClubName(string socialClubName)
        {
            List<PrimaryData> primaryDataList = new List<PrimaryData>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectCharacterPrimaryDataByPlayerSocialClubNameSql, socialClubName), connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            primaryDataList.Add(new PrimaryData(socialClubName, reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3),
                                reader.GetInt64(4), reader.GetInt32(5), reader.GetFloat(6), reader.GetFloat(7), reader.GetFloat(8)));
                        return primaryDataList;
                    }
                }
            }
        }

        public bool DeleteCharacterPrimaryDataById(int characterId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(DeleteCharacterPrimaryDataByIdSql, characterId), connection))
                {
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        private int GenerateCharacterBankIdOrPhone()
        {
            string resultString = "";
            for (int i = 0; i < 6; i++)
                resultString += Random.Next(0, 9).ToString();
            int result = 0;
            if (Int32.TryParse(resultString, out result))
                return result;
            else
                throw new FailedToGenerateCharacterBankIdOrPhoneException(resultString);
        }

        public void TestRepo()
        {
            Log.Info("Testing CharacterPrimaryDataRepo...");
            Log.Info("Delete existing: " + DeleteCharacterPrimaryDataById(3));
            Log.Info("Create: " + CreateNewCharacterPrimaryData("1234", "1234", "1234"));
            Log.Info("Get: ");
            GetCharacterPrimaryDataByPlayerSocialClubName("1234").ForEach((e) => Log.Info(e));
            Log.Info("Delete: " + DeleteCharacterPrimaryDataById(3));
            Log.Info("Done.");
        }
    }
}