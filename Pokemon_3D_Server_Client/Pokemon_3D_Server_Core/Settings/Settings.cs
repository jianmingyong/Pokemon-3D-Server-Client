using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using System.Reflection;
using System.Diagnostics;

namespace Global
{
    /// <summary>
    /// Class containing Settings
    /// </summary>
    public class Settings
    {
        #region Property
        #region Pokémon 3D Server Client Setting File
        /// <summary>
        /// Get/Set Application Directory
        /// </summary>
        public static string ApplicationDirectory { get; set; }

        /// <summary>
        /// Get Startup Time.
        /// </summary>
        public static DateTime StartTime { get; set; }

        /// <summary>
        /// Get Application Version.
        /// </summary>
        public static string ApplicationVersion { get { return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion; } }

        /// <summary>
        /// Get Protocol Version.
        /// </summary>
        public static string ProtocolVersion { get { return "0.5"; } }

        /// <summary>
        /// Get/Set Check For Update.
        /// </summary>
        public static bool CheckForUpdate { get; set; } = true;

        /// <summary>
        /// Get/Set Generate Public IP.
        /// </summary>
        public static bool GeneratePublicIP { get; set; } = true;
        #endregion Pokémon 3D Server Client Setting File

        #region Main Server Property
        /// <summary>
        /// Get/Set IP Address
        /// </summary>
        public static IPAddress _IPAddress = System.Net.IPAddress.Parse(Functions.GetPublicIP());
        /// <summary>
        /// Get/Set IP Address
        /// </summary>
        public static string IPAddress
        {
            get
            {
                return _IPAddress.ToString();
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _IPAddress = System.Net.IPAddress.Parse(Functions.GetPublicIP());
                }
                else
                {
                    _IPAddress = System.Net.IPAddress.Parse(value);
                }
            }
        }

        private static int _Port = 15124;
        /// <summary>
        /// Get/Set Port
        /// </summary>
        public static int Port
        {
            get
            {
                return _Port;
            }
            set
            {
                if (value < 0)
                {
                    _Port = 0;
                }
                else if (value > 65535)
                {
                    _Port = 65535;
                }
                else
                {
                    _Port = value;
                }
            }
        }

        /// <summary>
        /// Get/Set Server Name
        /// </summary>
        public static string ServerName { get; set; } = "P3D Server";

        /// <summary>
        /// Get/Set Server Message
        /// </summary>
        public static string ServerMessage { get; set; } = "";

        /// <summary>
        /// Get/Set Welcome Message
        /// </summary>
        public static string WelcomeMessage { get; set; } = "";

        /// <summary>
        /// Get/Set GameMode
        /// </summary>
        public static List<string> GameMode { get; set; } = new List<string> { "Pokemon 3D" };

        private static int _MaxPlayers = 20;
        /// <summary>
        /// Get/Set Max Players
        /// </summary>
        public static int MaxPlayers
        {
            get
            {
                return _MaxPlayers;
            }
            set
            {
                if (value <= 0)
                {
                    _MaxPlayers = int.MaxValue;
                }
                else
                {
                    _MaxPlayers = value;
                }
            }
        }

        /// <summary>
        /// Get/Set Offline Mode
        /// </summary>
        public static bool OfflineMode { get; set; } = false;
        #endregion Main Server Property

        #region Advanced Server Property
        #region World
        private static int _Season = -2;
        /// <summary>
        /// Get/Set Season
        /// </summary>
        public static int Season
        {
            get
            {
                return _Season;
            }
            set
            {
                if (value <= -4)
                {
                    _Season = -2;
                }
                else if (value > 3)
                {
                    _Season = -2;
                }
                else
                {
                    _Season = value;
                }
            }
        }

        private static int _Weather = -2;
        /// <summary>
        /// Get/Set Weather
        /// </summary>
        public static int Weather
        {
            get
            {
                return _Weather;
            }
            set
            {
                if (value <= -5)
                {
                    _Weather = -2;
                }
                else if (value > 9)
                {
                    _Weather = -2;
                }
                else
                {
                    _Weather = value;
                }
            }
        }

        /// <summary>
        /// Get/Set Do DayCycle
        /// </summary>
        public static bool DoDayCycle { get; set; } = true;

        /// <summary>
        /// Get/Set SeasonMonth
        /// </summary>
        public static SeasonMonth SeasonMonth { get; set; } = new SeasonMonth("-2|-2|-2|-2|-2|-2|-2|-2|-2|-2|-2|-2");

        /// <summary>
        /// Get/Set WeatherSeason
        /// </summary>
        public static WeatherSeason WeatherSeason { get; set; } = new WeatherSeason("-2|-2|-2|-2");

        /// <summary>
        /// Get/Set Default World Country
        /// </summary>
        public static string DefaultWorldCountry { get; set; } = RegionInfo.CurrentRegion.EnglishName;
        #endregion World

        #region FailSafe Features
        private static int _NoPingKickTime = 60;
        /// <summary>
        /// Get/Set No Ping Kick Time
        /// </summary>
        public static int NoPingKickTime
        {
            get
            {
                return _NoPingKickTime;
            }
            set
            {
                if (value < 10)
                {
                    _NoPingKickTime = -1;
                }
                else
                {
                    _NoPingKickTime = value;
                }
            }
        }

        private static int _AFKKickTime = 300;
        /// <summary>
        /// Get/Set AFK Kick Time
        /// </summary>
        public static int AFKKickTime
        {
            get
            {
                return _AFKKickTime;
            }
            set
            {
                if (value < 10)
                {
                    _AFKKickTime = -1;
                }
                else
                {
                    _AFKKickTime = value;
                }
            }
        }

        private static int _AutoRestartTime = -1;
        /// <summary>
        /// Get/Set Auto Restart Time
        /// </summary>
        public static int AutoRestartTime
        {
            get
            {
                return _AutoRestartTime;
            }
            set
            {
                if (value < 10)
                {
                    _AutoRestartTime = -1;
                }
                else
                {
                    _AutoRestartTime = value;
                }
            }
        }
        #endregion FailSafe Features

        #region Features
        /// <summary>
        /// Get/Set BlackList Feature
        /// </summary>
        public static bool BlackList { get; set; } = true;
        /// <summary>
        /// Get/Set BlackList Data
        /// </summary>
        public static List<BlackList> BlackListData { get; set; } = new List<BlackList>();

        /// <summary>
        /// Get/Set IPBlackList Feature
        /// </summary>
        public static bool IPBlackList { get; set; } = true;
        /// <summary>
        /// Get/Set IPBlackList Data
        /// </summary>
        public static List<IPBlackList> IPBlackListData { get; set; } = new List<IPBlackList>();

        /// <summary>
        /// Get/Set WhiteList Feature
        /// </summary>
        public static bool WhiteList { get; set; } = false;
        /// <summary>
        /// Get/Set WhiteList Data
        /// </summary>
        public static List<WhiteList> WhiteListData { get; set; } = new List<WhiteList>();

        /// <summary>
        /// Get/Set OperatorList Feature
        /// </summary>
        public static bool OperatorList { get; set; } = true;
        /// <summary>
        /// Get/Set OperatorList Data
        /// </summary>
        public static List<OperatorList> OperatorListData { get; set; } = new List<OperatorList>();

        /// <summary>
        /// Get/Set MuteList Feature
        /// </summary>
        public static bool MuteList { get; set; } = true;
        /// <summary>
        /// Get/Set MuteList Data
        /// </summary>
        public static List<MuteList> MuteListData { get; set; } = new List<MuteList>();

        /// <summary>
        /// Get/Set OnlineSettingList Feature
        /// </summary>
        public static bool OnlineSettingList { get; set; } = true;
        /// <summary>
        /// Get/Set OnlineSettingList Data
        /// </summary>
        public static List<OnlineSetting> OnlineSettingListData { get; set; } = new List<OnlineSetting>();

        /// <summary>
        /// Get/Set SwearInfractionList Feature
        /// </summary>
        public static bool SwearInfractionList { get; set; } = false;
        /// <summary>
        /// Get/Set SwearInfractionList Data
        /// </summary>
        public static List<SwearInfractionList> SwearInfractionListData { get; set; } = new List<SwearInfractionList>();

        #region Swear Infraction Feature
        /// <summary>
        /// Get/Set SwearInfraction Filter List
        /// </summary>
        public static List<SwearInfractionFilterList> SwearInfractionFilterListData { get; set; } = new List<SwearInfractionFilterList>();

        private static int _SwearInfractionCap = 5;
        /// <summary>
        /// Get/Set SwearInfraction Cap
        /// </summary>
        public static int SwearInfractionCap
        {
            get
            {
                return _SwearInfractionCap;
            }
            set
            {
                if (value < 1)
                {
                    _SwearInfractionCap = -1;
                }
                else
                {
                    _SwearInfractionCap = value;
                }
            }
        }

        private static int _SwearInfractionReset = 1;
        /// <summary>
        /// Get/Set SwearInfraction Reset time
        /// </summary>
        public static int SwearInfractionReset
        {
            get
            {
                return _SwearInfractionReset;
            }
            set
            {
                if (value < 1)
                {
                    _SwearInfractionReset = -1;
                }
                else
                {
                    _SwearInfractionReset = value;
                }
            }
        }
        #endregion Swear Infraction Feature

        #region Spam Detection
        private static int _SpamResetDuration = 30;
        /// <summary>
        /// Get/Set Spam Reset Duration
        /// </summary>
        public static int SpamResetDuration
        {
            get
            {
                return _SpamResetDuration;
            }
            set
            {
                if (value < 1)
                {
                    _SpamResetDuration = -1;
                }
                else
                {
                    _SpamResetDuration = value;
                }
            }
        }
        #endregion Spam Detection
        #endregion Features
        #endregion Advanced Server Property

        #region Server Client Logger
        /// <summary>
        /// Get/Set Logger Info Message
        /// </summary>
        public static bool LoggerInfo { get; set; } = true;

        /// <summary>
        /// Get/Set Logger Warning Message
        /// </summary>
        public static bool LoggerWarning { get; set; } = true;

        /// <summary>
        /// Get/Set Logger Debugger Message
        /// </summary>
        public static bool LoggerDebug { get; set; } = false;

        /// <summary>
        /// Get/Set Logger Chat Message
        /// </summary>
        public static bool LoggerChat { get; set; } = true;

        /// <summary>
        /// Get/Set Logger Private Message
        /// </summary>
        public static bool LoggerPM { get; set; } = true;

        /// <summary>
        /// Get/Set Logger Server Message
        /// </summary>
        public static bool LoggerServer { get; set; } = true;

        /// <summary>
        /// Get/Set Logger Trade Message
        /// </summary>
        public static bool LoggerTrade { get; set; } = true;

        /// <summary>
        /// Get/Set Logger PvP Message
        /// </summary>
        public static bool LoggerPvP { get; set; } = true;

        /// <summary>
        /// Get/Set Logger Command Message
        /// </summary>
        public static bool LoggerCommand { get; set; } = true;
        #endregion Server Client Logger

        #region Token
        /// <summary>
        /// Get/Set Token Defination
        /// </summary>
        public static Dictionary<string, string> TokenDefination { get; set; } = new Dictionary<string, string>();
        #endregion Token

        #region MapFile
        /// <summary>
        /// Get/Set Map File List Data
        /// </summary>
        public static List<MapFileList> MapFileListData { get; set; } = new List<MapFileList>();
        #endregion MapFile
        #endregion Property

        /// <summary>
        /// Setup Settings for first launch
        /// </summary>
        /// <param name="Application">Application Directory</param>
        public static void SetUp(string Application)
        {
            ApplicationDirectory = Application;
            StartTime = DateTime.Now;
            QueueMessage.Add("Setting.cs: Setting initiated.", MessageEventArgs.LogType.Info);
        }

        /// <summary>
        /// Load Setting File
        /// </summary>
        public static void Load()
        {
            QueueMessage.Add("Setting.cs: Load Setting.", MessageEventArgs.LogType.Info);

            try
            {
                #region application_settings.json

                #endregion application_settings.json
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }

        /// <summary>
        /// Save Setting File
        /// </summary>
        public static void Save()
        {
            try
            {
                if (!Directory.Exists(ApplicationDirectory + @"\Data"))
                {
                    Directory.CreateDirectory(ApplicationDirectory + @"\Data");
                }

                #region application_settings.json
                File.WriteAllText(ApplicationDirectory + @"\application_settings.json",
                    string.Format(@"{{

    /* Warning: The syntax for each setting is case sensitive.
       String: ""Text inside a quote""
       Integer: 0123456789
       Boolean: true
    */

    ""Pokémon 3D Server Client Setting File"":
    {{
        ""StartTime"": ""{0}"",
        ""ApplicationVersion"": ""{1}"",
        ""ProtocolVersion"": ""{2}"",

        /* CheckForUpdate:  To allow application to check for update upon launch.
           Syntax: Boolean: true, false */
        ""CheckForUpdate"": {3},

        /* GeneratePublicIP:  To allow application to update IP address upon launch.
		   Syntax: Boolean: true, false */
        ""GeneratePublicIP"": {4}
    }},

    ""Main Server Property"":
    {{
        /* IPAddress:  Public IP address of your server.
		   Syntax: Valid IPv4 address. */
        ""IPAddress"": ""{5}"",

        /* Port:  The port to use on your server.
		   Syntax: Integer: Between 0 to 65535 inclusive. */
        ""Port"": {6},

        /* ServerName:  The server name to display to public.
		   Syntax: String */
        ""ServerName"": ""{7}"",

        /* ServerMessage:  The server message to display when a player select a server.
		   Syntax: String: null for blank. */
        ""ServerMessage"": {8},

        /* WelcomeMessage:  The server message to display when a player joins a server.
		   Syntax: String: null for blank. */
        ""WelcomeMessage"": {9},

        /* GameMode:  The GameMode that player should play in order to join the server.
		   Syntax: String. You may insert multiple gamemode by adding a comma seperator on each gamemode name. */
        ""GameMode"": ""{10}"",

        /* MaxPlayers:  The maximum amount of player in the server that can join.
		   Syntax: Integer: -1: Unlimited. */
        ""MaxPlayers"": {11},

        /* OfflineMode:  The ability for offline profile player to join the server.
		   Syntax: Boolean: true, false */
        ""OfflineMode"": {12}
    }},

    ""Advanced Server Property"":
    {{
        ""World"":
        {{
            /* Season:  To set server default season.
			    Syntax: Integer: Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2 | SeasonMonth = -3 */
            ""Season"": {13},

            /* Weather:  To set server default weather.
			    Syntax: Integer: Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 | WeatherSeason = -3 | Real World Weather = -4 */
            ""Weather"": {14},

            /* DoDayCycle:  To allow the server to update day cycle.
			    Syntax: Boolean: true, false */
			""DoDayCycle"": {15},

            /* SeasonMonth:  To set the season based on local date. Must set Season = -3
			    Syntax: Integer: Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2
			    You may insert more than one season by separating it with a comma. */
            ""SeasonMonth"":
            {{
                ""January"": ""{16}"",
                ""February"": ""{17}"",
                ""March"": ""{18}"",
                ""April"": ""{19}"",
                ""May"": ""{20}"",
                ""June"": ""{21}"",
                ""July"": ""{22}"",
                ""August"": ""{23}"",
                ""September"": ""{24}"",
                ""October"": ""{25}"",
                ""November"": ""{26}"",
                ""December"": ""{27}""
            }},

            /* WeatherSeason:  To set the weather based on server season. Must set Weather = -3
			    Syntax: Integer: Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 | Real World Weather = -4
			    You may insert more than one weather by separating it with a comma. */
            ""WeatherSeason"":
            {{
                ""Winter"": ""{28}"",
                ""Spring"": ""{29}"",
                ""Summer"": ""{30}"",
                ""Fall"": ""{31}""
            }},

            /* DefaultWorldCountry:  To set the default country for real world weather.
                Syntax: String. Valid Country name / City name. No fancy character. Use Default A-Z a-z letter. */
            ""DefaultWorldCountry"": ""{32}""
        }},

        ""FailSafe Features"":
        {{
            /* NoPingKickTime:  To kick player out if there are no valid ping for n amount of seconds.
			    Syntax: Integer: -1 to disable it. */
            ""NoPingKickTime"": {33},

            /* AFKKickTime:  To kick player out if there are no valid activity for n amount of seconds.
			    Syntax: Integer: -1 to disable it. */
            ""AFKKickTime"": {34},
        
            /* AutoRestartTime:  To automatically restart the server after n seconds. Disable PvP and trade features for the last 5 minutes of the countdown.
			    Syntax: Integer: -1 to disable it. */
            ""AutoRestartTime"": {35}
        }},

        ""Features"":
        {{
            /* BlackList:  To allow using blacklist feature.
			    Syntax: Boolean: true, false */
            ""BlackList"": {36},

            /* IPBlackList:  To allow using ipblacklist feature.
			    Syntax: Boolean: true, false */
            ""IPBlackList"": {37},

            /* WhiteList:  To allow using whitelist feature.
			    Syntax: Boolean: true, false */
            ""WhiteList"": {38},

            /* OperatorList:  To allow using operator feature.
			    Syntax: Boolean: true, false */
            ""OperatorList"": {39},

            /* MuteList:  To allow using mute feature.
			    Syntax: Boolean: true, false */
            ""MuteList"": {40},

            /* OnlineSettingList:  To allow using mute feature.
			    Syntax: Boolean: true, false */
            ""OnlineSettingList"": {41},

            /* SwearInfractionList:  To allow using swear infraction feature.
			    Syntax: Boolean: true, false */
            ""SwearInfractionList"": {42},

            ""Swear Infraction Feature"":
            {{
                /* SwearInfractionCap:  Amount of infraction points before the first mute.
				    Syntax: Integer: -1 to disable. */
                ""SwearInfractionCap"": {43},

                /* SwearInfractionReset:  Amount of days before it expire the infraction count.
				    Syntax: Integer: -1 to disable. */
                ""SwearInfractionReset"": {44}
            }},

            ""Spam Feature"":
            {{
                /* SpamResetDuration:  Amount of seconds for the user to send the same word again.
				    Syntax: Integer: -1 to disable. */
                ""SpamResetDuration"": {45}
            }}
        }}
    }},

    ""Server Client Logger"":
    {{
        /* LoggerInfo:  To log server information.
		   Syntax: Boolean: true, false */
        ""LoggerInfo"": {46},

        /* LoggerWarning:  To log server warning including ex exception.
		   Syntax: Boolean: true, false */
        ""LoggerWarning"": {47},

        /* LoggerDebug:  To log server package data (Lag might happen if turn on).
		   Syntax: Boolean: true, false */
        ""LoggerDebug"": {48},

        /* LoggerChat:  To log server chat message.
		   Syntax: Boolean: true, false */
        ""LoggerChat"": {49},

        /* LoggerPM:  To log server private chat message. (Actual Private Message content is not logged)
		   Syntax: Boolean: true, false */
        ""LoggerPM"": {50},

        /* LoggerServer:  To log server message.
		   Syntax: Boolean: true, false */
        ""LoggerServer"": {51},

        /* LoggerTrade:  To log trade request. (Actual Trade Request content is not logged)
		   Syntax: Boolean: true, false */
        ""LoggerTrade"": {52},

        /* LoggerPvP:  To log pvp request. (Actual PvP Request content is not logged)
		   Syntax: Boolean: true, false */
        ""LoggerPvP"": {53},

        /* LoggerCommand:  To log server command usage. (Debug Commands are not logged)
		   Syntax: Boolean: true, false */
        ""LoggerCommand"": {54}
    }}
}}",
StartTime.ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffK"), // StartTime
ApplicationVersion, // ApplicationVersion
ProtocolVersion, // ProtocolVersion
CheckForUpdate.ToString().ToLower(), // CheckForUpdate
GeneratePublicIP.ToString().ToLower(), // GeneratePublicIP
IPAddress, // IPAddress
Port.ToString(), // Port
ServerName, // ServerName
string.IsNullOrWhiteSpace(ServerMessage) ? "null" : @"""" + ServerMessage + @"""", // ServerMessage
string.IsNullOrWhiteSpace(WelcomeMessage) ? "null" : @"""" + WelcomeMessage + @"""", // WelcomeMessage
GameMode[0].ToString(), // GameMode
MaxPlayers.ToString(), // MaxPlayers
OfflineMode.ToString().ToLower(), // OfflineMode
Season.ToString(), // Season
Weather.ToString(), // Weather
DoDayCycle.ToString().ToLower(), // DoDayCycle
SeasonMonth.SeasonData.GetSplit(0), // SeasonMonth
SeasonMonth.SeasonData.GetSplit(1), // SeasonMonth
SeasonMonth.SeasonData.GetSplit(2), // SeasonMonth
SeasonMonth.SeasonData.GetSplit(3), // SeasonMonth
SeasonMonth.SeasonData.GetSplit(4), // SeasonMonth
SeasonMonth.SeasonData.GetSplit(5), // SeasonMonth
SeasonMonth.SeasonData.GetSplit(6), // SeasonMonth
SeasonMonth.SeasonData.GetSplit(7), // SeasonMonth
SeasonMonth.SeasonData.GetSplit(8), // SeasonMonth
SeasonMonth.SeasonData.GetSplit(9), // SeasonMonth
SeasonMonth.SeasonData.GetSplit(10), // SeasonMonth
SeasonMonth.SeasonData.GetSplit(11), // SeasonMonth
WeatherSeason.WeatherData.GetSplit(0), // WeatherSeason
WeatherSeason.WeatherData.GetSplit(1), // WeatherSeason
WeatherSeason.WeatherData.GetSplit(2), // WeatherSeason
WeatherSeason.WeatherData.GetSplit(3), // WeatherSeason
DefaultWorldCountry, // DefaultWorldCountry
NoPingKickTime.ToString(), // NoPingKickTime
AFKKickTime.ToString(), // AFKKickTime
AutoRestartTime.ToString(), // AutoRestartTime
BlackList.ToString().ToLower(), // BlackList
IPBlackList.ToString().ToLower(), // IPBlackList
WhiteList.ToString().ToLower(), // WhiteList
OperatorList.ToString().ToLower(), // OperatorList
MuteList.ToString().ToLower(), // MuteList
OnlineSettingList.ToString().ToLower(), // OnlineSettingList
SwearInfractionList.ToString().ToLower(), // SwearInfractionList
SwearInfractionCap.ToString(), // SwearInfractionCap
SwearInfractionReset.ToString(), // SwearInfractionReset
SpamResetDuration.ToString(), // SpamResetDuration
LoggerInfo.ToString().ToLower(), // LoggerInfo
LoggerWarning.ToString().ToLower(), // LoggerWarning
LoggerDebug.ToString().ToLower(), // LoggerDebug
LoggerChat.ToString().ToLower(), // LoggerChat
LoggerPM.ToString().ToLower(), // LoggerPM
LoggerServer.ToString().ToLower(), // LoggerServer
LoggerTrade.ToString().ToLower(), // LoggerTrade
LoggerPvP.ToString().ToLower(), // LoggerPvP
LoggerCommand.ToString().ToLower() // LoggerCommand
), Encoding.Unicode);
                #endregion application_settings.json

                QueueMessage.Add("Setting.cs: Saved Setting.", MessageEventArgs.LogType.Info);
        }
            catch (Exception ex)
            {
                ex.CatchError();
                QueueMessage.Add("Setting.cs: Save Setting failed.", MessageEventArgs.LogType.Info);
            }
}

        /// <summary>
        /// Check for setting file.
        /// </summary>
        /// <param name="Files">File Name with extension.</param>
        public static bool HaveSettingFile(string Files)
        {
            try
            {
                if (File.Exists(ApplicationDirectory + @"\" + Files))
                {
                    if (string.IsNullOrWhiteSpace(File.ReadAllText(ApplicationDirectory + @"\" + Files)))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
            return false;
        }

        /// <summary>
        /// Get the token string from key.
        /// </summary>
        /// <param name="Key">The key of the token.</param>
        /// <param name="Variable">The Variable of the token.</param>
        /// <returns></returns>
        public static string Token(string Key,params string[] Variable)
        {
            string ReturnValue = null;

            if (TokenDefination.ContainsKey(Key))
            {
                ReturnValue = TokenDefination[Key];

                for (int i = 0; i < Variable.Count(); i++)
                {
                    ReturnValue = ReturnValue.Replace("{" + i + "}", Variable[i]);
                }
            }

            return ReturnValue;
        }
    }
}
