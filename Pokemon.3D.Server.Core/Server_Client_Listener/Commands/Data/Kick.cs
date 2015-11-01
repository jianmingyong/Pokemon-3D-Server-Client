using System;
using System.Collections.Generic;
using Pokemon_3D_Server_Core.Server_Client_Listener.Interface;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data
{
    /// <summary>
    /// Class containing Kick Function.
    /// </summary>
    public class Kick : ICommand
    {
        /// <summary>
        /// Name of the command. [To use, add "/" before the name]
        /// </summary>
        public string Name { get; } = "Kick";

        /// <summary>
        /// Short Description of the command.
        /// </summary>
        public string Description { get; } = "Kick the player from the server.";

        /// <summary>
        /// Minimum Permission require to use this command.
        /// </summary>
        public Player.OperatorTypes RequiredPermission { get; } = Player.OperatorTypes.ServerModerator;

        /// <summary>
        /// Handle the Package data.
        /// </summary>
        /// <param name="p">Package data.</param>
        /// <param name="Player">Player.</param>
        public void Handle(Package p, Player Player = null)
        {
            // Start from the most inner depth Command.
            #region /Kick <Name> [Reason]
            if (this.MatchRequiredParam(p, Functions.CommandParamType.Any, Functions.CommandParamType.Any))
            {
                List<string> Group = this.Groups(p, Functions.CommandParamType.Any, Functions.CommandParamType.Any);

                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    if (!Core.Player.HasPlayer(Group[0]))
                    {
                        Player.CommandFeedback(Core.Setting.Token("SERVER_PLAYERNOTEXIST"), null);
                        return;
                    }
                    else if (Player != null && string.Equals(Player.Name, Group[0], StringComparison.Ordinal))
                    {
                        Player.CommandFeedback(Core.Setting.Token("SERVER_KICKSELF"), null);
                        return;
                    }
                    else
                    {
                        Player KickPlayer = Core.Player.GetPlayer(Group[0]);
                        string KickPlayerName = KickPlayer.isGameJoltPlayer ? $"{KickPlayer.Name} ({KickPlayer.GameJoltID.ToString()})" : KickPlayer.Name;

                        Core.Player.Remove(KickPlayer.ID, Core.Setting.Token("SERVER_KICKED", Group[1]));

                        Player.CommandFeedback("You have successfully kicked " + KickPlayerName, $"have kick {KickPlayerName} with the following reason: {Group[1]}");
                        return;
                    }
                }
                else if (Player == null)
                {
                    if (!Core.Player.HasPlayer(Group[0]))
                    {
                        Core.Logger.Log(Core.Setting.Token("SERVER_PLAYERNOTEXIST"), Logger.LogTypes.Info);
                        return;
                    }
                    else
                    {
                        Player KickPlayer = Core.Player.GetPlayer(Group[0]);
                        string KickPlayerName = KickPlayer.isGameJoltPlayer ? $"{KickPlayer.Name} ({KickPlayer.GameJoltID.ToString()})" : KickPlayer.Name;

                        Core.Player.Remove(KickPlayer.ID, Core.Setting.Token("SERVER_KICKED", Group[1]));

                        Core.Logger.Log("You have successfully kicked " + KickPlayerName, Logger.LogTypes.Info);
                        return;
                    }
                }
            }
            #endregion /Kick <Name> [Reason]

            #region /kick <Name>
            if (this.MatchRequiredParam(p, Functions.CommandParamType.Any))
            {
                List<string> Group = this.Groups(p, Functions.CommandParamType.Any);

                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    if (!Core.Player.HasPlayer(Group[0]))
                    {
                        Player.CommandFeedback(Core.Setting.Token("SERVER_PLAYERNOTEXIST"), null);
                    }
                    else if (Player != null && string.Equals(Player.Name, Group[0], StringComparison.Ordinal))
                    {
                        Player.CommandFeedback(Core.Setting.Token("SERVER_KICKSELF"), null);
                    }
                    else
                    {
                        Player KickPlayer = Core.Player.GetPlayer(Group[0]);
                        string KickPlayerName = KickPlayer.isGameJoltPlayer ? string.Format("{0} ({1})", KickPlayer.Name, KickPlayer.GameJoltID.ToString()) : KickPlayer.Name;

                        Core.Player.Remove(KickPlayer.ID, Core.Setting.Token("SERVER_KICKED", "No reason."));

                        Player.CommandFeedback("You have successfully kicked " + KickPlayerName, string.Format("have kick {0} with the following reason: No reason.", KickPlayerName));
                    }
                }
                else if (Player == null)
                {
                    if (!Core.Player.HasPlayer(Group[0]))
                    {
                        Core.Logger.Log(Core.Setting.Token("SERVER_PLAYERNOTEXIST"), Logger.LogTypes.Info);
                    }
                    else
                    {
                        Player KickPlayer = Core.Player.GetPlayer(Group[0]);
                        string KickPlayerName = KickPlayer.isGameJoltPlayer ? string.Format("{0} ({1})", KickPlayer.Name, KickPlayer.GameJoltID.ToString()) : KickPlayer.Name;

                        Core.Player.Remove(KickPlayer.ID, Core.Setting.Token("SERVER_KICKED", "No reason."));

                        Core.Logger.Log("You have successfully kicked " + KickPlayerName, Logger.LogTypes.Info);
                    }
                }
            }
            #endregion /kick <Name>
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
                        $"Usage: /kick <Name> [Optional:Reason]",
                        $"-------------------------------------",
                        $"Name: Player Name.",
                        $"Reason: Reason to kick. [Default: No reason.]",
                        $"-------------------------------------",
                        $"Description: {Description}",
                        $"Required Permission: {RequiredPermission.ToString().Replace("Moderator", " Moderator")} and above."
                        );
                    break;
            }
        }
    }
}