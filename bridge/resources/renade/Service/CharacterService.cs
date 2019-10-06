using System.Linq;
using System.Collections.Generic;

namespace renade
{
    public class CharacterService
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private readonly PrimaryDataRepo CharacterPrimaryDataRepo;
        private readonly PassRepo PassRepo;
        private readonly AppearanceRepo AppearanceRepo;
        private readonly PhoneContactRepo PhoneContactRepo;

        public CharacterService(string connectionString)
        {
            CharacterPrimaryDataRepo = new PrimaryDataRepo(connectionString);
            PassRepo = new PassRepo(connectionString);
            AppearanceRepo = new AppearanceRepo(connectionString);
            PhoneContactRepo = new PhoneContactRepo(connectionString);
        }

        public void CreateCharacter(string playerSocialClubName, string firstName, string familyName,
            Gender gender, Mother mother, Father father, float similarity, float skinColor, float noseHeight, float noseWidth,
            float noseLength, float noseBridge, float noseTip, float noseBridgeTip, float browWidth, float browHeight,
            float cheekboneWidth, float cheekboneHeight, float cheeksWidth, float eyes, float lips, float jawWidth, float jawHeight,
            float chinLength, float chinPosition, float chinWidth, float chinShape, float neckWidth, Hair hair, Eyebrows eyebrows,
            Beard beard, EyeColor eyeColor, HairColor hairColor)
        {
            if (!CharacterPrimaryDataRepo.CreateNewCharacterPrimaryData(playerSocialClubName, firstName, familyName))
                throw new FailedToCreateCharacterPrimaryDataException(playerSocialClubName);
            int characterId = CharacterPrimaryDataRepo.GetCharacterPrimaryDataByPlayerSocialClubName(playerSocialClubName)
                .OrderByDescending(item => item.Id).First().Id;
            if (!PassRepo.CreatePass(characterId, PassType.Regular)) // TODO - implement different pass types
                throw new FailedToCreateCharacterPassException(characterId);
            if (!AppearanceRepo.CreateAppearance(characterId, gender, mother, father, similarity, skinColor, noseHeight, noseWidth, noseLength,
                noseBridge, noseTip, noseBridgeTip, browWidth, browHeight, cheekboneWidth, cheekboneHeight, cheeksWidth, eyes,
                lips, jawWidth, jawHeight, chinLength, chinPosition, chinWidth, chinShape, neckWidth, hair, eyebrows, beard, eyeColor, hairColor))
                throw new FailedToCreateCharacterAppearanceException(characterId);
            Log.Info("Character '{0} {1}' successfully created for player '{2}'", firstName, familyName, playerSocialClubName);
        }

        public List<Character> GetPlayerCharactersBySocialClubName(string playerSocialClubName)
        {
            List<Character> characters = new List<Character>();
            foreach (PrimaryData primaryData in CharacterPrimaryDataRepo.GetCharacterPrimaryDataByPlayerSocialClubName(playerSocialClubName))
            {
                int characterId = primaryData.Id;
                Pass pass = PassRepo.GetPassByCharacterId(characterId);
                if (pass == null)
                    throw new CharacterPassDoesNotExistException(characterId);
                Appearance appearance = AppearanceRepo.GetAppearanceByCharacterId(characterId);
                if (appearance == null)
                    throw new CharacterAppearanceDoesNotExistException(characterId);
                characters.Add(new Character(primaryData, pass, PhoneContactRepo.GetPhoneContactsByCharacterId(characterId), appearance));
            }
            return characters;
        }

        public void SaveCharacter()
        {
            // TODO 
        }
    }
}