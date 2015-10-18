using System.Collections.Generic;
using Pokemon_3D_Server_Core.Server_Client_Listener.Interface;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data
{
    /// <summary>
    /// Class containing Operator Function.
    /// </summary>
    public class Operator_Remove : ICommand
    {
        /// <summary>
        /// Name of the command. [To use, add "/" before the name]
        /// </summary>
        public string Name { get; } = "Operator.Remove";

        /// <summary>
        /// Short Description of the command.
        /// </summary>
        public string Description { get; } = "Remove operator.";

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
            #region /Operator.Remove [Name]
            if (this.MatchRequiredParam(p, Functions.CommandParamType.Any, Functions.CommandParamType.Integer, Functions.CommandParamType.Any))
            {
                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    List<string> Group = this.Groups(p, Functions.CommandParamType.Any, Functions.CommandParamType.Integer, Functions.CommandParamType.Any);

                    if (!Core.Player.HasPlayer(Group[0]))
                    {
                        Core.Player.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_PLAYERNOTEXIST"), p.Client));
                    }
                    else
                    {
                        Player Players = Core.Player.GetPlayer(Group[0]);
                        string PlayerName = Players.isGameJoltPlayer ? $"{Players.Name} ({ Players.GameJoltID})" : $"{Players.Name}";

                        if (Players.IsOperator())
                        {
                            Players.RemoveOperator();

                            Player.CommandFeedback($"You have successfully remove {PlayerName} as operator.", $"have remove {PlayerName} as operator.");
                        }
                        else
                        {
                            Core.Player.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_NOTOPERATOR"), p.Client));
                        }
                    }
                }
                else if (Player == null)
                {
                    List<string> Group = this.Groups(p, Functions.CommandParamType.Any, Functions.CommandParamType.Integer, Functions.CommandParamType.Any);

                    if (!Core.Player.HasPlayer(Group[0]))
                    {
                        Core.Logger.Log(Core.Setting.Token("SERVER_PLAYERNOTEXIST"), Loggers.Logger.LogTypes.Info);
                    }
                    else
                    {
                        Player Players = Core.Player.GetPlayer(Group[0]);
                        string PlayerName = Players.isGameJoltPlayer ? $"{Players.Name} ({ Players.GameJoltID})" : $"{Players.Name}";

                        if (Players.IsOperator())
                        {
                            Players.RemoveOperator();

                            Core.Logger.Log($"You have successfully remove {PlayerName} as operator.", Loggers.Logger.LogTypes.Info);
                        }
                        else
                        {
                            Core.Logger.Log(Core.Setting.Token("SERVER_NOTOPERATOR"), Loggers.Logger.LogTypes.Info);
                        }
                    }
                }
            }
            #endregion /Operator.Remove [Name]
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
                        $"Usage: /Operator.Remove [Name]",
                        $"-------------------------------------",
                        $"Name: Player name.",
                        $"-------------------------------------",
                        $"Description: {Description}",
                        $"Required Permission: {RequiredPermission.ToString().Replace("Moderator", " Moderator")} and above."
                        );
                    break;
            }
        }
    }
}