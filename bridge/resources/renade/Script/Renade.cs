using System;
using System.Threading;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using GTANetworkAPI;

namespace renade
{
    public class Renade : Script
    {
        private const string RenadeConfigLocation = "renade.json";
        private const string NLogConfigLocation = "NLog.config";
        private const string LogMainSeparator = "==================================================================";
        private const string LogTimerSeparator = "------------------------------------------------------------------";

        private const string ConnectionString = "Server=localhost; database=renade; UID={0}; password={1}";
        private const int UnauthorizedDimension = 100;
        private const int MainDimension = 0;

        private const int GlobalTimerInteval = 1;

        private const int MaxOnline = 1000;

        private List<Player> OnlinePlayers = new List<Player>();
        private List<string> UnauthorizedPlayers = new List<string>();

        private static Timer GlobalTimer;
        private static readonly NLog.Logger Log;

        public static readonly PlayerRepo PlayerRepo;
        public static readonly BanService BanService;
        public static readonly CharacterService CharacterService;

        static Renade()
        {
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(NLogConfigLocation);
            Log = NLog.LogManager.GetCurrentClassLogger();

            Log.Info(LogMainSeparator);
            Log.Info("Setting up RenadeMP...");

            Log.Info("Processing config...");
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(RenadeConfigLocation, System.Text.Encoding.UTF8));
            Log.Info("Config processed.");

            Log.Info("Setting up services...");
            string formattedConnectionString = string.Format(ConnectionString, config.DatabaseUser, config.DatabasePassword);
            Log.Info("Setting up player repo...");
            PlayerRepo = new PlayerRepo(formattedConnectionString);
            Log.Info("Player repo set up.");
            Log.Info("Setting up ban service...");
            BanService = new BanService(formattedConnectionString);
            Log.Info("Ban service set up.");
            Log.Info("Setting up character service...");
            CharacterService = new CharacterService(formattedConnectionString);
            Log.Info("Character service set up.");
            Log.Info("Services set up.");

            Log.Info("Setting up global timer...");
            GlobalTimer = new Timer((e) =>
            {
                try
                {
                    Log.Info(LogTimerSeparator);
                    Log.Info("Global timer triggered. (Current time: {0})", DateTimeOffset.Now);

                    NAPI.Task.Run(() =>
                    {
                        NAPI.World.SetTime(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                        Log.Info("Server time set.");
                    });
                    BanService.RemoveTemporaryBans();

                    Log.Info("Global timer executed.");
                    Log.Info(LogTimerSeparator);
                }
                catch (Exception exc)
                {
                    Log.Error(exc);
                }
            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(GlobalTimerInteval));
            Log.Info("Global timer set up.");

            Log.Info("RenadeMP set up.");
            Log.Info(LogMainSeparator);
        }

        private void LogConnection(string socialClubName)
        {
            int online = OnlinePlayers.Count;
            int unauthorized = UnauthorizedPlayers.Count;
            Log.Info("Online: {0} (Authorized: {1} / Unauthorized: {2}) - {3} has been connected.", online + unauthorized, online, unauthorized, socialClubName);
        }

        private void LogAuthorization(string socialClubName)
        {
            int online = OnlinePlayers.Count;
            int unauthorized = UnauthorizedPlayers.Count;
            Log.Info("Online: {0} (Authorized: {1} / Unauthorized: {2}) - {3} has been spawned.", online + unauthorized, online, unauthorized, socialClubName);
        }

        private void LogDisconnection(string socialClubName)
        {
            int online = OnlinePlayers.Count;
            int unauthorized = UnauthorizedPlayers.Count;
            Log.Info("Online: {0} (Authorized: {1} / Unauthorized: {2}) - {3} has been disconnected.", online + unauthorized, online, unauthorized, socialClubName);
        }

        [ServerEvent(Event.PlayerConnected)]
        public void Event_OnPlayerConnected(Client player)
        {
            try
            {
                string socialClubName = player.SocialClubName;
                LogConnection(socialClubName);
                UnauthorizedPlayers.Add(socialClubName);

                player.Dimension = UnauthorizedDimension;

                // TODO - handle max online limit
                if (OnlinePlayers.Any(p => p.SocialClubName == socialClubName))
                    BanService.KickPlayer(player, "Already in the game.");
                else
                {
                    if (BanService.IsPlayerBanned(player))
                        BanService.KickPlayer(player, "Ban.");
                    else
                        player.TriggerEvent("authBrowser", OnlinePlayers.Count, MaxOnline);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void Event_OnPlayerDisconnected(Client player, DisconnectionType type, string reason)
        {
            try
            {
                string socialClubName = player.SocialClubName;
                LogDisconnection(socialClubName);

                if (UnauthorizedPlayers.Contains(socialClubName))
                    UnauthorizedPlayers.Remove(socialClubName);
                else
                {
                    List<Player> disconnectedPlayer = OnlinePlayers.Where(p => p.SocialClubName == socialClubName).ToList();
                    int foundPlayers = disconnectedPlayer.Count;
                    if (foundPlayers != 1)
                        throw new FailedToRemovePlayerFromOnlineListException(socialClubName, foundPlayers);
                    OnlinePlayers.Remove(disconnectedPlayer[0]);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        [RemoteEvent("LoginPlayer")]
        public void LoginPlayer(Client player, string loginOrMail, string password)
        {
            try
            {
                string socialClubName = player.SocialClubName;

                Player playerAccount = PlayerRepo.GetPlayerByLogin(loginOrMail);
                if (playerAccount == null)
                    playerAccount = PlayerRepo.GetPlayerByMail(loginOrMail);

                if (playerAccount != null && PlayerRepo.IsPlayerPasswordValid(playerAccount, password) && playerAccount.SocialClubName == socialClubName)
                {
                    Character character = CharacterService.GetPlayerCharactersBySocialClubName(playerAccount.SocialClubName)[0];

                    player.TriggerEvent("loginOrRegisterPlayerSuccess", playerAccount.Login,
                        character.PrimaryData.FirstName, character.PrimaryData.FamilyName, character.PrimaryData.Level);
                    Log.Info("Player {0} successfully logged in.", socialClubName);
                }
                else
                    player.TriggerEvent("loginPlayerFailure");
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.TriggerEvent("loginPlayerFailure");
            }
        }

        [RemoteEvent("RegisterPlayer")]
        public void RegisterPlayer(Client player, string login, string mail, string password1, string password2)
        {
            try
            {
                string socialClubName = player.SocialClubName;

                if (password1 == password2)
                {
                    try
                    {
                        if (!PlayerRepo.CreateNewPlayer(login, socialClubName, mail, password1))
                            throw new FailedToCreatePlayerException(login, socialClubName, mail);
                    }
                    catch (PlayerLoginTooLongException) { } // TODO
                    catch (PlayerSocialClubNameTooLongException) { } // TODO
                    catch (PlayerMailTooLongException) { } // TODO
                    catch (PlayerPasswordTooLongException) { } // TODO
                    catch (PlayerPasswordTooShortException) { } // TODO
                    catch (PlayerSocialClubNameIsTakenException) { } // TODO
                    catch (PlayerLoginIsTakenException) { } // TODO
                    catch (PlayerMailIsTakenException) { } // TODO
                    catch (CharacterFirstNameTooLongException) { } // TODO
                    catch (CharacterFamilyNameTooLongException) { } // TODO

                    Character character = CharacterService.GetPlayerCharactersBySocialClubName(socialClubName)[0];
                    player.TriggerEvent("loginOrRegisterPlayerSuccess", login, character.PrimaryData.FirstName,
                        character.PrimaryData.FamilyName, character.PrimaryData.Level);
                    Log.Info("Player {0} successfully registered.", socialClubName);
                }
                else
                {
                    // TODO
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.TriggerEvent("registerPlayerFailure");
            }
        }

        [RemoteEvent("CreateCharacter")]
        public void CreateCharacter(Client player, Gender gender, Mother mother, Father father)
        {
            try
            {
                // TODO - add all parameters & print them
                // CharacterService.CreateCharacter(socialClubName, "Steve", "Jobsa", PassType.Regular, Gender.Male, 1, 1, 1, 1, 1, "asd");
                // catch (CharacterFirstNameTooLongException) { } // TODO
                // catch (CharacterFamilyNameTooLongException) { } // TODO
            }
            catch (Exception e)
            {
                Log.Error(e);
                // TODO - do something client-side
            }
        }

        [RemoteEvent("SpawnPlayer")]
        public void SpawnPlayer(Client player)
        {
            try
            {
                string socialClubName = player.SocialClubName;

                UnauthorizedPlayers.Remove(socialClubName);
                OnlinePlayers.Add(PlayerRepo.GetPlayerBySocialClubName(socialClubName));

                string playerIp = player.Address;
                if (!PlayerRepo.UpdatePlayerIpHistoryBySocialClubName(socialClubName, playerIp))
                    throw new FailedToUpdatePlayerIpHistoryException(playerIp);

                player.Dimension = MainDimension;
                player.TriggerEvent("spawnPlayerSuccess");
                LogAuthorization(socialClubName);
                Character character = CharacterService.GetPlayerCharactersBySocialClubName(socialClubName)[0];
                player.Position = new Vector3(character.PrimaryData.PosX, character.PrimaryData.PosY, character.PrimaryData.PosZ);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}