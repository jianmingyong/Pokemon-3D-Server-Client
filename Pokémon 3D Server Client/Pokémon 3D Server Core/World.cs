using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    /// <summary>
    /// Class containing World Property
    /// </summary>
    public class World
    {
        /// <summary>
        /// A collection of Season Type
        /// </summary>
        public enum SeasonType
        {
            /// <summary>
            /// Winter Season
            /// </summary>
            Winter = 0,

            /// <summary>
            /// Spring Season
            /// </summary>
            Spring = 1,

            /// <summary>
            /// Summer Season
            /// </summary>
            Summer = 2,

            /// <summary>
            /// Fall Season
            /// </summary>
            Fall = 3,

            /// <summary>
            /// Random Season
            /// </summary>
            Random = -1,

            /// <summary>
            /// Default Server Season
            /// </summary>
            DefaultSeason = -2,

            /// <summary>
            /// Custom Server Season
            /// </summary>
            Custom = -3,

            /// <summary>
            /// Nothing (Only used in command)
            /// </summary>
            Nothing = -4,
        }

        /// <summary>
        /// A collection of Weather Type
        /// </summary>
        public enum WeatherType
        {
            /// <summary>
            /// Clear Weather
            /// </summary>
            Clear = 0,

            /// <summary>
            /// Rain Weather
            /// </summary>
            Rain = 1,

            /// <summary>
            /// Snow Weather
            /// </summary>
            Snow = 2,

            /// <summary>
            /// Underwater Weather
            /// </summary>
            Underwater = 3,

            /// <summary>
            /// Sunny Weather
            /// </summary>
            Sunny = 4,

            /// <summary>
            /// Fog Weather
            /// </summary>
            Fog = 5,

            /// <summary>
            /// Thunderstorm Weather
            /// </summary>
            Thunderstorm = 6,

            /// <summary>
            /// Sandstorm Weather
            /// </summary>
            Sandstorm = 7,

            /// <summary>
            /// Ash Weather
            /// </summary>
            Ash = 8,

            /// <summary>
            /// Blizzard Weather
            /// </summary>
            Blizzard = 9,

            /// <summary>
            /// Random Weather
            /// </summary>
            Random = -1,

            /// <summary>
            /// Default Server Weather
            /// </summary>
            DefaultWeather = -2,

            /// <summary>
            /// Custom Server Weather
            /// </summary>
            Custom = -3,

            /// <summary>
            /// Real World Weather
            /// </summary>
            Real = -4,

            /// <summary>
            /// Nothing (used in server command)
            /// </summary>
            Nothing = -5,
        }

        /// <summary>
        /// Class containing Advanced Season
        /// </summary>
        public class SeasonMonth
        {
            /* Advanced World Property
            <!-- SeasonMonth set the season based on real world date -->
            <!-- Syntax (Index of season on each month separated by "|"): Jan | Feb | March | April | May | June | July | Aug | Sep | Oct | Nov | Dec -->
            <!-- Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2 -->
            <!-- Each Month cannot have an empty season Or the program might fail to process it. -->
            */
            /// <summary>
            /// Season Data
            /// </summary>
            public static string SeasonData { get; set; }
        }
    }
}
