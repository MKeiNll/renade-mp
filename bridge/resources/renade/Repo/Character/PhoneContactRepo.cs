using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace renade
{
    public class PhoneContactRepo
    {
        private const string InsertPhoneContactSql = "INSERT INTO character_phone_contact (character_id, phone_number) VALUES ({0}, {1});";
        private const string SelectPhoneContactsByCharacterIdSql = "SELECT phone_number FROM character_phone_contact WHERE character_id = {0};";
        private const string DeletePhoneContactByCharacterIdAndPhoneNumberSql = "DELETE FROM character_phone_contact WHERE character_id = {0} AND phone_number = {1};";

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
                    return command.ExecuteNonQuery() > 0;
            }
        }

        public List<PhoneContact> GetPhoneContactsByCharacterId(int characterId)
        {
            List<PhoneContact> phoneContacts = new List<PhoneContact>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectPhoneContactsByCharacterIdSql, characterId), connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                    while (reader.Read())
                        phoneContacts.Add(new PhoneContact(reader.GetInt32(0)));
                return phoneContacts;
            }
        }

        public bool DeletePhoneContact(int characterId, int phoneNumber)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(DeletePhoneContactByCharacterIdAndPhoneNumberSql, characterId, phoneNumber), connection))
                    return command.ExecuteNonQuery() > 0;
            }
        }

        public void TestRepo()
        {
            Log.Info("Testing AppearanceRepo...");
            Log.Info("Delete existing: " + DeletePhoneContact(1234, 1111) + " " + DeletePhoneContact(1234, 2222));
            Log.Info("Create 1: " + CreatePhoneContact(1234, 1111));
            Log.Info("Create 2: " + CreatePhoneContact(1234, 2222));
            Log.Info("Get:");
            GetPhoneContactsByCharacterId(1234).ForEach((e) => Log.Info(e));
            Log.Info("Delete: " + DeletePhoneContact(1234, 1111) + " " + DeletePhoneContact(1234, 2222));
            Log.Info("Done.");
        }
    }
}