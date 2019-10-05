using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace renade
{
    public class PhoneContactRepo
    {
        private const string InsertPhoneContactSql = "INSERT INTO character_phone_contact (character_id, phone_number) VALUES ({0}, {1});";
        private const string SelectPhoneContactsByCharacterIdSql = "SELECT phone_number FROM character_phone_contact WHERE character_id = {0};";

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private readonly string ConnectionString;

        public PhoneContactRepo(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public bool CreatePhoneContact(int characterId, int phoneNumber)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(InsertPhoneContactSql, characterId, phoneNumber), connection))
                {
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<PhoneContact> GetPhoneContactsByCharacterId(int characterId)
        {
            List<PhoneContact> phoneContacts = new List<PhoneContact>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectPhoneContactsByCharacterIdSql, characterId), connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            phoneContacts.Add(new PhoneContact(reader.GetInt32(0)));
                        return phoneContacts;
                    }
                }
            }
        }
    }
}