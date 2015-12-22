using System;
using System.Collections.Generic;
using Pokemon_3D_Server_Core.Server_Client_Listener.Interface;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data.Chat_Channels
{
    /// <summary>
    /// Class containing Kick Function.
    /// </summary>
    public class ChatChannels_Change : ICommand
    {
        /// <summary>
        /// Name of the command. [To use, add "/" before the name]
        /// </summary>
        public string Name { get; } = "ChatChannel";

        /// <summary>
        /// Short Description of the command.
        /// </summary>
        public string Description { get; } = "Change the current Chat Channel.";

        /// <summary>
        /// Minimum Permission require to use this command.
        /// </summary>
        public Player.OperatorTypes RequiredPermission { get; } = Player.OperatorTypes.Player;

        /// <summary>
        /// Handle the Package data.
        /// </summary>
        /// <param name="p">Package data.</param>
        /// <param name="Player">Player.</param>
        public void Handle(Package p, Player Player = null)
        {
            // Start from the most inner depth Command.
            #region /ChatChannel <ID>
            if (this.MatchRequiredParam(p, Functions.CommandParamType.Integer))
            {
                List<string> Group = this.Groups(p, Functions.CommandParamType.Integer);

                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    switch (Group[0].ToString().ToInt().Clamp(0,5))
                    {
                        case (int)Player.ChatChannelType.Default:
                            Player.CC_CurrentChatChannel = Player.ChatChannelType.Default.ToString();
                            break;

                        case (int)Player.ChatChannelType.General:
                            Player.CC_CurrentChatChannel = Player.ChatChannelType.General.ToString();
                            break;

                        case (int)Player.ChatChannelType.Trade:
                            Player.CC_CurrentChatChannel = Player.ChatChannelType.Trade.ToString();
                            break;

                        case (int)Player.ChatChannelType.PvP_Casual:
                            Player.CC_CurrentChatChannel = Player.ChatChannelType.PvP_Casual.ToString();
                            break;

                        case (int)Player.ChatChannelType.PvP_League :
                            Player.CC_CurrentChatChannel = Player.ChatChannelType.PvP_League.ToString();
                            break;

                        case (int)Player.ChatChannelType.German_Lounge:
                            Player.CC_CurrentChatChannel = Player.ChatChannelType.German_Lounge.ToString();
                            break;

                        default:
                            Player.CC_CurrentChatChannel = Player.ChatChannelType.Default.ToString();
                            break;
                    }

                    if (Core.Setting.AllowChatChannels)
                    {
                        for (int i = 0; i < Core.Player.Count; i++)
                        {
                            if (Core.Player[i].CC_CurrentChatChannel == Player.CC_CurrentChatChannel && Core.Player[i] != Player)
                            {
                                Core.Player.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Player.isGameJoltPlayer ?
                                    Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "have joined the current chat channel you are in.") :
                                    Core.Setting.Token("SERVER_COMMANDNOGAMEJOLT", Player.Name, "have joined the current chat channel you are in.")
                                    , Core.Player[i].Network.Client));
                            }
                        }

                        Player.CommandFeedback(Core.Setting.Token("SERVER_CURRENTCHATCHANNEL", Player.CC_CurrentChatChannel), "have changed chat channel.");
                    }
                    
                }
            }
            #endregion /ChatChannel <ID>
        }

        /// <summary>
        /// Create a Help Page.
        /// </summary>
        /// <param name="Pages">Page Number. Start from Zero.</param>
        /// <param name="Player">Player.</param>
        public void Help(int Pages, Player Player = null)
        {
            switch (Pages)
            {
                default:
                    this.HelpPageGenerator(Player,
                        $"---------- Help: {Name} ----------",
                        $"Usage: /ChatChannel <ID>",
                        $"-------------------------------------",
                        $"ID: Chat ID.",
                        $"0 = Default, 1 = General Chat, 2 = Trade Chat, 3 = PvP Casual, 4 = PvP League, 5 = German Lounge",
                        $"-------------------------------------",
                        $"Description: {Description}",
                        $"Required Permission: {RequiredPermission.ToString().Replace("Moderator", " Moderator")} and above."
                        );
                    break;
            }
        }
    }
}