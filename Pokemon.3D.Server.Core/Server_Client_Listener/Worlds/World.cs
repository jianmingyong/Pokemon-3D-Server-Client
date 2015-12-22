using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Worlds
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
                _Season = value.RollOver(0, 3);

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
                _Weather = value.RollOver(0, 9);
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
                    _CurrentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, value.GetSplit(0, ",").ToInt(), value.GetSplit(1, ",").ToInt(), value.GetSplit(2, ",").ToInt());
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
        public int TimeOffset { get; set; } = 0;

        private DateTime LastWorldUpdate { get; set; }

        private int WeekOfYear { get { return (int)(DateTime.Now.DayOfYear - (DateTime.Now.DayOfWeek - DayOfWeek.Monday) / 7.0 + 1.0); } }

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
        /// New World
        /// </summary>
        public World()
        {
            _CurrentTime = DateTime.Now;
        }

        /// <summary>
        /// Update World
        /// </summary>
        public void Update()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            do
            {
                try
                {
                    _CurrentTime = DateTime.Now;

                    if (LastWorldUpdate == null || LastWorldUpdate.AddHours(1) <= DateTime.Now)
                    {
                        switch (Core.Setting.Season)
                        {
                            case (int)SeasonType.DefaultSeason:
                                switch (WeekOfYear % 4)
                                {
                                    case 0:
                                        Season = (int)SeasonType.Fall;
                                        break;
                                    case 1:
                                        Season = (int)SeasonType.Winter;
                                        break;
                                    case 2:
                                        Season = (int)SeasonType.Spring;
                                        break;
                                    case 3:
                                        Season = (int)SeasonType.Summer;
                                        break;
                                    default:
                                        Season = (int)SeasonType.Summer;
                                        break;
                                }
                                break;
                            case (int)SeasonType.Random:
                                Season = MathHelper.Random(0, 3);
                                break;
                            case (int)SeasonType.Custom:
                                Season = GetCustomSeason();
                                break;
                            default:
                                Season = Core.Setting.Season;
                                break;
                        }

                        switch (Core.Setting.Weather)
                        {
                            case (int)WeatherType.DefaultWeather:
                                int Random = MathHelper.Random(1, 100);
                                switch (Season)
                                {
                                    case (int)SeasonType.Winter:
                                        if (Random > 50)
                                        {
                                            Weather = (int)WeatherType.Snow;
                                        }
                                        else if (Random > 20)
                                        {
                                            Weather = (int)WeatherType.Clear;
                                        }
                                        else
                                        {
                                            Weather = (int)WeatherType.Rain;
                                        }
                                        break;
                                    case (int)SeasonType.Spring:
                                        if (Random > 40)
                                        {
                                            Weather = (int)WeatherType.Clear;
                                        }
                                        else if (Random > 5)
                                        {
                                            Weather = (int)WeatherType.Rain;
                                        }
                                        else
                                        {
                                            Weather = (int)WeatherType.Snow;
                                        }
                                        break;
                                    case (int)SeasonType.Summer:
                                        if (Random > 10)
                                        {
                                            Weather = (int)WeatherType.Clear;
                                        }
                                        else
                                        {
                                            Weather = (int)WeatherType.Rain;
                                        }
                                        break;
                                    case (int)SeasonType.Fall:
                                        if (Random > 80)
                                        {
                                            Weather = (int)WeatherType.Clear;
                                        }
                                        else if (Random > 5)
                                        {
                                            Weather = (int)WeatherType.Rain;
                                        }
                                        else
                                        {
                                            Weather = (int)WeatherType.Snow;
                                        }
                                        break;
                                    default:
                                        Weather = (int)WeatherType.Clear;
                                        break;
                                }
                                break;
                            case (int)WeatherType.Random:
                                Weather = MathHelper.Random(0, 9);
                                break;
                            case (int)WeatherType.Custom:
                                Weather = GetCustomWeather();
                                break;
                            default:
                                Weather = Core.Setting.Weather;
                                break;
                        }

                        TimeOffset = Core.Setting.TimeOffset;

                        LastWorldUpdate = DateTime.Now;
                        Core.Logger.Log(string.Format(@"Current Season: {0} | Current Weather: {1} | Current Time: {2}", GetSeasonName(Season), GetWeatherName(Weather), _CurrentTime.AddSeconds(TimeOffset).ToString()), Logger.LogTypes.Info);
                    }

                    for (int i = 0; i < Core.Player.Count; i++)
                    {
                        if (Core.Player[i].Network.IsActive)
                        {
                            Core.Player.SentToPlayer(new Package(Package.PackageTypes.WorldData, GenerateWorld(Core.Player[i]), Core.Player[i].Network.Client));
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    ex.CatchError();
                }

                sw.Stop();
                if (sw.ElapsedMilliseconds < 1000)
                {
                    Thread.Sleep(1000 - sw.ElapsedMilliseconds.ToString().ToInt());
                }
                sw.Restart();
            }
            while (true);
        }

        /// <summary>
        /// Generate World Data
        /// </summary>
        /// <param name="Player">Player to Generate.</param>
        public List<string> GenerateWorld(Player Player)
        {
            try
            {
                List<string> ReturnList = new List<string>();

                if (Core.Setting.DoDayCycle)
                {
                    CurrentTime = _CurrentTime.AddSeconds(TimeOffset).Hour.ToString() + "," + _CurrentTime.AddSeconds(TimeOffset).Minute.ToString() + "," + _CurrentTime.AddSeconds(TimeOffset).Second.ToString();
                }
                else
                {
                    CurrentTime = "12,0,0";
                }

                if (Player.isGameJoltPlayer)
                {
                    OnlineSetting OnlineSetting = (from OnlineSetting p in Core.Setting.OnlineSettingListData where Player.GameJoltID == p.GameJoltID select p).FirstOrDefault();

                    if (OnlineSetting.LastWorldUpdate == null || OnlineSetting.LastWorldUpdate.AddHours(1) <= DateTime.Now)
                    {
                        OnlineSetting.CurrentWorldSeason = GenerateSeason(OnlineSetting.Season);
                        OnlineSetting.CurrentWorldWeather = GenerateWeather(OnlineSetting.Weather, OnlineSetting.Season);
                        OnlineSetting.LastWorldUpdate = LastWorldUpdate;
                    }

                    ReturnList = new List<string>
                    {
                        OnlineSetting.Season == (int)SeasonType.Nothing ? Season.ToString() : OnlineSetting.CurrentWorldSeason.ToString(),
                        OnlineSetting.Weather == (int)WeatherType.Nothing ? Weather.ToString() : OnlineSetting.CurrentWorldWeather.ToString(),
                        CurrentTime
                    };
                }
                else
                {
                    ReturnList = new List<string>
                    {
                        Season.ToString(),
                        Weather.ToString(),
                        CurrentTime
                    };
                }

                return ReturnList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Generate Season
        /// </summary>
        /// <param name="Season">Proposed Season ID</param>
        public int GenerateSeason(int Season)
        {
            switch (Season)
            {
                case (int)SeasonType.DefaultSeason:
                    switch (WeekOfYear % 4)
                    {
                        case 0:
                            return (int)SeasonType.Fall;
                        case 1:
                            return (int)SeasonType.Winter;
                        case 2:
                            return (int)SeasonType.Spring;
                        case 3:
                            return (int)SeasonType.Summer;
                        default:
                            return (int)SeasonType.Summer;
                    }
                case (int)SeasonType.Random:
                    return MathHelper.Random(0, 3);
                case (int)SeasonType.Custom:
                    return GetCustomSeason();
                case (int)SeasonType.Nothing:
                    return this.Season;
                default:
                    if (Season < 0 && Season > 3)
                    {
                        return (int)SeasonType.Winter;
                    }
                    else
                    {
                        return Season;
                    }
            }
        }

        /// <summary>
        /// Generate Weather
        /// </summary>
        /// <param name="Weather">Proposed Weather ID</param>
        /// <param name="Season">Proposed Season ID</param>
        public int GenerateWeather(int Weather, int Season)
        {
            switch (Weather)
            {
                case (int)WeatherType.DefaultWeather:
                    int Random = MathHelper.Random(1, 100);
                    switch (Season)
                    {
                        case (int)SeasonType.Winter:
                            if (Random > 50)
                            {
                                return (int)WeatherType.Snow;
                            }
                            else if (Random > 20)
                            {
                                return (int)WeatherType.Clear;
                            }
                            else
                            {
                                return (int)WeatherType.Rain;
                            }
                        case (int)SeasonType.Spring:
                            if (Random > 40)
                            {
                                return (int)WeatherType.Clear;
                            }
                            else if (Random > 5)
                            {
                                return (int)WeatherType.Rain;
                            }
                            else
                            {
                                return (int)WeatherType.Snow;
                            }
                        case (int)SeasonType.Summer:
                            if (Random > 10)
                            {
                                return (int)WeatherType.Clear;
                            }
                            else
                            {
                                return (int)WeatherType.Rain;
                            }
                        case (int)SeasonType.Fall:
                            if (Random > 80)
                            {
                                return (int)WeatherType.Clear;
                            }
                            else if (Random > 5)
                            {
                                return (int)WeatherType.Rain;
                            }
                            else
                            {
                                return (int)WeatherType.Snow;
                            }
                        default:
                            return (int)WeatherType.Clear;
                    }
                case (int)WeatherType.Random:
                    return MathHelper.Random(0, 9);
                case (int)WeatherType.Custom:
                    return GetCustomWeather();
                case (int)WeatherType.Nothing:
                    return this.Weather;
                default:
                    if (Weather < 0 && Weather > 9)
                    {
                        return (int)WeatherType.Clear;
                    }
                    else
                    {
                        return Weather;
                    }
            }
        }

        private int GetCustomSeason()
        {
            try
            {
                return Core.Setting.SeasonMonth.SeasonList[MathHelper.Random(0, Core.Setting.SeasonMonth.SeasonList.Count - 1)];
            }
            catch (Exception)
            {
                return (int)SeasonType.Winter;
            }
        }

        private int GetCustomWeather()
        {
            try
            {
                return Core.Setting.WeatherSeason.WeatherList[MathHelper.Random(0, Core.Setting.WeatherSeason.WeatherList.Count - 1)];
            }
            catch (Exception)
            {
                return (int)WeatherType.Clear;
            }
        }

        /// <summary>
        /// Get Season Name
        /// </summary>
        /// <param name="Season">Season ID</param>
        public string GetSeasonName(int Season)
        {
            switch (Season)
            {
                case (int)SeasonType.Winter:
                    return "Winter";
                case (int)SeasonType.Spring:
                    return "Spring";
                case (int)SeasonType.Summer:
                    return "Summer";
                case (int)SeasonType.Fall:
                    return "Fall";
                default:
                    return "Winter";
            }
        }

        /// <summary>
        /// Get Weather Name
        /// </summary>
        /// <param name="Weather">Weather ID</param>
        public string GetWeatherName(int Weather)
        {
            switch (Weather)
            {
                case (int)WeatherType.Ash:
                    return "Ash";
                case (int)WeatherType.Blizzard:
                    return "Blizzard";
                case (int)WeatherType.Clear:
                    return "Clear";
                case (int)WeatherType.Fog:
                    return "Fog";
                case (int)WeatherType.Rain:
                    return "Rain";
                case (int)WeatherType.Sandstorm:
                    return "Sandstorm";
                case (int)WeatherType.Snow:
                    return "Snow";
                case (int)WeatherType.Sunny:
                    return "Sunny";
                case (int)WeatherType.Thunderstorm:
                    return "Thunderstorm";
                case (int)WeatherType.Underwater:
                    return "Underwater";
                default:
                    return "Clear";
            }
        }

        /// <summary>
        /// Get current World
        /// </summary>
        public override string ToString()
        {
            return string.Format(@"Current Season: {0} | Current Weather: {1} | Current Time: {2}", GetSeasonName(Season), GetWeatherName(Weather), _CurrentTime.AddSeconds(TimeOffset).ToString());
        }

        /// <summary>
        /// Get current World with custom season and weather.
        /// </summary>
        /// <param name="Season">Season.</param>
        /// <param name="Weather">Weather.</param>
        public string ToString(int Season, int Weather)
        {
            return string.Format(@"Current Season: {0} | Current Weather: {1} | Current Time: {2}", GetSeasonName(Season), GetWeatherName(Weather), _CurrentTime.AddSeconds(TimeOffset).ToString());
        }
    }
}