using System.Collections.Generic;
using Pokemon_3D_Server_Core.Server_Client_Listener.Interface;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data.World
{
    /// <summary>
    /// Class containing Timeoffset Function.
    /// </summary>
    public class Timeoffset : ICommand
    {
        /// <summary>
        /// Name of the command. [To use, add "/" before the name]
        /// </summary>
        public string Name { get; } = "Timeoffset";

        /// <summary>
        /// Short Description of the command.
        /// </summary>
        public string Description { get; } = "Change the time offset in seconds.";

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
            #region /Timeoffset <Duration>
            if (this.MatchRequiredParam(p, Functions.CommandParamType.Integer))
            {
                List<string> Group = this.Groups(p, Functions.CommandParamType.Integer);

                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    Core.World.TimeOffset = Group[0].ToInt();

                    Player.CommandFeedback(Core.World.ToString(), $"have changed the world time offset.");
                }
                else if (Player == null)
                {
                    Core.World.TimeOffset = Group[0].ToInt();

                    Core.Logger.Log(Core.World.ToString(), Logger.LogTypes.Info);
                }
            }
            #endregion /Timeoffset <Duration>
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
                        $"Usage: /Timeoffset <Duration>",
                        $"-------------------------------------",
                        $"Duration: Amount of time offset in seconds.",
                        $"-------------------------------------",
                        $"Description: {Description}",
                        $"Required Permission: {RequiredPermission.ToString().Replace("Moderator", " Moderator")} and above."
                        );
                    break;
            }
        }
    }
}