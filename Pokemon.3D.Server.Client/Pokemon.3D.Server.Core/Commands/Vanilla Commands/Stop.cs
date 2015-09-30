using System.Collections.Generic;
using Pokemon_3D_Server_Core.Interface;
using Pokemon_3D_Server_Core.Packages;
using Pokemon_3D_Server_Core.Players;
using static Pokemon_3D_Server_Core.Players.Player;
using static Pokemon_3D_Server_Core.Loggers.Logger;
using static Pokemon_3D_Server_Core.Commands.CommandFunctions;
using static Pokemon_3D_Server_Core.Packages.Package;

namespace Pokemon_3D_Server_Core.Commands
{
    /// <summary>
    /// Class containing Stop Function.
    /// </summary>
    public class Stop : ICommand
    {
        public string Name { get; } = "Stop";

        public string Description { get; } = "Stop the server from running.";

        public OperatorTypes RequiredPermission { get; } = OperatorTypes.Administrator;

        public void Handle(Package p, Player Player = null)
        {
            // Start from the most inner depth Command.
            #region /stop
            if (this.MatchRequiredParam(p,true,CommandParamType.Nothing))
            {
                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    throw new System.NotImplementedException("/stop is not implemented.");
                }
                else if (Player == null)
                {
                    throw new System.NotImplementedException("/stop is not implemented.");
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