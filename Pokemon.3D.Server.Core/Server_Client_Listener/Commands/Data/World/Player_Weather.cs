using System;
using System.Collections.Generic;
using Pokemon_3D_Server_Core.Server_Client_Listener.Interface;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data.World
{
    /// <summary>
    /// Class containing Weather Function.
    /// </summary>
    public class Player_Weather : ICommand
    {
        /// <summary>
        /// Name of the command. [To use, add "/" before the name]
        /// </summary>
        public string Name { get; } = "Player.Weather";

        /// <summary>
        /// Short Description of the command.
        /// </summary>
        public string Description { get; } = "Change the player weather.";

        /// <summary>
        /// Minimum Permission require to use this command.
        /// </summary>
        public Player.OperatorTypes RequiredPermission { get; } = Player.OperatorTypes.GameJoltPlayer;

        /// <summary>
        /// Handle the Package data.
        /// </summary>
        /// <param name="p">Package data.</param>
        /// <param name="Player">Player.</param>
        public void Handle(Package p, Player Player = null)
        {
            // Start from the most inner depth Command.
            #region /Player.Weather <id>
            if (this.MatchRequiredParam(p, Functions.CommandParamType.Integer))
            {
                List<string> Group = this.Groups(p, Functions.CommandParamType.Integer);

                if (Player != null && this.MatchRequiredPermission(Player))
                {
                    OnlineSetting Settings = Player.GetOnlineSetting();
                    Settings.Weather = Group[0].ToInt().RollOver(-5, 9);
                    Settings.CurrentWorldWeather = Core.World.GenerateWeather(Settings.Weather, Settings.Season);
                    Settings.LastWorldUpdate = DateTime.Now;

                    Player.CommandFeedback(Core.World.ToString(Settings.CurrentWorldSeason, Settings.CurrentWorldWeather), $"have changed the player weather.");
                }
            }
            #endregion /Player.Weather <id>
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
                        $"Usage: /Player.Weather <ID>",
                        $"-------------------------------------",
                        $"ID: Weather ID.",
                        $"Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 | WeatherSeason = -3 | Real World Weather = -4 | Server Default = -5",
                        $"-------------------------------------",
                        $"Description: {Description}",
                        $"Required Permission: GameJolt Player."
                        );
                    break;
            }
        }
    }
}