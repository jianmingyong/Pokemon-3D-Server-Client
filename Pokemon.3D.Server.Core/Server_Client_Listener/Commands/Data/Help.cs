using System;
using System.Collections.Generic;
using Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data.Chat_Channels;
using Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data.Client;
using Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data.PvP;
using Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data.World;
using Pokemon_3D_Server_Core.Server_Client_Listener.Interface;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data
{
    /// <summary>
    /// Class containing Help Function.
    /// </summary>
    public class HelpCommand : ICommand
    {
        /// <summary>
        /// Name of the command. [To use, add "/" before the name]
        /// </summary>
        public string Name { get; } = "Help";

        /// <summary>
        /// Short Description of the command.
        /// </summary>
        public string Description { get; } = "Display help.";

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
            #region /Help <page>
            if (this.MatchRequiredParam(p, Functions.CommandParamType.Integer))
            {
                List<string> Group = this.Groups(p, Functions.CommandParamType.Integer);

                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    List<string> GetHelpContent = GenerateHelp(Group[0].ToInt());

                    for (int i = 0; i < GetHelpContent.Count; i++)
                    {
                        Player.CommandFeedback(GetHelpContent[i], null);
                    }
                }
                else if (Player == null)
                {
                    List<string> GetHelpContent = GenerateHelp(Group[0].ToInt());

                    for (int i = 0; i < GetHelpContent.Count; i++)
                    {
                        Core.Logger.Log(GetHelpContent[i], Logger.LogTypes.Info);
                    }
                }
            }
            #endregion /Help <page>

            #region /Help <name>
            if (this.MatchRequiredParam(p, Functions.CommandParamType.Any))
            {
                List<string> Group = this.Groups(p, Functions.CommandParamType.Any);

                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    for (int i = 0; i < Core.Command.Count; i++)
                    {
                        if (string.Equals(Group[0], Core.Command[i].Name, StringComparison.OrdinalIgnoreCase))
                        {
                            Core.Command[i].Help(0, Player);
                        }
                    }
                }
                else if (Player == null)
                {
                    for (int i = 0; i < Core.Command.Count; i++)
                    {
                        if (string.Equals(Group[0], Core.Command[i].Name, StringComparison.OrdinalIgnoreCase))
                        {
                            Core.Command[i].Help(0, Player);
                        }
                    }
                }
            }
            #endregion /Help <name>

            #region /Help
            if (this.MatchRequiredParam(p, Functions.CommandParamType.Nothing))
            {
                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    List<string> GetHelpContent = GenerateHelp(0);

                    for (int i = 0; i < GetHelpContent.Count; i++)
                    {
                        Player.CommandFeedback(GetHelpContent[i], null);
                    }
                }
                else if (Player == null)
                {
                    List<string> GetHelpContent = GenerateHelp(0);

                    for (int i = 0; i < GetHelpContent.Count; i++)
                    {
                        Core.Logger.Log(GetHelpContent[i], Logger.LogTypes.Info);
                    }
                }
            }
            #endregion /Help
        }

        /// <summary>
        /// Create a Help Page.
        /// </summary>
        /// <param name="Pages">Page Number. Start from Zero.</param>
        /// <param name="Player">Player.</param>
        public void Help(int Pages, Player Player = null) { }

        /// <summary>
        /// Gernerate Help Content.
        /// </summary>
        /// <param name="Pages">Page Number.</param>
        /// <returns></returns>
        public List<string> GenerateHelp(int Pages)
        {
            // Each Page can only have 14 indexes.
            switch (Pages)
            {
                case 0:
                case 1:
                    return new List<string>
                    {
                        $"---------- Help: Index (1/2) ----------",
                        $"Use /help [Name/Index] to get page index of help.",
                        $"---------- Category: No Category ----------",
                        $"/{new Kick().Name} - {new Kick().Description}",
                        $"/{new Say().Name} - {new Say().Description}",
                        $"/{new ChatChannels_Change().Name} - {new ChatChannels_Change().Description}",
                        $"---------- Category: Client ----------",
                        $"/{new About().Name} - {new About().Description}",
                        $"/{new Restart().Name} - {new Restart().Description}",
                        $"/{new Stop().Name} - {new Stop().Description}",
                        $"/{new Update().Name} - {new Update().Description}",
                        $"---------- Category: Operator ----------",
                        $"/{new Operator.Add().Name} - {new Operator.Add().Description}",
                        $"/{new Operator.Remove().Name} - {new Operator.Remove().Description}",
                    };
                case 2:
                    return new List<string>
                    {
                        $"---------- Help: Index (2/2) ----------",
                        $"Use /help [Name/Index] to get page index of help.",
                        $"---------- Category: World ----------",
                        $"/{new Player_Season().Name} - {new Player_Season().Description}",
                        $"/{new Player_Weather().Name} - {new Player_Weather().Description}",
                        $"/{new Player_World().Name} - {new Player_World().Description}",
                        $"/{new Season().Name} - {new Season().Description}",
                        $"/{new Timeoffset().Name} - {new Timeoffset().Description}",
                        $"/{new Weather().Name} - {new Weather().Description}",
                        $"/{new World.World().Name} - {new World.World().Description}",
                        $"---------- Category: PvP ----------",
                        $"/{new League().Name} - {new League().Description}",
                    };
                default:
                    return new List<string>
                    {
                        $"---------- Help: Index (1/2) ----------",
                        $"Use /help [Name/Index] to get page index of help.",
                        $"---------- Category: No Category ----------",
                        $"/{new Kick().Name} - {new Kick().Description}",
                        $"/{new Say().Name} - {new Say().Description}",
                        $"---------- Category: Client ----------",
                        $"/{new About().Name} - {new About().Description}",
                        $"/{new Restart().Name} - {new Restart().Description}",
                        $"/{new Stop().Name} - {new Stop().Description}",
                        $"/{new Update().Name} - {new Update().Description}",
                        $"---------- Category: Operator ----------",
                        $"/{new Operator.Add().Name} - {new Operator.Add().Description}",
                        $"/{new Operator.Remove().Name} - {new Operator.Remove().Description}",
                    };
            }
        }
    }
}
