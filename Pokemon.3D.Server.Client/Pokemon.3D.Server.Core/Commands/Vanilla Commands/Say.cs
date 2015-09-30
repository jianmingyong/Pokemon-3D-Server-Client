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
    /// Class containing Say Function.
    /// </summary>
    public class Say : ICommand
    {
        public string Name { get; } = "Say";

        public string Description { get; } = "Chat globally to all player.";

        public OperatorTypes RequiredPermission { get; } = OperatorTypes.ChatModerator;

        public void Handle(Package p, Player Player = null)
        {
            // Start from the most inner depth Command.
            #region /Say <Message>
            if (this.MatchRequiredParam(p, true, CommandParamType.Any))
            {
                List<string> Group = this.Groups(p, CommandParamType.Any);

                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    Core.Server.SendToAllPlayer(new Package(PackageTypes.ChatMessage, Group[0], Player.Network.Client));

                    Player.CommandFeedback(Group[0], string.Format("have sent a server chat."));
                }
                else if (Player == null)
                {
                    Core.Server.SendToAllPlayer(new Package(PackageTypes.ChatMessage, Group[0], Player.Network.Client));

                    Core.Logger.Add(Group[0], LogTypes.Server);
                }
            }
            #endregion /Say <Message>
        }

        public void Help(int Pages, Player Player = null)
        {
            switch (Pages)
            {
                default:
                    this.HelpPageGenerator(Player,
                        string.Format("---------- Help: {0} ----------", Name),
                        string.Format("Usage: /Say [Message]"),
                        string.Format("-------------------------------------"),
                        string.Format("Message: Message."),
                        string.Format("-------------------------------------"),
                        string.Format("Description: {0}", Description),
                        string.Format("Required Permission: {0} and above.", RequiredPermission.ToString().Replace("Moderator", " Moderator"))
                        );
                    break;
            }
        }
    }
}