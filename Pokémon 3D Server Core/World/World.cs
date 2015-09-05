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
        private int _Season;
        /// <summary>
        /// Get/Set Current World Season
        /// </summary>
        public int Season
        {
            get
            {
                return _Season;
            }
            set
            {
                if (value < 0)
                {
                    _Season = 0;
                }
                else if (value > 3)
                {
                    _Season = 3;
                }
                else
                {
                    _Season = value;
                }
            }
        }

        private int _Weather;
        /// <summary>
        /// Get/Set Current World Weather
        /// </summary>
        public int Weather
        {
            get
            {
                return _Weather;
            }
            set
            {
                if (value < 0)
                {
                    _Weather = 0;
                }
                else if (value > 9)
                {
                    _Weather = 9;
                }
                else
                {
                    _Weather = value;
                }
            }
        }

        private DateTime _CurrentTime;
        /// <summary>
        /// Get/Set Current World Time
        /// </summary>
        public string CurrentTime
        {
            get
            {
                return _CurrentTime.Hour + "," + _CurrentTime.Minute + "," + _CurrentTime.Second;
            }
            set
            {
                try
                {
                    _CurrentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, value.GetSplit(0, ",").Toint(), value.GetSplit(1, ",").Toint(), value.GetSplit(2, ",").Toint());
                }
                catch (Exception)
                {
                    _CurrentTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Get/Set Current World Time Offset
        /// </summary>
        public int TimeOffset { get; set; }

        /// <summary>
        /// Get/Set Current World Can Update?
        /// </summary>
        public bool CanUpdate { get; set; }

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

        
    }
}
