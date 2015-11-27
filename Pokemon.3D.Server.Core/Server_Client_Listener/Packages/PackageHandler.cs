using System;
using System.Collections.Generic;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using System.Linq;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Packages
{
    /// <summary>
    /// Class containing Package Handler
    /// </summary>
    public class PackageHandler
    {
        /// <summary>
        /// Handle PackageData
        /// </summary>
        /// <param name="p">Package</param>
        public void Handle(Package p)
        {
            try
            {
                switch (p.PackageType)
                {
                    case (int)Package.PackageTypes.Unknown:
                        Core.Logger.Log("Unable to handle the package due to unknown type.", Logger.LogTypes.Debug, p.Client);
                        break;

                    case (int)Package.PackageTypes.GameData:
                        HandleGameData(p);
                        break;

                    case (int)Package.PackageTypes.PrivateMessage:
                        HandlePrivateMessage(p);
                        break;

                    case (int)Package.PackageTypes.ChatMessage:
                        HandleChatMessage(p);
                        break;

                    case (int)Package.PackageTypes.Ping:
                        HandlePing(p);
                        break;

                    case (int)Package.PackageTypes.GamestateMessage:
                        HandleGamestateMessage(p);
                        break;

                    case (int)Package.PackageTypes.TradeRequest:
                        HandleTradeRequest(p);
                        break;

                    case (int)Package.PackageTypes.TradeJoin:
                        HandleTradeJoin(p);
                        break;

                    case (int)Package.PackageTypes.TradeQuit:
                        HandleTradeQuit(p);
                        break;

                    case (int)Package.PackageTypes.TradeOffer:
                        HandleTradeOffer(p);
                        break;

                    case (int)Package.PackageTypes.TradeStart:
                        HandleTradeStart(p);
                        break;

                    case (int)Package.PackageTypes.BattleRequest:
                        HandleBattleRequest(p);
                        break;

                    case (int)Package.PackageTypes.BattleJoin:
                        HandleBattleJoin(p);
                        break;

                    case (int)Package.PackageTypes.BattleQuit:
                        HandleBattleQuit(p);
                        break;

                    case (int)Package.PackageTypes.BattleOffer:
                        HandleBattleOffer(p);
                        break;

                    case (int)Package.PackageTypes.BattleStart:
                        HandleBattleStart(p);
                        break;

                    case (int)Package.PackageTypes.BattleClientData:
                        HandleBattleClientData(p);
                        break;

                    case (int)Package.PackageTypes.BattleHostData:
                        HandleBattleHostData(p);
                        break;

                    case (int)Package.PackageTypes.BattlePokemonData:
                        HandleBattlePokemonData(p);
                        break;

                    case (int)Package.PackageTypes.ServerDataRequest:
                        HandleServerDataRequest(p);
                        break;
                    default:
                        Core.Logger.Log("Unable to handle the package due to unknown type.", Logger.LogTypes.Debug, p.Client);
                        break;
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }

        #region Pokemon 3D Data
        private void HandleGameData(Package p)
        {
            if (Core.Pokemon3DPlayer.HasPlayer(p.Client))
            {
                Core.Pokemon3DPlayer.GetPlayer(p.Client).Update(p, true);
            }
            else
            {
                // New Player - Pending to join.
                Player Player = new Player(p);

                // Server Space Limit
                if (Core.Pokemon3DPlayer.Count >= Core.Setting.MaxPlayers)
                {
                    Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.Kicked, Core.Setting.Token("SERVER_FULL"), p.Client));
                    Core.Logger.Log(Player.isGameJoltPlayer ?
                        Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_FULL")) :
                        Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_FULL")), Logger.LogTypes.Info, p.Client);
                    return;
                }

                // Offline mode?
                if (!Core.Setting.OfflineMode && !Player.isGameJoltPlayer && string.IsNullOrWhiteSpace(p.DataItems[2]))
                {
                    Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.Kicked, Core.Setting.Token("SERVER_OFFLINEMODE"), p.Client));
                    Core.Logger.Log(Player.isGameJoltPlayer ?
                        Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_OFFLINEMODE")) :
                        Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_OFFLINEMODE")), Logger.LogTypes.Info, p.Client);
                    return;
                }

                // GameMode
                bool IsGameModeMatched = false;
                for (int i = 0; i < Core.Setting.GameMode.Count; i++)
                {
                    if (string.Equals(Core.Setting.GameMode[i].Trim(), Player.GameMode, StringComparison.OrdinalIgnoreCase) || string.Equals(Core.Setting.GameMode[i].Trim(), p.DataItems[0], StringComparison.OrdinalIgnoreCase))
                    {
                        IsGameModeMatched = true;
                        break;
                    }
                    else
                    {
                        IsGameModeMatched = false;
                    }
                }

                if (!IsGameModeMatched)
                {
                    string GameModeAllowed = null;
                    for (int i = 0; i < Core.Setting.GameMode.Count; i++)
                    {
                        GameModeAllowed += Core.Setting.GameMode[i].Trim() + ", ";
                    }
                    GameModeAllowed = GameModeAllowed.Remove(GameModeAllowed.LastIndexOf(","));

                    Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.Kicked, Core.Setting.Token("SERVER_WRONGGAMEMODE", GameModeAllowed), p.Client));
                    Core.Logger.Log(Player.isGameJoltPlayer ?
                        Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_WRONGGAMEMODE", GameModeAllowed)) :
                        Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_WRONGGAMEMODE", GameModeAllowed)), Logger.LogTypes.Info, p.Client);
                    return;
                }

                // BlackList
                if (Player.IsBlackListed())
                {
                    Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.Kicked, Core.Setting.Token("SERVER_BLACKLISTED", Player.GetBlackList().Reason, Player.GetBlackList().RemainingTime), p.Client));
                    Core.Logger.Log(Player.isGameJoltPlayer ?
                        Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_BLACKLISTED", Player.GetBlackList().Reason, Player.GetBlackList().RemainingTime)) :
                        Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_BLACKLISTED", Player.GetBlackList().Reason, Player.GetBlackList().RemainingTime)), Logger.LogTypes.Info, p.Client);
                    return;
                }

                // IP BlackList
                if (Player.IsIPBlackListed())
                {
                    Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.Kicked, Core.Setting.Token("SERVER_IPBLACKLISTED", Player.GetIPBlackList().Reason, Player.GetIPBlackList().RemainingTime), p.Client));
                    Core.Logger.Log(Player.isGameJoltPlayer ?
                        Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_IPBLACKLISTED", Player.GetIPBlackList().Reason, Player.GetIPBlackList().RemainingTime)) :
                        Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_IPBLACKLISTED", Player.GetIPBlackList().Reason, Player.GetIPBlackList().RemainingTime)), Logger.LogTypes.Info, p.Client);
                    return;
                }

                // WhiteList
                if (Core.Setting.WhiteList && !Player.IsWhiteListed())
                {
                    Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.Kicked, Core.Setting.Token("SERVER_DISALLOW"), p.Client));
                    Core.Logger.Log(Player.isGameJoltPlayer ?
                        Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_DISALLOW")) :
                        Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_DISALLOW")), Logger.LogTypes.Info, p.Client);
                    return;
                }

                // A Clone GHOST - kidding
                for (int i = 0; i < Core.Pokemon3DPlayer.Count; i++)
                {
                    if (Player.isGameJoltPlayer)
                    {
                        if (Player.GameJoltID == Core.Pokemon3DPlayer[i].GameJoltID)
                        {
                            Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.Kicked, Core.Setting.Token("SERVER_CLONE"), p.Client));
                            Core.Logger.Log(Player.isGameJoltPlayer ?
                                Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_CLONE")) :
                                Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_CLONE")), Logger.LogTypes.Info, p.Client);
                            return;
                        }
                    }
                    else
                    {
                        if (string.Equals(Player.Name, Core.Pokemon3DPlayer[i].Name, StringComparison.Ordinal) && Core.Pokemon3DPlayer[i].GameJoltID == -1)
                        {
                            Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.Kicked, Core.Setting.Token("SERVER_CLONE"), p.Client));
                            Core.Logger.Log(Player.isGameJoltPlayer ?
                                Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_CLONE")) :
                                Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to join the server with the following reason: " + Core.Setting.Token("SERVER_CLONE")), Logger.LogTypes.Info, p.Client);
                            return;
                        }
                    }
                }

                // Else Let it roll :)
                Core.Pokemon3DPlayer.Add(p);
            }
        }

        private void HandlePrivateMessage(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Player PMPlayer = Core.Pokemon3DPlayer.GetPlayer(p.DataItems[0]);

            // Check if external player exist.
            if (!Core.Pokemon3DPlayer.HasPlayer(p.DataItems[0]))
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, PMPlayer.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", PMPlayer.Name, PMPlayer.GameJoltID.ToString(), "does not exist.") :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", PMPlayer.Name, "does not exist."), p.Client));
                Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to PM with the following reason: " + Core.Setting.Token("SERVER_GAMEJOLT", PMPlayer.Name, PMPlayer.GameJoltID.ToString(), "does not exist.")) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to PM with the following reason: " + Core.Setting.Token("SERVER_NOGAMEJOLT", PMPlayer.Name, "does not exist.")), Logger.LogTypes.PM, p.Client);
                return;
            }

            // Check if you are muted Globally
            if (Player.IsMuteListed())
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_MUTED", Player.GetMuteList().Reason, Player.GetMuteList().RemainingTime), p.Client));
                Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to PM with the following reason: " + Core.Setting.Token("SERVER_MUTED", Player.GetMuteList().Reason, Player.GetMuteList().RemainingTime)) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to PM with the following reason: " + Core.Setting.Token("SERVER_MUTED", Player.GetMuteList().Reason, Player.GetMuteList().RemainingTime)), Logger.LogTypes.PM, p.Client);
                return;
            }

            // Check if you are muted by the player.
            if (Player.IsMuteListed(PMPlayer))
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_MUTEDTEMP", Player.GetMuteList(PMPlayer).Reason, Player.GetMuteList(PMPlayer).RemainingTime), p.Client));
                Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to PM with the following reason: " + Core.Setting.Token("SERVER_MUTEDTEMP", Player.GetMuteList(PMPlayer).Reason, Player.GetMuteList(PMPlayer).RemainingTime)) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to PM with the following reason: " + Core.Setting.Token("SERVER_MUTEDTEMP", Player.GetMuteList(PMPlayer).Reason, Player.GetMuteList(PMPlayer).RemainingTime)), Logger.LogTypes.PM, p.Client);
                return;
            }

            // Else Let send :)
            // Before send, check if you swear.
            if (p.DataItems[1].HaveSweared())
            {
                Player.AddInfractionCount(1);
                Player.AddMuteList(3600, "You have swear too much today.");
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.SwearInfractionCap < 1 ?
                    Core.Setting.Token("SERVER_SWEAR", p.DataItems[1].SwearWord()) :
                    Core.Setting.Token("SERVER_SWEARWARNING", p.DataItems[1].SwearWord(), Player.GetSwearInfractionList().Points.ToString(), Core.Setting.SwearInfractionCap.ToString()), p.Client));
                Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "triggered swear infraction with the following reason: " + Core.Setting.Token("SERVER_SWEAR", p.DataItems[1].SwearWord())) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "triggered swear infraction with the following reason: " + Core.Setting.Token("SERVER_SWEAR", p.DataItems[1].SwearWord())), Logger.LogTypes.PM, p.Client);
            }

            // Here we go!
            // Let's do this.
            if (!string.IsNullOrWhiteSpace(p.DataItems[1]))
            {

                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.PrivateMessage, Player.ID, p.DataItems[1], PMPlayer.Network.Client));
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.PrivateMessage, Player.ID, p.DataItems, p.Client));

                Core.Logger.Log(Player.isGameJoltPlayer ?
                        Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "have sent a private message to " + p.DataItems[0]) :
                        Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have sent a private message to " + p.DataItems[0]), Logger.LogTypes.PM, p.Client);
            }
        }

        private void HandleChatMessage(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);

            if (Core.Setting.AllowChatInServer)
            {
                // Check if you are muted Globally
                if (Player.IsMuteListed())
                {
                    Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_MUTED", Player.GetMuteList().Reason, Player.GetMuteList().RemainingTime), p.Client));
                    Core.Logger.Log(Player.isGameJoltPlayer ?
                        Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to chat with the following reason: " + Core.Setting.Token("SERVER_MUTED", Player.GetMuteList().Reason, Player.GetMuteList().RemainingTime)) :
                        Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to chat with the following reason: " + Core.Setting.Token("SERVER_MUTED", Player.GetMuteList().Reason, Player.GetMuteList().RemainingTime)), Logger.LogTypes.Chat, p.Client);
                    return;
                }

                // Spam?
                if (Player.CC_LastChatTime != null && Player.CC_LastChatMessage == p.DataItems[0])
                {
                    if ((DateTime.Now - Player.CC_LastChatTime).TotalSeconds < Core.Setting.SpamResetDuration)
                    {
                        Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_SPAM"), p.Client));
                        Core.Logger.Log(Player.isGameJoltPlayer ?
                            Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to chat with the following reason: " + Core.Setting.Token("SERVER_SPAM")) :
                            Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to chat with the following reason: " + Core.Setting.Token("SERVER_SPAM")), Logger.LogTypes.Chat, p.Client);
                        return;
                    }
                }

                // Command?
                if (p.DataItems[0].StartsWith("/"))
                {
                    HandleChatCommand(p);
                    return;
                }

                // Before send, check if you swear.
                if (p.DataItems[0].HaveSweared())
                {
                    Player.AddInfractionCount(1);
                    Player.AddMuteList(3600, "You have swear too much today.");
                    Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.SwearInfractionCap < 1 ?
                        Core.Setting.Token("SERVER_SWEAR", p.DataItems[0].SwearWord()) :
                        Core.Setting.Token("SERVER_SWEARWARNING", p.DataItems[0].SwearWord(), Player.GetSwearInfractionList().Points.ToString(), Core.Setting.SwearInfractionCap.ToString()), p.Client));
                    Core.Logger.Log(Player.isGameJoltPlayer ?
                        Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "triggered swear infraction with the following reason: " + Core.Setting.Token("SERVER_SWEAR", p.DataItems[0].SwearWord())) :
                        Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "triggered swear infraction with the following reason: " + Core.Setting.Token("SERVER_SWEAR", p.DataItems[0].SwearWord())), Logger.LogTypes.Chat, p.Client);
                }

                // Let's do this.
                if (!string.IsNullOrWhiteSpace(p.DataItems[0]))
                {
                    for (int i = 0; i < Core.Pokemon3DPlayer.Count; i++)
                    {
                        if (!Player.IsMuteListed(Core.Pokemon3DPlayer[i]) && (Player.CC_CurrentChatChannel == Core.Pokemon3DPlayer[i].CC_CurrentChatChannel || Core.Pokemon3DPlayer[i].CC_CurrentChatChannel == Player.ChatChannelType.Default.ToString() || !Core.Setting.AllowChatChannels))
                        {
                            Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Player.ID, p.DataItems[0], Core.Pokemon3DPlayer[i].Network.Client));
                        }
                    }

                    Core.Logger.Log(Player.isGameJoltPlayer ?
                            Core.Setting.Token("SERVER_CHATGAMEJOLT", Player.Name, Player.GameJoltID.ToString(), p.DataItems[0]) :
                            Core.Setting.Token("SERVER_CHATNOGAMEJOLT", Player.Name, p.DataItems[0]), Logger.LogTypes.Chat, p.Client);

                    Player.CC_LastChatMessage = p.DataItems[0];
                    Player.CC_LastChatTime = DateTime.Now;
                }
            }
            else
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_NOCHAT"), p.Client));
            }
        }

        private void HandlePing(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Player.Network.LastValidPing = DateTime.Now;
        }

        private void HandleGamestateMessage(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Core.Pokemon3DPlayer.SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, Player.isGameJoltPlayer ?
                "The player " + Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), p.DataItems[0]) :
                "The player " + Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, p.DataItems[0]), null));
            Core.Logger.Log(Player.isGameJoltPlayer ?
                "The player " + Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), p.DataItems[0]) :
                "The player " + Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, p.DataItems[0]), Logger.LogTypes.Server, p.Client);
        }

        private void HandleTradeRequest(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Player TradePlayer = Core.Pokemon3DPlayer.GetPlayer(p.DataItems[0].ToInt());

            string TradePlayerName = TradePlayer.isGameJoltPlayer ? Core.Setting.Token("SERVER_GAMEJOLT", TradePlayer.Name, TradePlayer.GameJoltID.ToString(), "") : Core.Setting.Token("SERVER_NOGAMEJOLT", TradePlayer.Name, "");

            if (!Core.Setting.AllowTrade)
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_TRADEDISALLOW"), p.Client));
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.TradeQuit, Player.ID, "", p.Client));

                Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to trade with the following reason: " + Core.Setting.Token("SERVER_TRADEDISALLOW")) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to trade with the following reason: " + Core.Setting.Token("SERVER_TRADEDISALLOW")), Logger.LogTypes.Trade, p.Client);
                return;
            }

            // Server Restart Timer.
            if (Core.Setting.AutoRestartTime >= 10 && (Core.Setting.StartTime.AddSeconds(Core.Setting.AutoRestartTime) - DateTime.Now).TotalSeconds <= 300)
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_TRADEPVPFAIL", Core.Pokemon3DListener.TimeLeft()), p.Client));
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.TradeQuit, Player.ID, "", p.Client));

                Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to trade with the following reason: " + Core.Setting.Token("SERVER_TRADEPVPFAIL", Core.Pokemon3DListener.TimeLeft())) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to trade with the following reason: " + Core.Setting.Token("SERVER_TRADEPVPFAIL", Core.Pokemon3DListener.TimeLeft())), Logger.LogTypes.Trade, p.Client);
                return;
            }

            // Check if you are blocked.
            if (Player.IsMuteListed(TradePlayer))
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_MUTEDTEMP", Player.GetMuteList(TradePlayer).Reason, Player.GetMuteList(TradePlayer).RemainingTime), p.Client));
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.TradeQuit, Player.ID, "", p.Client));

                Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to trade with the following reason: " + Core.Setting.Token("SERVER_MUTEDTEMP", Player.GetMuteList(TradePlayer).Reason, Player.GetMuteList(TradePlayer).RemainingTime)) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to trade with the following reason: " + Core.Setting.Token("SERVER_MUTEDTEMP", Player.GetMuteList(TradePlayer).Reason, Player.GetMuteList(TradePlayer).RemainingTime)), Logger.LogTypes.Trade, p.Client);
                return;
            }

            Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.TradeRequest, Player.ID, "", TradePlayer.Network.Client));
            Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "have sent a trade request to " + TradePlayerName) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have sent a trade request to " + TradePlayerName), Logger.LogTypes.Trade, p.Client);
        }

        private void HandleTradeJoin(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Player TradePlayer = Core.Pokemon3DPlayer.GetPlayer(p.DataItems[0].ToInt());

            string TradePlayerName = TradePlayer.isGameJoltPlayer ? Core.Setting.Token("SERVER_GAMEJOLT", TradePlayer.Name, TradePlayer.GameJoltID.ToString(), "") : Core.Setting.Token("SERVER_NOGAMEJOLT", TradePlayer.Name, "");

            if (!Core.Setting.AllowTrade)
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_TRADEDISALLOW"), p.Client));
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.TradeQuit, Player.ID, "", p.Client));

                Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to trade with the following reason: " + Core.Setting.Token("SERVER_TRADEDISALLOW")) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to trade with the following reason: " + Core.Setting.Token("SERVER_TRADEDISALLOW")), Logger.LogTypes.Trade, p.Client);
                return;
            }

            // Server Restart Timer.
            if (Core.Setting.AutoRestartTime >= 10 && (Core.Setting.StartTime.AddSeconds(Core.Setting.AutoRestartTime) - DateTime.Now).TotalSeconds <= 300)
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_TRADEPVPFAIL", Core.Pokemon3DListener.TimeLeft()), p.Client));
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.TradeQuit, Player.ID, "", p.Client));

                Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to trade with the following reason: " + Core.Setting.Token("SERVER_TRADEPVPFAIL", Core.Pokemon3DListener.TimeLeft())) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to trade with the following reason: " + Core.Setting.Token("SERVER_TRADEPVPFAIL", Core.Pokemon3DListener.TimeLeft())), Logger.LogTypes.Trade, p.Client);
                return;
            }

            Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.TradeJoin, Player.ID, "", TradePlayer.Network.Client));
            Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "have joined the trade request from " + TradePlayerName) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have joined the trade request from " + TradePlayerName), Logger.LogTypes.Trade, p.Client);
        }

        private void HandleTradeQuit(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Player TradePlayer = Core.Pokemon3DPlayer.GetPlayer(p.DataItems[0].ToInt());

            string TradePlayerName = TradePlayer.isGameJoltPlayer ? Core.Setting.Token("SERVER_GAMEJOLT", TradePlayer.Name, TradePlayer.GameJoltID.ToString(), "") : Core.Setting.Token("SERVER_NOGAMEJOLT", TradePlayer.Name, "");

            Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.TradeQuit, Player.ID, "", TradePlayer.Network.Client));
            Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "have rejected the trade request from " + TradePlayerName) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have rejected the trade request from " + TradePlayerName), Logger.LogTypes.Trade, p.Client);
        }

        private void HandleTradeOffer(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Player TradePlayer = Core.Pokemon3DPlayer.GetPlayer(p.DataItems[0].ToInt());

            Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.TradeOffer, Player.ID, p.DataItems[1], TradePlayer.Network.Client));
        }

        private void HandleTradeStart(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Player TradePlayer = Core.Pokemon3DPlayer.GetPlayer(p.DataItems[0].ToInt());

            string TradePlayerName = TradePlayer.isGameJoltPlayer ? Core.Setting.Token("SERVER_GAMEJOLT", TradePlayer.Name, TradePlayer.GameJoltID.ToString(), "") : Core.Setting.Token("SERVER_NOGAMEJOLT", TradePlayer.Name, "");

            Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.TradeStart, Player.ID, "", TradePlayer.Network.Client));
            Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "have accept the trade from " + TradePlayerName) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have accept the trade from " + TradePlayerName), Logger.LogTypes.Trade, p.Client);
        }

        private void HandleBattleRequest(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Player PvPPlayer = Core.Pokemon3DPlayer.GetPlayer(p.DataItems[0].ToInt());

            string PVPPlayerName = PvPPlayer.isGameJoltPlayer ? Core.Setting.Token("SERVER_GAMEJOLT", PvPPlayer.Name, PvPPlayer.GameJoltID.ToString(), "") : Core.Setting.Token("SERVER_NOGAMEJOLT", PvPPlayer.Name, "");

            if (!Core.Setting.AllowPvP)
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_PVPDISALLOW"), p.Client));
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleQuit, Player.ID, "", p.Client));

                Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to battle with the following reason: " + Core.Setting.Token("SERVER_PVPDISALLOW")) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to battle with the following reason: " + Core.Setting.Token("SERVER_PVPDISALLOW")), Logger.LogTypes.PvP, p.Client);
                return;
            }

            // Server Restart Timer.
            if (Core.Setting.AutoRestartTime >= 10 && (Core.Setting.StartTime.AddSeconds(Core.Setting.AutoRestartTime) - DateTime.Now).TotalSeconds <= 300)
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_TRADEPVPFAIL", Core.Pokemon3DListener.TimeLeft()), p.Client));
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleQuit, Player.ID, "", p.Client));

                Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to trade with the following reason: " + Core.Setting.Token("SERVER_TRADEPVPFAIL", Core.Pokemon3DListener.TimeLeft())) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to trade with the following reason: " + Core.Setting.Token("SERVER_TRADEPVPFAIL", Core.Pokemon3DListener.TimeLeft())), Logger.LogTypes.PvP, p.Client);
                return;
            }

            // Check if you are blocked.
            if (Player.IsMuteListed(PvPPlayer))
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_MUTEDTEMP", Player.GetMuteList(PvPPlayer).Reason, Player.GetMuteList(PvPPlayer).RemainingTime), p.Client));
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleQuit, Player.ID, "", p.Client));

                Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to battle with the following reason: " + Core.Setting.Token("SERVER_MUTEDTEMP", Player.GetMuteList(PvPPlayer).Reason, Player.GetMuteList(PvPPlayer).RemainingTime)) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to battle with the following reason: " + Core.Setting.Token("SERVER_MUTEDTEMP", Player.GetMuteList(PvPPlayer).Reason, Player.GetMuteList(PvPPlayer).RemainingTime)), Logger.LogTypes.PvP, p.Client);
                return;
            }

            Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleRequest, Player.ID, "", PvPPlayer.Network.Client));
            Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "have sent a battle request to " + PVPPlayerName) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have sent a battle request to " + PVPPlayerName), Logger.LogTypes.PvP, p.Client);
        }

        private void HandleBattleJoin(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Player PvPPlayer = Core.Pokemon3DPlayer.GetPlayer(p.DataItems[0].ToInt());

            string PVPPlayerName = PvPPlayer.isGameJoltPlayer ? Core.Setting.Token("SERVER_GAMEJOLT", PvPPlayer.Name, PvPPlayer.GameJoltID.ToString(), "") : Core.Setting.Token("SERVER_NOGAMEJOLT", PvPPlayer.Name, "");

            if (!Core.Setting.AllowPvP)
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_PVPDISALLOW"), p.Client));
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleQuit, Player.ID, "", p.Client));

                Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to battle with the following reason: " + Core.Setting.Token("SERVER_PVPDISALLOW")) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to battle with the following reason: " + Core.Setting.Token("SERVER_PVPDISALLOW")), Logger.LogTypes.PvP, p.Client);
                return;
            }

            // Server Restart Timer.
            if (Core.Setting.AutoRestartTime >= 10 && (Core.Setting.StartTime.AddSeconds(Core.Setting.AutoRestartTime) - DateTime.Now).TotalSeconds <= 300)
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_TRADEPVPFAIL", Core.Pokemon3DListener.TimeLeft()), p.Client));
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleQuit, Player.ID, "", p.Client));

                Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to battle with the following reason: " + Core.Setting.Token("SERVER_TRADEPVPFAIL", Core.Pokemon3DListener.TimeLeft())) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to battle with the following reason: " + Core.Setting.Token("SERVER_TRADEPVPFAIL", Core.Pokemon3DListener.TimeLeft())), Logger.LogTypes.PvP, p.Client);
                return;
            }

            Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleJoin, Player.ID, "", PvPPlayer.Network.Client));
            Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "have joined the battle request from " + PVPPlayerName) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have joined the battle request from " + PVPPlayerName), Logger.LogTypes.PvP, p.Client);

            // Status Update
            Player.PvP_Status = Player.PvPTypes.Lobby;
            Player.PvP_OpponentID = PvPPlayer.ID;
            Player.PvP_Host = false;
            Player.PvP_Validatated = false;

            PvPPlayer.PvP_Status = Player.PvPTypes.Lobby;
            PvPPlayer.PvP_OpponentID = Player.ID;
            PvPPlayer.PvP_Host = true;
            PvPPlayer.PvP_Validatated = false;
        }

        private void HandleBattleQuit(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Player PVPPlayer = Core.Pokemon3DPlayer.GetPlayer(p.DataItems[0].ToInt());

            string PVPPlayerName = PVPPlayer.isGameJoltPlayer ? Core.Setting.Token("SERVER_GAMEJOLT", PVPPlayer.Name, PVPPlayer.GameJoltID.ToString(), "") : Core.Setting.Token("SERVER_NOGAMEJOLT", PVPPlayer.Name, "");

            Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleQuit, Player.ID, "", PVPPlayer.Network.Client));
            Core.Logger.Log(Player.isGameJoltPlayer ?
                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "have rejected the battle request from " + PVPPlayerName) :
                    Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have rejected the battle request from " + PVPPlayerName), Logger.LogTypes.PvP, p.Client);

            Player.PvP_Status = Player.PvPTypes.Nothing;
            PVPPlayer.PvP_Status = Player.PvPTypes.Nothing;
        }

        private void HandleBattleOffer(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Player PVPPlayer = Core.Pokemon3DPlayer.GetPlayer(p.DataItems[0].ToInt());

            Player.PvP_Pokemon = p.DataItems[1].Split('|').ToList();

            string ValidationResult = Player.DoPvPValidation();
            Player.PvP_Validatated = true;

            if (ValidationResult == null)
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleOffer, Player.ID, p.DataItems[1], PVPPlayer.Network.Client));
            }
            else if (ValidationResult.Contains("You may use this team"))
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, ValidationResult, Player.Network.Client));
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleOffer, Player.ID, p.DataItems[1], PVPPlayer.Network.Client));
            }
            else
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_PVPVALIDATION", ValidationResult), Player.Network.Client));
            }
        }

        private void HandleBattleStart(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Player PVPPlayer = Core.Pokemon3DPlayer.GetPlayer(p.DataItems[0].ToInt());

            string PVPPlayerName = PVPPlayer.isGameJoltPlayer ? Core.Setting.Token("SERVER_GAMEJOLT", PVPPlayer.Name, PVPPlayer.GameJoltID.ToString(), "") : Core.Setting.Token("SERVER_NOGAMEJOLT", PVPPlayer.Name, "");

            if (Player.PvP_Validatated)
            {
                Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleStart, Player.ID, "", PVPPlayer.Network.Client));
                Core.Logger.Log(Player.isGameJoltPlayer ?
                        Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "have accept the battle from " + PVPPlayerName) :
                        Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have accept the battle from " + PVPPlayerName), Logger.LogTypes.PvP, p.Client);

                Player.PvP_Status = Player.PvPTypes.Battling;
            }
            else
            {
                string ValidationResult = Player.DoPvPValidation();
                Player.PvP_Validatated = true;

                if (ValidationResult == null)
                {
                    Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleStart, Player.ID, "", PVPPlayer.Network.Client));
                    Core.Logger.Log(Player.isGameJoltPlayer ?
                            Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "have accept the battle from " + PVPPlayerName) :
                            Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have accept the battle from " + PVPPlayerName), Logger.LogTypes.PvP, p.Client);

                    Player.PvP_Status = Player.PvPTypes.Battling;
                }
                else if (ValidationResult.Contains("You may use this team"))
                {
                    Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, ValidationResult, Player.Network.Client));

                    Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleStart, Player.ID, "", PVPPlayer.Network.Client));
                    Core.Logger.Log(Player.isGameJoltPlayer ?
                            Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "have accept the battle from " + PVPPlayerName) :
                            Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have accept the battle from " + PVPPlayerName), Logger.LogTypes.PvP, p.Client);

                    Player.PvP_Status = Player.PvPTypes.Battling;
                }
                else
                {
                    Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_PVPVALIDATION", ValidationResult), Player.Network.Client));
                }
            }
        }

        private void HandleBattleClientData(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Player PVPPlayer = Core.Pokemon3DPlayer.GetPlayer(p.DataItems[0].ToInt());

            Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleClientData, Player.ID, p.DataItems[1], PVPPlayer.Network.Client));
        }

        private void HandleBattleHostData(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Player PVPPlayer = Core.Pokemon3DPlayer.GetPlayer(p.DataItems[0].ToInt());

            Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleHostData, Player.ID, p.DataItems[1], PVPPlayer.Network.Client));
        }

        private void HandleBattlePokemonData(Package p)
        {
            Player Player = Core.Pokemon3DPlayer.GetPlayer(p.Client);
            Player PVPPlayer = Core.Pokemon3DPlayer.GetPlayer(p.DataItems[0].ToInt());

            Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattlePokemonData, Player.ID, p.DataItems[1], PVPPlayer.Network.Client));
        }

        private void HandleServerDataRequest(Package p)
        {
            List<string> DataItems = new List<string>
            {
                Core.Pokemon3DPlayer.Count.ToString(),
                Core.Setting.MaxPlayers == -1 ? int.MaxValue.ToString() : Core.Setting.MaxPlayers.ToString(),
                Core.Setting.ServerName,
                string.IsNullOrWhiteSpace(Core.Setting.ServerMessage) ? "" : Core.Setting.ServerMessage
            };

            if (Core.Pokemon3DPlayer.Count > 0)
            {
                for (int i = 0; i < Core.Pokemon3DPlayer.Count; i++)
                {
                    DataItems.Add(Core.Pokemon3DPlayer[i].isGameJoltPlayer ? string.Format("{0} ({1})", Core.Pokemon3DPlayer[i].Name, Core.Pokemon3DPlayer[i].GameJoltID.ToString()) : Core.Pokemon3DPlayer[i].Name);
                }
            }

            Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.ServerInfoData, DataItems, p.Client));
        }

        private void HandleChatCommand(Package p)
        {
            Core.Command.HandleAllCommand(p);
        }
        #endregion Pokemon 3D Data
    }
}