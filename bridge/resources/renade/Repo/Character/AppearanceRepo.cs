using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace renade
{
    public class AppearanceRepo
    {
        private const string InsertAppearanceSql = "INSERT INTO character_appearance (character_id, gender, mother, father, similarity, skin_color, nose_width, " +
            "nose_height, nose_length, nose_bridge, nose_tip, nose_bridge_shift, brow_height, brow_width, cheekbone_height, cheekbone_width, cheeks_width, " +
            "eyes, lips, jaw_width, jaw_height, chin_length, chin_position, chin_width, chin_shape, neck_width, hair, eyebrows, beard, eye_color, hair_color) " +
            "VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, " +
            "{24}, {25}, {26}, {27}, {28}, {29}, {30});";
        private const string SelectAppearanceByCharacterIdSql = "SELECT gender, mother, father, similarity, skin_color, nose_width, " +
            "nose_height, nose_length, nose_bridge, nose_tip, nose_bridge_shift, brow_height, brow_width, cheekbone_height, cheekbone_width, cheeks_width, " +
            "eyes, lips, jaw_width, jaw_height, chin_length, chin_position, chin_width, chin_shape, neck_width, hair, eyebrows, beard, eye_color, hair_color " +
            "FROM character_appearance WHERE character_id = {0};";
        private const string DeleteAppearanceByCharacterIdSql = "DELETE FROM character_appearance WHERE character_id = {0};";

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private readonly string ConnectionString;

        public AppearanceRepo(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public bool CreateAppearance(int characterId, Gender gender, Mother mother, Father father, float similarity, float skinColor, float noseHeight, float noseWidth,
            float noseLength, float noseBridge, float noseTip, float noseBridgeTip, float browWidth, float browHeight, float cheekboneWidth, float cheekboneHeight,
            float cheeksWidth, float eyes, float lips, float jawWidth, float jawHeight, float chinLength, float chinPosition, float chinWidth, float chinShape,
            float neckWidth, Hair hair, Eyebrows eyebrows, Beard beard, EyeColor eyeColor, HairColor hairColor)
        {
            // TODO - format checks

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(InsertAppearanceSql, characterId, (int)gender, (int)mother, (int)father, similarity,
                    skinColor, noseHeight, noseWidth, noseLength, noseBridge, noseTip, noseBridgeTip, browWidth, browHeight, cheekboneWidth, cheekboneHeight, cheeksWidth,
                    eyes, lips, jawWidth, jawHeight, chinLength, chinPosition, chinWidth, chinShape, neckWidth, (int)hair, (int)eyebrows, (int)beard, (int)eyeColor,
                    (int)hairColor), connection))
                {
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public Appearance GetAppearanceByCharacterId(int characterId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(SelectAppearanceByCharacterIdSql, characterId), connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return new Appearance((Gender)reader.GetInt16(0), (Mother)reader.GetInt16(1), (Father)reader.GetInt16(2), reader.GetFloat(3), reader.GetFloat(4),
                                reader.GetFloat(5), reader.GetFloat(6), reader.GetFloat(7), reader.GetFloat(8), reader.GetFloat(9), reader.GetFloat(10), reader.GetFloat(11),
                                reader.GetFloat(12), reader.GetFloat(13), reader.GetFloat(14), reader.GetFloat(15), reader.GetFloat(16), reader.GetFloat(17), reader.GetFloat(18),
                                reader.GetFloat(19), reader.GetFloat(20), reader.GetFloat(21), reader.GetFloat(22), reader.GetFloat(23), reader.GetFloat(24),
                                (Hair)reader.GetInt16(25), (Eyebrows)reader.GetInt16(26), (Beard)reader.GetInt16(27), (EyeColor)reader.GetInt16(28), (HairColor)reader.GetInt16(29));
                        else
                            return null;
                    }
                }
            }
        }

        public bool DeleteAppearanceByCharacterId(int characterId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(string.Format(DeleteAppearanceByCharacterIdSql, characterId), connection))
                {
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public void TestRepo()
        {
            Log.Info("Testing AppearanceRepo...");
            Log.Info("Delete existing: " + DeleteAppearanceByCharacterId(1234));
            Log.Info("Create: " + CreateAppearance(1234, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
            Log.Info("Get: " + GetAppearanceByCharacterId(1234));
            Log.Info("Delete: " + DeleteAppearanceByCharacterId(1234));
            Log.Info("Done.");
        }
    }
}