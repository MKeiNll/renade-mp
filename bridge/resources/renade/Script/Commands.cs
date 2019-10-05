using System;
using System.Text.RegularExpressions;
using GTANetworkAPI;

namespace renade
{
    class Commands : Script
    {
        private const string DefaultErrorMessage = "~r~Something went wrong! ;(";
        private readonly Regex TimeRegex = new Regex(@"^(\d)+([smhdy])$", RegexOptions.IgnoreCase);
        private static readonly Principal Principal = Renade.Principal;
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private Client GetPlayerBySocialClubName(string socialClubName)
        {
            foreach (Client client in NAPI.Pools.GetAllPlayers())
            {
                if (client.SocialClubName == socialClubName)
                    return client;
            }
            return null;
        }

        #region chatCommands
        public void CMD_meChat(Client client, string text)
        {
            try
            {
                foreach (Client player in NAPI.Player.GetPlayersInRadiusOfPlayer(20, client))
                {
                    client.SendChatMessage($"~c~{player.Name} {text}");
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                client.SendNotification(DefaultErrorMessage);
            }
        }

        #endregion chatCommands

        #region adminCommands[1]

        [Command("a", GreedyArg = true)] // `/a Heya, admins!`
        public void CMD_aChat(Client player, string text)
        {
            // TODO � if admLVL >= 1 then display this message

            try
            {
                player.SendChatMessage($"~r~[{player.Name}]: {text}");
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("kick", GreedyArg = true)] // `/kick banany 1.12`
        public void CMD_kick(Client player, Client targetName, string reason)
        {
            try
            {
                NAPI.Player.KickPlayer(targetName, reason);
                NAPI.Chat.SendChatMessageToAll($"~r~{player.Name} kicked {targetName.Name}: {reason}");
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("cban", GreedyArg = true)] // `/cban banany`
        public void CMD_cBan(Client player, Client target)
        {
            try
            {
                player.SendChatMessage(Principal.IsPlayerBanned(target).ToString());
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        #endregion adminCommands[1]

        #region adminCommands[2]

        [Command("ban", GreedyArg = true)] // `/ban banany 1.10`
        public void CMD_ban(Client player, string targetSocialClubName, string reason, BanCategory category, string time)
        {
            try
            {
                Client target = GetPlayerBySocialClubName(targetSocialClubName);
                if (target != null)
                    Principal.IssueTemporaryBan(target, reason, category, ConvertTimeToEpoch(time));
                else
                    player.SendNotification("Target doesn't exist");
            }
            catch (BanReasonTooLongException)
            {
                player.SendNotification("Ban reason is too long!");
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("gethere", GreedyArg = true)] // `/getHere banany`
        public void CMD_getHere(Client player, Client targetName)
        {
            try
            {
                targetName.Position = new Vector3(player.Position.X, player.Position.Y, (player.Position.Z + 1));
                player.SendChatMessage($"{targetName.Name} teleported to {player.Name}");
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("goto", GreedyArg = true)] // `/goto banany`
        public void CMD_goTo(Client player, Client targetName)
        {
            try
            {
                player.Position = new Vector3(targetName.Position.X, targetName.Position.Y, (targetName.Position.Z + 1));
                player.SendChatMessage($"Teleported to {targetName.Name}");
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        #endregion adminCommands[2]

        #region adminCommands[3]

        [Command("freeze", GreedyArg = true)] // `/freeze Sweetburn`
        public void CMD_freeze(Client player, Client targetName)
        {
            try
            {
                targetName.Freeze(true);
                player.SendNotification($"{targetName.Name} freezed");
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("unfreeze", GreedyArg = true)] // `/unfreeze Sweetburn`
        public void CMD_unFreeze(Client player, Client targetName)
        {
            try
            {
                targetName.Freeze(false);
                player.SendNotification($"{targetName.Name} unfreezed");
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("getip", GreedyArg = true)] // `/getIP Sweetburn`
        public void CMD_getIP(Client player, Client targetName)
        {
            try
            {
                player.SendChatMessage($"{targetName.Name} IP is: {targetName.Address}");
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        #endregion adminCommands[3]

        #region adminCommands[4]

        [Command("iban", GreedyArg = true)] // `/iban`
        public void CMD_iBan(Client player, string targetSocialClubName, string reason, BanCategory category)
        {
            try
            {
                Client target = GetPlayerBySocialClubName(targetSocialClubName);
                if (target != null)
                    Principal.IssuePermanentBan(target, reason, category);
                else
                    player.SendNotification("Target doesn't exist");
            }
            catch (BanReasonTooLongException)
            {
                player.SendNotification("Ban reason is too long!");
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("unban", GreedyArg = true)] // `/unban banany`
        public void CMD_unban(Client player, string targetName)
        {
            try
            {
                Principal.RemovePlayerBanBySocialClubName(targetName);
                player.SendNotification("Ban removed");
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        #endregion adminCommands[4]

        #region adminCommands[5]

        #endregion adminCommands[5]

        #region adminCommands[6]

        #endregion adminCommands[6]

        #region adminCommands[7]

        #endregion adminCommands[7]

        #region adminCommands[8]

        #endregion adminCommands[8]

        [RemoteEvent("kickclient")]
        public void ClientEvent_Kick(Client player)
        {
            try
            {
                player.Kick();
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("armor", "Usage: /armor [target] [amount]")] // `/armor Sweetburn 50`
        public void CMD_armor(Client player, string targetSocialClubName, int amount = 100)
        {
            try
            {
                Client target = GetPlayerBySocialClubName(targetSocialClubName);
                if (target != null)
                {
                    target.Armor = amount;
                    target.SendChatMessage($"{target.Name} armor is: {amount}");
                }
                else
                    player.SendNotification("Target doesn't exist");
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("ataser", "Usage: /ataser [targetName]")] // `/ataser Sweetburn`
        public static void CMD_aTaser(Client player, Client targetName, int type)
        {
            try
            {
                player.TriggerEvent("aTaser", targetName, type);
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("fix", "Usage: /fix")] // `/fix`
        public void CMD_repairVehicle(Client player)
        {
            try
            {
                if (player.IsInVehicle == true)
                    NAPI.Vehicle.RepairVehicle(NAPI.Player.GetPlayerVehicle(player));
                else
                    player.SendChatMessage("~r~You are not in vehicle!");
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("gm", "Usage: /gm")] // `/gm`
        public void CMD_gm(Client player)
        {
            try
            {
                player.Invincible = !player.Invincible;
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("hp", "Usage: /hp [target] [amount]")] // `/hp Sweetburn 50`
        public void CMD_hp(Client player, string targetSocialClubName, int amount = 100)
        {
            try
            {
                if (amount > 100)
                    amount = 100;
                else if (amount < 0)
                    player.SendChatMessage("~r~HP couldn't be a lower then 0");
                else
                {
                    Client target = GetPlayerBySocialClubName(targetSocialClubName);
                    if (target != null)
                    {
                        target.Health = amount;
                        target.SendChatMessage($"{target.Name} health is: {amount}");
                    }
                    else
                        player.SendNotification("Target doesn't exist");
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("inv", "Usage: /inv", Alias = "invisible")] // `/inv`
        public void CMD_invisible(Client player)
        {
            // Hide / unhide player label

            try
            {
                if (player.Transparency == 255)
                    player.Transparency = 0;
                else
                    player.Transparency = 255;
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("pos")] // `pos` Only for debug goals
        public void CMD_pos(Client player)
        {
            try
            {
                player.SendChatMessage($"X: {player.Position.X},\n Y: {player.Position.Y}, \n Z: {player.Position.Z}");
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("re", "Usage: /re [target]", Alias = "recon,��")] // `/re Sweetburn` [TODO]
        public void CMD_recon(Client player, Client targetName)
        {
            try
            {
                // TODO - Somenthing like GTA:O
                player.TriggerEvent("spectatorEvent", targetName);
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("skin")]
        public void ChangeSkinCommand(Client player, PedHash model)
        {
            try
            {
                NAPI.Player.SetPlayerSkin(player, model);
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("time", "Usage: /time [hours] [minutes] [seconds]")] // `/time 23 15 00`
        public void CMD_setTime(Client player, int hours, int minutes, int seconds)
        {
            try
            {
                NAPI.World.SetTime(hours, minutes, seconds);
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("v", "Usage: /v [vehicle_name] [color1] [color2] [numberPlate] [plateColor]")] // `/v tampa 5 5 dev 2`
        public void CMD_CreateVehicle(Client player, VehicleHash vehicle_name, int color1, int color2, string numberPlate, int plate)
        {
            try
            {
                if (player.HasData("personalVehicle"))
                {
                    Vehicle previousVehicle = player.GetData("personalVehicle");
                    previousVehicle.Delete();
                }

                Vehicle veh = NAPI.Vehicle.CreateVehicle(vehicle_name, player.Position.Around(5), 0, color1, color2);

                veh.NumberPlate = $"{numberPlate}";
                NAPI.Vehicle.SetVehicleNumberPlateStyle(veh, plate);

                player.SetData("personalVehicle", veh);

            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("g", "Usage: /g [weaponHash] [ammo]", Alias = "weap,weapon")] // `/g Revolver 1000`
        public void CMD_giveWeapon(Client player, WeaponHash hash, int count)
        {
            try
            {
                NAPI.Player.GivePlayerWeapon(player, hash, count);
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("weather", "Usage: /weather [weather]")] // `/weather XMAS` [EXTRASUNNY, CLEAR, CLOUDS, SMOG, FOGGY, OVERCAST, RAIN, THUNDER, CLEARING, NEUTRAL, SNOW, BLIZZARD, SNOWLIGHT, XMAS]
        public void CMD_setWeather(Client player, Weather weather)
        {
            try
            {
                NAPI.World.SetWeather(weather);
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("whonear", "Usage: /whonear [target] [radius]")] // `/whonear Sweetburn 100`
        public void CMD_whoNear(Client client, Client targetName, float radius = 100)
        {
            try
            {
                foreach (Client player in NAPI.Player.GetPlayersInRadiusOfPlayer(radius, targetName))
                {
                    client.SendChatMessage($"{player.Name}");
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                client.SendNotification(DefaultErrorMessage);
            }
        }

        [Command("q")] // `/q` Not work for these moment [TODO]
        public static void CMD_disconnect(Client player)
        {
            try
            {
                player.TriggerEvent("quitcmd");
            }
            catch (Exception e)
            {
                Log.Error(e);
                player.SendNotification(DefaultErrorMessage);
            }
        }

        private long ConvertTimeToEpoch(String input)
        {
            Match match = TimeRegex.Match(input);
            if (match == Match.Empty)
                throw new FailedToConvertTimeToEpochException(input);

            string numeral = match.Groups[1].Value;
            string letter = match.Groups[2].Value;

            long unbanDate = 0;
            switch (letter.ToLower())
            {
                case "s":
                    unbanDate = int.Parse(numeral) * 1000;
                    break;
                case "m":
                    unbanDate = int.Parse(numeral) * 60000;
                    break;
                case "h":
                    unbanDate = int.Parse(numeral) * 3600000;
                    break;
                case "d":
                    unbanDate = int.Parse(numeral) * 86400000;
                    break;
                case "y":
                    unbanDate = int.Parse(numeral) * 31536000000;
                    break;
            }

            return unbanDate + (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }
}