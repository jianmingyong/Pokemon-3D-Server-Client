using Pokemon_3D_Server_Core.Interface;
using Pokemon_3D_Server_Core.Network;
using Pokemon_3D_Server_Core.Packages;
using Pokemon_3D_Server_Core.Players;
using static Pokemon_3D_Server_Core.Commands.CommandFunctions;
using static Pokemon_3D_Server_Core.Players.Player;

namespace Pokemon_3D_Server_Core.Commands.Vanilla_Commands
{
    /// <summary>
    /// Class containing Restart Function.
    /// </summary>
    public class Restart : ICommand
    {
        public string Name { get; } = "Restart";

        public string Description { get; } = "Restart the server.";

        public OperatorTypes RequiredPermission { get; } = OperatorTypes.Administrator;

        public void Handle(Package p, Player Player = null)
        {
            // Start from the most inner depth Command.
            #region /Restart
            if (this.MatchRequiredParam(p, true, CommandParamType.Nothing))
            {
                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    RestartTrigger.Restart();
                }
                else if (Player == null)
                {
                    RestartTrigger.Restart();
                }
            }
            #endregion /Restart
        }

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
