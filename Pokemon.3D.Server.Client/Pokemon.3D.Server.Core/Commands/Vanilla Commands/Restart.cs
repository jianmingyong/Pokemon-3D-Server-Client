using Pokemon_3D_Server_Core.Event;
using Pokemon_3D_Server_Core.Interface;
using Pokemon_3D_Server_Core.Packages;
using Pokemon_3D_Server_Core.Players;

namespace Pokemon_3D_Server_Core.Commands.Vanilla_Commands
{
    /// <summary>
    /// Class containing Restart Function.
    /// </summary>
    public class Restart : ICommand
    {
        public string Name { get; } = "Restart";

        public string Description { get; } = "Restart the server.";

        public Player.OperatorTypes RequiredPermission { get; } = Player.OperatorTypes.Administrator;

        public void Handle(Package p, Player Player = null)
        {
            // Start from the most inner depth Command.
            #region /Restart
            if (this.MatchRequiredParam(p, true, Functions.CommandParamType.Nothing))
            {
                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    ClientEvent.Invoke(ClientEvent.Types.Restart);
                }
                else if (Player == null)
                {
                    ClientEvent.Invoke(ClientEvent.Types.Restart);
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
