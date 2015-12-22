using System.Collections.Generic;
using Pokemon_3D_Server_Core.Server_Client_Listener.Interface;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data.PvP
{
    /// <summary>
    /// Class containing League Function.
    /// </summary>
    public class League : ICommand
    {
        /// <summary>
        /// Name of the command. [To use, add "/" before the name]
        /// </summary>
        public string Name { get; } = "League";

        /// <summary>
        /// Short Description of the command.
        /// </summary>
        public string Description { get; } = "Change the PvP Rules to obey League.";

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
            #region /League
            if (this.MatchRequiredParam(p,  Functions.CommandParamType.Nothing))
            {
                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    if (Player.PvP_Status == Player.PvPTypes.Lobby)
                    {
                        Player.PvP_Rules = new List<string> { Player.PvPRules.Custom_League.ToString() };
                        Player.PvP_Validatated = false;

                        Core.Player.GetPlayer(Player.PvP_OpponentID).PvP_Rules = new List<string> { Player.PvPRules.Custom_League.ToString() };
                        Core.Player.GetPlayer(Player.PvP_OpponentID).PvP_Validatated = false;

                        Player.CommandFeedback("The PvP match will now obey League rules. For more info refer:", null);
                        Player.CommandFeedback("http://www.aggressivegaming.org/pokemon/link-forums/general-league-rules.219/", null);

                        Core.Player.GetPlayer(Player.PvP_OpponentID).CommandFeedback("The PvP match will now obey League rules. For more info refer:", null);
                        Core.Player.GetPlayer(Player.PvP_OpponentID).CommandFeedback("http://www.aggressivegaming.org/pokemon/link-forums/general-league-rules.219/", null);
                    }
                }
            }
            #endregion /League
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
                        $"Usage: /League",
                        $"-------------------------------------",
                        $"Description: {Description}",
                        $"Required Permission: {RequiredPermission.ToString().Replace("Moderator", " Moderator")} and above."
                        );
                    break;
            }
        }
    }
}