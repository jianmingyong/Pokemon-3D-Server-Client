using System.Collections.Generic;
using Pokemon_3D_Server_Core.Server_Client_Listener.Interface;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data
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
                    Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, Group[0], null));

                    Core.Logger.Log(Group[0], Logger.LogTypes.Server);
                    Player.CommandFeedback(null, string.Format("have sent a server chat."));
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
                        $"---------- Help: {Name} ----------",
                        $"Usage: /Say <Message>",
                        $"-------------------------------------",
                        $"Message: Message.",
                        $"-------------------------------------",
                        $"Description: {Description}",
                        $"Required Permission: {RequiredPermission.ToString().Replace("Moderator", " Moderator")} and above."
                        );
                    break;
            }
        }
    }
}