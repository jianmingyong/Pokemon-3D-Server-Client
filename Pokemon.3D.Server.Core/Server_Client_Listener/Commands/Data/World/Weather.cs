using System.Collections.Generic;
using Pokemon_3D_Server_Core.Server_Client_Listener.Interface;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data.World
{
    /// <summary>
    /// Class containing Weather Function.
    /// </summary>
    public class Weather : ICommand
    {
        /// <summary>
        /// Name of the command. [To use, add "/" before the name]
        /// </summary>
        public string Name { get; } = "Weather";

        /// <summary>
        /// Short Description of the command.
        /// </summary>
        public string Description { get; } = "Change the global weather.";

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
            #region /Weather <id>
            if (this.MatchRequiredParam(p, Functions.CommandParamType.Integer))
            {
                List<string> Group = this.Groups(p, Functions.CommandParamType.Integer);

                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    Core.World.Weather = Core.World.GenerateWeather(Group[0].ToInt(), Core.World.Season);
                    
                    Player.CommandFeedback(Core.World.ToString(), string.Format("have changed the global weather."));
                }
                else if (Player == null)
                {
                    Core.World.Weather = Core.World.GenerateWeather(Group[0].ToInt(), Core.World.Season);

                    Core.Logger.Log(Core.World.ToString(), Logger.LogTypes.Info);
                }
            }
            #endregion /Weather <id>
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
                        $"Usage: /Weather <ID>",
                        $"-------------------------------------",
                        $"ID: Weather ID.",
                        $"Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 | WeatherSeason = -3 | Real World Weather = -4",
                        $"-------------------------------------",
                        $"Description: {Description}",
                        $"Required Permission: {RequiredPermission.ToString().Replace("Moderator", " Moderator")} and above."
                        );
                    break;
            }
        }
    }
}