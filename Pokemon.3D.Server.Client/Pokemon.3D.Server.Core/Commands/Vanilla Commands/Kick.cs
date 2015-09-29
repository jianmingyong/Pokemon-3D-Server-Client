using System.Collections.Generic;
using Pokemon_3D_Server_Core.Interface;
using Pokemon_3D_Server_Core.Packages;
using Pokemon_3D_Server_Core.Players;
using static Pokemon_3D_Server_Core.Loggers.Logger;
using static Pokemon_3D_Server_Core.Commands.CommandFunctions;
using static Pokemon_3D_Server_Core.Packages.Package;

namespace Pokemon_3D_Server_Core.Commands
{
    /// <summary>
    /// Class containing Kick Function.
    /// </summary>
    public class Kick : ICommand
    {
        public string Name { get; set; } = "Kick";

        public string Description { get; set; } = "Kick the player from the server.";

        public Player.OperatorTypes RequiredPermission { get; set; } = Player.OperatorTypes.ServerModerator;

        public void Handle(Package p, Player Player = null)
        {
            // Start from the most inner depth Command.
            #region /Kick <Name> [Reason]
            if (this.MatchRequiredParam(p, false, CommandParamType.Any, CommandParamType.Any))
            {
                List<string> Group = this.Groups(p, CommandParamType.Any, CommandParamType.Any);

                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    if (!Core.Player.HasPlayer(Group[0]))
                    {
                        Core.Server.SentToPlayer(new Package(PackageTypes.ChatMessage, Core.Setting.Token("SERVER_PLAYERNOTEXIST"), p.Client));
                    }
                    else
                    {
                        Player KickPlayer = Core.Player.GetPlayer(Group[0]);
                        string KickPlayerName = KickPlayer.isGameJoltPlayer ? string.Format("{0} ({1})", KickPlayer.Name, KickPlayer.GameJoltID.ToString()) : KickPlayer.Name;

                        Core.Server.SentToPlayer(new Package(PackageTypes.Kicked, Core.Setting.Token("SERVER_KICKED", Group[1]), KickPlayer.Network.Client));

                        Player.CommandFeedback("You have successfully kicked " + KickPlayerName, string.Format("have kick {0} with the following reason: {1}", KickPlayerName, Group[1]));
                    }
                }
                else if (Player == null)
                {
                    if (!Core.Player.HasPlayer(Group[0]))
                    {
                        Core.Logger.Add(Core.Setting.Token("SERVER_PLAYERNOTEXIST"), LogTypes.Info);
                    }
                    else
                    {
                        Player KickPlayer = Core.Player.GetPlayer(Group[0]);
                        string KickPlayerName = KickPlayer.isGameJoltPlayer ? string.Format("{0} ({1})", KickPlayer.Name, KickPlayer.GameJoltID.ToString()) : KickPlayer.Name;

                        Core.Server.SentToPlayer(new Package(PackageTypes.Kicked, Core.Setting.Token("SERVER_KICKED", Group[1]), KickPlayer.Network.Client));

                        Core.Logger.Add("You have successfully kicked " + KickPlayerName, LogTypes.Info);
                    }
                }
            }
            #endregion /Kick <Name> [Reason]

            #region /kick <Name>
            if (this.MatchRequiredParam(p, true, CommandParamType.Any))
            {
                List<string> Group = this.Groups(p, CommandParamType.Any);

                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    if (!Core.Player.HasPlayer(Group[0]))
                    {
                        Core.Server.SentToPlayer(new Package(PackageTypes.ChatMessage, Core.Setting.Token("SERVER_PLAYERNOTEXIST"), p.Client));
                    }
                    else
                    {
                        Player KickPlayer = Core.Player.GetPlayer(Group[0]);
                        string KickPlayerName = KickPlayer.isGameJoltPlayer ? string.Format("{0} ({1})", KickPlayer.Name, KickPlayer.GameJoltID.ToString()) : KickPlayer.Name;

                        Core.Server.SentToPlayer(new Package(PackageTypes.Kicked, Core.Setting.Token("SERVER_KICKED", "No reason."), KickPlayer.Network.Client));

                        Player.CommandFeedback("You have successfully kicked " + KickPlayerName, string.Format("have kick {0} with the following reason: No reason.", KickPlayerName));
                    }
                }
                else if (Player == null)
                {
                    if (!Core.Player.HasPlayer(Group[0]))
                    {
                        Core.Logger.Add(Core.Setting.Token("SERVER_PLAYERNOTEXIST"), LogTypes.Info);
                    }
                    else
                    {
                        Player KickPlayer = Core.Player.GetPlayer(Group[0]);
                        string KickPlayerName = KickPlayer.isGameJoltPlayer ? string.Format("{0} ({1})", KickPlayer.Name, KickPlayer.GameJoltID.ToString()) : KickPlayer.Name;

                        Core.Server.SentToPlayer(new Package(PackageTypes.Kicked, Core.Setting.Token("SERVER_KICKED", "No reason."), KickPlayer.Network.Client));

                        Core.Logger.Add("You have successfully kicked " + KickPlayerName, LogTypes.Info);
                    }
                }
            }
            #endregion /kick <Name>
        }

        public void Help(int Pages)
        {

        }
    }
}
