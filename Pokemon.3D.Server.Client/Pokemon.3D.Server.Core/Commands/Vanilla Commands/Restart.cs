using Pokemon_3D_Server_Core.Events;
using Pokemon_3D_Server_Core.Interface;
using Pokemon_3D_Server_Core.Packages;
using Pokemon_3D_Server_Core.Players;

namespace Pokemon_3D_Server_Core.Commands
{
    /// <summary>
    /// Class containing Restart Function.
    /// </summary>
    public class Restart : ICommand
    {
        /// <summary>
        /// Name of the command. [To use, add "/" before the name]
        /// </summary>
        public string Name { get; } = "Restart";

        /// <summary>
        /// Short Description of the command.
        /// </summary>
        public string Description { get; } = "Restart the server.";

        /// <summary>
        /// Minimum Permission require to use this command.
        /// </summary>
        public Player.OperatorTypes RequiredPermission { get; } = Player.OperatorTypes.Administrator;

        /// <summary>
        /// Handle the Package data.
        /// </summary>
        /// <param name="p">Package data.</param>
        /// <param name="Player">Player.</param>
        public void Handle(Package p, Player Player = null)
        {
            // Start from the most inner depth Command.
            #region /Restart
            if (this.MatchRequiredParam(p, Functions.CommandParamType.Nothing))
            {
                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    ClientEvent.Invoke(ClientEvent.Types.Restart, null);
                }
                else if (Player == null)
                {
                    ClientEvent.Invoke(ClientEvent.Types.Restart, null);
                }
            }
            #endregion /Restart
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
                        string.Format("Usage: /restart"),
                        string.Format("-------------------------------------"),
                        string.Format("Description: {0}", Description),
                        string.Format("Required Permission: {0} and above.", RequiredPermission.ToString().Replace("Moderator", " Moderator"))
                        );
                    break;
            }
        }
    }
}
