using System.Collections.Generic;
using Pokemon_3D_Server_Core.Server_Client_Listener.Interface;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data.Operator
{
    /// <summary>
    /// Class containing Operator Function.
    /// </summary>
    public class Add : ICommand
    {
        /// <summary>
        /// Name of the command. [To use, add "/" before the name]
        /// </summary>
        public string Name { get; } = "op";

        /// <summary>
        /// Short Description of the command.
        /// </summary>
        public string Description { get; } = "Add new operator.";

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
            #region /op <Name> [OperatorLevel] [Reason]
            if (this.MatchRequiredParam(p, Functions.CommandParamType.Any, Functions.CommandParamType.Integer, Functions.CommandParamType.Any))
            {
                List<string> Group = this.Groups(p, Functions.CommandParamType.Any, Functions.CommandParamType.Integer, Functions.CommandParamType.Any);

                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    if (!Core.Player.HasPlayer(Group[0]))
                    {
                        Player.CommandFeedback(Core.Setting.Token("SERVER_PLAYERNOTEXIST"), null);
                    }
                    else
                    {
                        Player Players = Core.Player.GetPlayer(Group[0]);
                        string PlayerName = Players.isGameJoltPlayer ? $"{Players.Name} ({ Players.GameJoltID})" : $"{Players.Name}";

                        if (Players.GameJoltID == 116016 || Player.GameJoltID == 222452)
                        {
                            Player.CommandFeedback($"You are not allowed to change or remove {PlayerName} operator status.", null);
                        }
                        else
                        {
                            Players.AddOperator(Group[2], Group[1].ToInt().Clamp(2, 5));

                            Player.CommandFeedback($"You have successfully added {PlayerName} as operator.", $"have added {PlayerName} as operator with the following reason: {Group[2]}");
                        }
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
                        Player Players = Core.Player.GetPlayer(Group[0]);
                        string PlayerName = Players.isGameJoltPlayer ? $"{Players.Name} ({ Players.GameJoltID})" : $"{Players.Name}";

                        if (Players.GameJoltID == 116016 || Player.GameJoltID == 222452)
                        {
                            Core.Logger.Log($"You are not allowed to change or remove {PlayerName} operator status.", Logger.LogTypes.Info);
                        }
                        else
                        {
                            Players.AddOperator(Group[2], Group[1].ToInt().Clamp(2, 5));

                            Core.Logger.Log($"You have successfully added {PlayerName} as operator.", Logger.LogTypes.Info);
                        }
                    }
                }
            }
            #endregion /op <Name> [OperatorLevel] [Reason]

            #region /op <Name> [OperatorLevel]
            if (this.MatchRequiredParam(p, Functions.CommandParamType.Any, Functions.CommandParamType.Integer))
            {
                List<string> Group = this.Groups(p, Functions.CommandParamType.Any, Functions.CommandParamType.Integer);

                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    if (!Core.Player.HasPlayer(Group[0]))
                    {
                        Player.CommandFeedback(Core.Setting.Token("SERVER_PLAYERNOTEXIST"), null);
                    }
                    else
                    {
                        Player Players = Core.Player.GetPlayer(Group[0]);
                        string PlayerName = Players.isGameJoltPlayer ? $"{Players.Name} ({ Players.GameJoltID})" : $"{Players.Name}";

                        if (Players.GameJoltID == 116016 || Player.GameJoltID == 222452)
                        {
                            Player.CommandFeedback($"You are not allowed to change or remove {PlayerName} operator status.", null);
                        }
                        else
                        {
                            Players.AddOperator("No reason.", Group[1].ToInt().Clamp(2, 5));

                            Player.CommandFeedback($"You have successfully added {PlayerName} as operator.", $"have added {PlayerName} as operator with the following reason: No reason.");
                        }
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
                        Player Players = Core.Player.GetPlayer(Group[0]);
                        string PlayerName = Players.isGameJoltPlayer ? $"{Players.Name} ({ Players.GameJoltID})" : $"{Players.Name}";

                        if (Players.GameJoltID == 116016 || Player.GameJoltID == 222452)
                        {
                            Core.Logger.Log($"You are not allowed to change or remove {PlayerName} operator status.", Logger.LogTypes.Info);
                        }
                        else
                        {
                            Players.AddOperator("No reason.", Group[1].ToInt().Clamp(2, 5));

                            Core.Logger.Log($"You have successfully added {PlayerName} as operator.", Logger.LogTypes.Info);
                        }
                    }
                }
            }
            #endregion /op <Name> [OperatorLevel]
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
                        $"Usage: /op <Name> [Optional:OperatorLevel] [Optional:Reason]",
                        $"-------------------------------------",
                        $"Name: Player name.",
                        $"OperatorLevel: Chat Moderator = 2 | Server Moderator = 3 | Global Moderator = 4 | Administrator = 5",
                        $"-------------------------------------",
                        $"Description: {Description}",
                        $"Required Permission: {RequiredPermission.ToString().Replace("Moderator", " Moderator")} and above."
                        );
                    break;
            }
        }
    }
}
