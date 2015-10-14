using System.Collections.Generic;
using Pokemon_3D_Server_Core.Interface;
using Pokemon_3D_Server_Core.Packages;
using Pokemon_3D_Server_Core.Players;
using Pokemon_3D_Server_Core.Loggers;

namespace Pokemon_3D_Server_Core.Commands
{
    /// <summary>
    /// Class containing Say Function.
    /// </summary>
    public class Say : ICommand
    {
        /// <summary>
        /// Name of the command. [To use, add "/" before the name]
        /// </summary>
        public string Name { get; } = "Say";

        /// <summary>
        /// Short Description of the command.
        /// </summary>
        public string Description { get; } = "Chat globally to all player.";

        /// <summary>
        /// Minimum Permission require to use this command.
        /// </summary>
        public Player.OperatorTypes RequiredPermission { get; } = Player.OperatorTypes.ChatModerator;

        /// <summary>
        /// Handle the Package data.
        /// </summary>
        /// <param name="p">Package data.</param>
        /// <param name="Player">Player.</param>
        public void Handle(Package p, Player Player = null)
        {
            // Start from the most inner depth Command.
            #region /Say <Message>
            if (this.MatchRequiredParam(p,  Functions.CommandParamType.Any))
            {
                List<string> Group = this.Groups(p, Functions.CommandParamType.Any);

                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, Group[0], Player.Network.Client));

                    Player.CommandFeedback(Group[0], string.Format("have sent a server chat."));
                }
                else if (Player == null)
                {
                    Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, Group[0], null));

                    Core.Logger.Log(Group[0], Logger.LogTypes.Server);
                }
            }
            #endregion /Say <Message>
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