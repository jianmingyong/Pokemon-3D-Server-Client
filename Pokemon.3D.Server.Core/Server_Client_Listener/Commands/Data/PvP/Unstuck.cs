using System.Collections.Generic;
using Pokemon_3D_Server_Core.Server_Client_Listener.Interface;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data.PvP
{
    /// <summary>
    /// Class containing Unstuck Function.
    /// </summary>
    public class Unstuck : ICommand
    {
        /// <summary>
        /// Name of the command. [To use, add "/" before the name]
        /// </summary>
        public string Name { get; } = "Unstuck";

        /// <summary>
        /// Short Description of the command.
        /// </summary>
        public string Description { get; } = "Unstuck Player in PvP.";

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
            #region /Unstuck
            if (this.MatchRequiredParam(p,  Functions.CommandParamType.Nothing))
            {
                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    if (Player.PvP_Status == Player.PvPTypes.Battling)
                    {
                        Player.PvP_Status = Player.PvPTypes.Nothing;
                        Core.Pokemon3DPlayer.SentToPlayer(new Package(Package.PackageTypes.BattleQuit, Player.ID, "", Core.Pokemon3DPlayer.GetPlayer(Player.PvP_OpponentID).Network.Client));
                    }

                    Player.CommandFeedback(null, string.Format("have forced quit a battle."));
                }
            }
            #endregion /Unstuck
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
                        $"Usage: /Unstuck",
                        $"-------------------------------------",
                        $"Description: {Description}",
                        $"Required Permission: {RequiredPermission.ToString().Replace("Moderator", " Moderator")} and above."
                        );
                    break;
            }
        }
    }
}