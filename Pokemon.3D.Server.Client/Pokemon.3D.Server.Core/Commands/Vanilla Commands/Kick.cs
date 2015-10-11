using System.Collections.Generic;
using Pokemon_3D_Server_Core.Interface;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Packages;
using Pokemon_3D_Server_Core.Players;

namespace Pokemon_3D_Server_Core.Commands
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
                        Core.Server.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_PLAYERNOTEXIST"), p.Client));
                    }
                    else
                    {
                        Player KickPlayer = Core.Player.GetPlayer(Group[0]);
                        string KickPlayerName = KickPlayer.isGameJoltPlayer ? string.Format("{0} ({1})", KickPlayer.Name, KickPlayer.GameJoltID.ToString()) : KickPlayer.Name;

                        Core.Server.SentToPlayer(new Package(Package.PackageTypes.Kicked, Core.Setting.Token("SERVER_KICKED", Group[1]), KickPlayer.Network.Client));

                        Player.CommandFeedback("You have successfully kicked " + KickPlayerName, string.Format("have kick {0} with the following reason: {1}", KickPlayerName, Group[1]));
                    }
                }
                else if (Player == null)
                {
                    if (!Core.Player.HasPlayer(Group[0]))
                    {
                        Core.Logger.Add(Core.Setting.Token("SERVER_PLAYERNOTEXIST"), Logger.LogTypes.Info);
                    }
                    else
                    {
                        Player KickPlayer = Core.Player.GetPlayer(Group[0]);
                        string KickPlayerName = KickPlayer.isGameJoltPlayer ? string.Format("{0} ({1})", KickPlayer.Name, KickPlayer.GameJoltID.ToString()) : KickPlayer.Name;

                        Core.Server.SentToPlayer(new Package(Package.PackageTypes.Kicked, Core.Setting.Token("SERVER_KICKED", Group[1]), KickPlayer.Network.Client));

                        Core.Logger.Add("You have successfully kicked " + KickPlayerName, Logger.LogTypes.Info);
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
                        Core.Server.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_PLAYERNOTEXIST"), p.Client));
                    }
                    else
                    {
                        Player KickPlayer = Core.Player.GetPlayer(Group[0]);
                        string KickPlayerName = KickPlayer.isGameJoltPlayer ? string.Format("{0} ({1})", KickPlayer.Name, KickPlayer.GameJoltID.ToString()) : KickPlayer.Name;

                        Core.Server.SentToPlayer(new Package(Package.PackageTypes.Kicked, Core.Setting.Token("SERVER_KICKED", "No reason."), KickPlayer.Network.Client));

                        Player.CommandFeedback("You have successfully kicked " + KickPlayerName, string.Format("have kick {0} with the following reason: No reason.", KickPlayerName));
                    }
                }
                else if (Player == null)
                {
                    if (!Core.Player.HasPlayer(Group[0]))
                    {
                        Core.Logger.Add(Core.Setting.Token("SERVER_PLAYERNOTEXIST"), Logger.LogTypes.Info);
                    }
                    else
                    {
                        Player KickPlayer = Core.Player.GetPlayer(Group[0]);
                        string KickPlayerName = KickPlayer.isGameJoltPlayer ? string.Format("{0} ({1})", KickPlayer.Name, KickPlayer.GameJoltID.ToString()) : KickPlayer.Name;

                        Core.Server.SentToPlayer(new Package(Package.PackageTypes.Kicked, Core.Setting.Token("SERVER_KICKED", "No reason."), KickPlayer.Network.Client));

                        Core.Logger.Add("You have successfully kicked " + KickPlayerName, Logger.LogTypes.Info);
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
                        string.Format("---------- Help: {0} ----------", Name),
                        string.Format("Usage: /kick [Name] [Optional:Reason]"),
                        string.Format("-------------------------------------"),
                        string.Format("Name: Player Name."),
                        string.Format("Reason: Reason to kick. [Default: No reason.]"),
                        string.Format("-------------------------------------"),
                        string.Format("Description: {0}", Description),
                        string.Format("Required Permission: {0} and above.", RequiredPermission.ToString().Replace("Moderator", " Moderator"))
                        );
                    break;
            }
        }
    }
}