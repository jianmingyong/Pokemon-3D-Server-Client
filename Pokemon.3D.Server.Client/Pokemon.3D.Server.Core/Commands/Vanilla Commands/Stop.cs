using Pokemon_3D_Server_Core.Event;
using Pokemon_3D_Server_Core.Interface;
using Pokemon_3D_Server_Core.Packages;
using Pokemon_3D_Server_Core.Players;

namespace Pokemon_3D_Server_Core.Commands
{
    /// <summary>
    /// Class containing Stop Function.
    /// </summary>
    public class Stop : ICommand
    {
        public string Name { get; } = "Stop";

        public string Description { get; } = "Stop the server from running.";

        public Player.OperatorTypes RequiredPermission { get; } = Player.OperatorTypes.Administrator;

        public void Handle(Package p, Player Player = null)
        {
            // Start from the most inner depth Command.
            #region /stop
            if (this.MatchRequiredParam(p, true, Functions.CommandParamType.Nothing))
            {
                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    ClientEvent.Invoke(ClientEvent.Types.Stop);
                }
                else if (Player == null)
                {
                    ClientEvent.Invoke(ClientEvent.Types.Stop);
                }
            }
            #endregion /stop
        }

        public void Help(int Pages, Player Player = null)
        {
            switch (Pages)
            {
                default:
                    this.HelpPageGenerator(Player,
                        string.Format("---------- Help: {0} ----------", Name),
                        string.Format("Usage: /stop"),
                        string.Format("-------------------------------------"),
                        string.Format("Description: {0}", Description),
                        string.Format("Required Permission: {0} and above.", RequiredPermission.ToString().Replace("Moderator", " Moderator"))
                        );
                    break;
            }
        }
    }
}