using System.Collections.Generic;
using Pokemon_3D_Server_Core.Interface;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Packages;
using Pokemon_3D_Server_Core.Players;
using Pokemon_3D_Server_Core.Modules;

namespace Pokemon_3D_Server_Core.Commands
{
    /// <summary>
    /// Class containing Weather Function.
    /// </summary>
    public class Weather : ICommand
    {
        /// <summary>
        /// Name of the command. [To use, add "/" before the name]
        /// </summary>
        public string Name { get; } = "Global.Weather";

        /// <summary>
        /// Short Description of the command.
        /// </summary>
        public string Description { get; } = "Change the Global Weather.";

        /// <summary>
        /// Minimum Permission require to use this command.
        /// </summary>
        public Player.OperatorTypes RequiredPermission { get; } = Player.OperatorTypes.ServerModerator;

        /// <summary>
        /// Handle the Package data.
        /// </summary>
        /// <param name="p">Package data.</param>
        /// <param name="Player">Player.</param>
        public void Handle(Package p, Player Player = null)
        {
            // Start from the most inner depth Command.
            #region /Global.Weather <id>

            if (this.MatchRequiredParam(p, Functions.CommandParamType.Integer))
            {
                List<string> Group = this.Groups(p, Functions.CommandParamType.Integer);

                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    Core.World.Weather = Core.World.GenerateWeather(Group[0].Toint(), Core.World.Season);
                    
                    Player.CommandFeedback(Core.World.ToString(), string.Format("have changed the Global Weather."));
                }
                else if (Player == null)
                {
                    Core.World.Weather = Core.World.GenerateWeather(Group[0].Toint(), Core.World.Season);

                    Core.Logger.Log(Core.World.ToString(), Logger.LogTypes.Info);
                }
            }
            #endregion /Global.Weather <id>
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
                        string.Format("Usage: /Global.Weather [ID]"),
                        string.Format("-------------------------------------"),
                        string.Format("ID: Weather ID."),
                        string.Format("Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 | Real World Weather = -4"),
                        string.Format("-------------------------------------"),
                        string.Format("Description: {0}", Description),
                        string.Format("Required Permission: {0} and above.", RequiredPermission.ToString().Replace("Moderator", " Moderator"))
                        );
                    break;
            }
        }
    }
}