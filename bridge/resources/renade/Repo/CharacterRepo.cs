using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace renade
{
    public class CharacterRepo
    {
        public const int MaxCharacterFirstNameLength = 16;
        public const int MaxCharacterFamilyNameLength = 16;

        public const int DuplicateKeyOnRandomValueMaxRetryCount = 10;

        private const string SelectCharacterByPlayerSocialClubNameSql = "SELECT character_id, first_name, family_name, character_level, dev_pass_id, " +
            "admin_pass_id, media_pass_id, regular_pass_id, gender, reg_date, pos_x, pos_y, pos_z FROM ingame_character WHERE player_social_club_name = '{0}';";
        private const string InsertCharacterSql = "INSERT INTO ingame_character (player_social_club_name, first_name, family_name, dev_pass_id, " +
            "admin_pass_id, media_pass_id, regular_pass_id, gender, reg_date, bank_id, phone_number, mother, father, eyeColor, hair, hairColor, faceFeatures) " +
            "VALUES ('{0}', '{1}', '{2}', {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, '{16}');";
        private const string SelectMaxDevPassIdSql = "SELECT MAX(dev_pass_id) FROM ingame_character;";
        private const string SelectMaxAdminPassId = "SELECT MAX(admin_pass_id) FROM ingame_character;";
        private const string SelectMaxMediaPassId = "SELECT MAX(media_pass_id) FROM ingame_character;";
        private const string SelectMaxRegularPassId = "SELECT MAX(regular_pass_id) FROM ingame_character;";
        private const string DeleteCharacterByIdSql = "DELETE FROM ingame_character WHERE character_id = '{0}';";

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private readonly string ConnectionString;
        private Random Random = new Random();

        public CharacterRepo(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public bool CreateNewCharacter(string playerSocialClubName, string firstName, string familyName, PassType passType, Gender gender, int mother, int father, int eyeColor, int hair, int hairColor, string faceFeatures)
        {
            return CreateNewCharacter(playerSocialClubName, firstName, familyName, passType, gender, mother, father, eyeColor, hair, hairColor, faceFeatures, 0);
        }

        private bool CreateNewCharacter(string playerSocialClubName, string firstName, string familyName, PassType passType, Gender gender, int mother, int father, int eyeColor, int hair, int hairColor, string faceFeatures, int repeatCount)
        {
            if (firstName.Length > MaxCharacterFirstNameLength)
                throw new CharacterFirstNameTooLongException(firstName);
            if (familyName.Length > MaxCharacterFamilyNameLength)
                throw new CharacterFamilyNameTooLongException(familyName);

            string commandSql = null;
            int passId = GetFreeCharacterPassId(passType);
            long regDate = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            int bankId = GenerateCharacterBankIdOrPhone();
            int phoneNumber = GenerateCharacterBankIdOrPhone();
            switch (passType)
            {
                case PassType.Developer:
                    commandSql = string.Format(InsertCharacterSql, playerSocialClubName, firstName, familyName, passId, "null", "null", "null", (int)gender, regDate, bankId, phoneNumber, mother, father, eyeColor, hair, hairColor, faceFeatures);
                    break;
                case PassType.Admin:
                    commandSql = string.Format(InsertCharacterSql, playerSocialClubName, firstName, familyName, "null", passId, "null", "null", (int)gender, regDate, bankId, phoneNumber, mother, father, eyeColor, hair, hairColor, faceFeatures);
                    break;
                case PassType.Media:
                    commandSql = string.Format(InsertCharacterSql, playerSocialClubName, firstName, familyName, "null", "null", passId, "null", (int)gender, regDate, bankId, phoneNumber, mother, father, eyeColor, hair, hairColor, faceFeatures);
                    break;
                case PassType.Regular:
                    commandSql = string.Format(InsertCharacterSql, playerSocialClubName, firstName, familyName, "null", "null", "null", passId, (int)gender, regDate, bankId, phoneNumber, mother, father, eyeColor, hair, hairColor, faceFeatures);
                    break;
            }
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(commandSql, connection))
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
                            return CreateNewCharacter(playerSocialClubName, firstName, familyName, passType, gender, mother, father, eyeColor, hair, hairColor, faceFeatures, repeatCount + 1);
                        }
                        throw;
                    }
                }
            }
        }

        public List<Character> GetCharactersByPlayerSocialClubName(string socialClubName)
        {
            List<Character> characters = new List<Character>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectCharacterByPlayerSocialClubNameSql, socialClubName), connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PassType passType;
                            int passId = 0;
                            int id = reader.GetInt32(0);

                            if (!reader.IsDBNull(4))
                                passType = PassType.Developer;
                            else if (!reader.IsDBNull(5))
                                passType = PassType.Admin;
                            else if (!reader.IsDBNull(6))
                                passType = PassType.Media;
                            else if (!reader.IsDBNull(7))
                                passType = PassType.Regular;
                            else
                                throw new CharacterPassIdDoesNotExistException(id);
                            switch (passType)
                            {
                                case PassType.Developer:
                                    passId = reader.GetInt16(4);
                                    break;
                                case PassType.Admin:
                                    passId = 10 + reader.GetInt16(5);
                                    break;
                                case PassType.Media:
                                    passId = 100 + reader.GetInt16(6);
                                    break;
                                case PassType.Regular:
                                    passId = 1000 + reader.GetInt16(7);
                                    break;
                            }

                            characters.Add(new Character(socialClubName, reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), passId, passType,
                                (Gender)reader.GetInt16(8), reader.GetInt64(9), reader.GetFloat(10), reader.GetFloat(11), reader.GetFloat(12)));
                        }
                        return characters;
                    }
                }
            }
        }

        private int GenerateCharacterBankIdOrPhone()
        {
            string resultString = "";
            for (int i = 0; i < 6; i++)
            {
                resultString += Random.Next(0, 9).ToString();
            }
            int result = 0;
            if (Int32.TryParse(resultString, out result))
                return result;
            else
                throw new FailedToGenerateCharacterBankIdOrPhoneException(resultString);
        }

        public int GetFreeCharacterPassId(PassType passType)
        {
            switch (passType)
            {
                case PassType.Developer:
                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {
                        connection.Open();
                        using (MySqlCommand command = new MySqlCommand(SelectMaxDevPassIdSql, connection))
                        {
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (reader.IsDBNull(0))
                                        return 1;
                                    else
                                        return reader.GetInt32(0) + 1;
                                }
                            }
                        }
                    }
                    break;
                case PassType.Admin:
                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {
                        connection.Open();
                        using (MySqlCommand command = new MySqlCommand(SelectMaxAdminPassId, connection))
                        {
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (reader.IsDBNull(0))
                                        return 1;
                                    else
                                        return reader.GetInt32(0) + 1;
                                }
                            }
                        }
                    }
                    break;
                case PassType.Media:
                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {
                        connection.Open();
                        using (MySqlCommand command = new MySqlCommand(SelectMaxMediaPassId, connection))
                        {
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (reader.IsDBNull(0))
                                        return 1;
                                    else
                                        return reader.GetInt32(0) + 1;
                                }
                            }
                        }
                    }
                    break;
                case PassType.Regular:
                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {
                        connection.Open();
                        using (MySqlCommand command = new MySqlCommand(SelectMaxRegularPassId, connection))
                        {
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (reader.IsDBNull(0))
                                        return 1;
                                    else
                                        return reader.GetInt32(0) + 1;
                                }
                            }
                        }
                    }
                    break;
            }
            return 0;
        }

        public bool DeleteCharacterById(int characterId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(DeleteCharacterByIdSql, characterId), connection))
                {
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public void TestRepo()
        {
            Log.Info("Testing CharacterRepo...");
            Log.Info("Delete existing: " + DeleteCharacterById(3));
            Log.Info("Free pass id: " + GetFreeCharacterPassId(PassType.Developer));
            Log.Info("Create: " + CreateNewCharacter("1234", "1234", "1234", PassType.Developer, Gender.Female, 1, 1, 1, 1, 1, "1234"));
            Log.Info("Get: ");
            GetCharactersByPlayerSocialClubName("1234").ForEach((e) => Log.Info(e));
            Log.Info("Free pass id: " + GetFreeCharacterPassId(PassType.Developer));
            Log.Info("Delete: " + DeleteCharacterById(3));
            Log.Info("Done.");
        }
    }
}