using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Modules;
using Pokemon_3D_Server_Core.Players;
using Pokemon_3D_Server_Core.Worlds;

namespace Pokemon_3D_Server_Core.Settings
{
    /// <summary>
    /// Class containing Settings
    /// </summary>
    public class Setting
    {
        #region Property
        #region Pokémon 3D Server Client Setting File
        /// <summary>
        /// Get/Set Application Directory.
        /// </summary>
        public string ApplicationDirectory { get; set; }

        /// <summary>
        /// Get Startup Time.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Get Application Version.
        /// </summary>
        public string ApplicationVersion { get { return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion; } }

        /// <summary>
        /// Get Protocol Version.
        /// </summary>
        public string ProtocolVersion { get { return "0.5"; } }

        /// <summary>
        /// Get/Set Check For Update.
        /// </summary>
        public bool CheckForUpdate { get; set; } = true;

        /// <summary>
        /// Get/Set Generate Public IP.
        /// </summary>
        public bool GeneratePublicIP { get; set; } = true;
        #endregion Pokémon 3D Server Client Setting File

        #region Main Server Property
        /// <summary>
        /// Get/Set IP Address
        /// </summary>
        public IPAddress _IPAddress = System.Net.IPAddress.Parse(Functions.GetPublicIP());
        /// <summary>
        /// Get/Set IP Address
        /// </summary>
        public string IPAddress
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
        public int Port
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
        public string ServerName { get; set; } = "P3D Server";

        /// <summary>
        /// Get/Set Server Message
        /// </summary>
        public string ServerMessage { get; set; } = "";

        /// <summary>
        /// Get/Set Welcome Message
        /// </summary>
        public string WelcomeMessage { get; set; } = "";

        /// <summary>
        /// Get/Set GameMode
        /// </summary>
        public List<string> GameMode { get; set; } = new List<string> { "Pokemon 3D" };

        private int _MaxPlayers = 20;
        /// <summary>
        /// Get/Set Max Players
        /// </summary>
        public int MaxPlayers
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
        public bool OfflineMode { get; set; } = false;
        #endregion Main Server Property

        #region Advanced Server Property
        #region World
        private int _Season = -2;
        /// <summary>
        /// Get/Set Season
        /// </summary>
        public int Season
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

        private int _Weather = -2;
        /// <summary>
        /// Get/Set Weather
        /// </summary>
        public int Weather
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
        public bool DoDayCycle { get; set; } = true;

        /// <summary>
        /// Get/Set SeasonMonth
        /// </summary>
        public SeasonMonth SeasonMonth { get; set; } = new SeasonMonth("-2|-2|-2|-2|-2|-2|-2|-2|-2|-2|-2|-2");

        /// <summary>
        /// Get/Set WeatherSeason
        /// </summary>
        public WeatherSeason WeatherSeason { get; set; } = new WeatherSeason("-2|-2|-2|-2");

        /// <summary>
        /// Get/Set Default World Country
        /// </summary>
        public string DefaultWorldCountry { get; set; } = RegionInfo.CurrentRegion.EnglishName;
        #endregion World

        #region FailSafe Features
        private int _NoPingKickTime = 20;
        /// <summary>
        /// Get/Set No Ping Kick Time
        /// </summary>
        public int NoPingKickTime
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

        private int _AFKKickTime = 300;
        /// <summary>
        /// Get/Set AFK Kick Time
        /// </summary>
        public int AFKKickTime
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

        private int _AutoRestartTime = -1;
        /// <summary>
        /// Get/Set Auto Restart Time
        /// </summary>
        public int AutoRestartTime
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
        public bool BlackList { get; set; } = true;
        /// <summary>
        /// Get/Set BlackList Data
        /// </summary>
        public List<BlackList> BlackListData { get; set; } = new List<BlackList>();

        /// <summary>
        /// Get/Set IPBlackList Feature
        /// </summary>
        public bool IPBlackList { get; set; } = true;
        /// <summary>
        /// Get/Set IPBlackList Data
        /// </summary>
        public List<IPBlackList> IPBlackListData { get; set; } = new List<IPBlackList>();

        /// <summary>
        /// Get/Set WhiteList Feature
        /// </summary>
        public bool WhiteList { get; set; } = false;
        /// <summary>
        /// Get/Set WhiteList Data
        /// </summary>
        public List<WhiteList> WhiteListData { get; set; } = new List<WhiteList>();

        /// <summary>
        /// Get/Set OperatorList Feature
        /// </summary>
        public bool OperatorList { get; set; } = true;
        /// <summary>
        /// Get/Set OperatorList Data
        /// </summary>
        public List<OperatorList> OperatorListData { get; set; } = new List<OperatorList>();

        /// <summary>
        /// Get/Set MuteList Feature
        /// </summary>
        public bool MuteList { get; set; } = true;
        /// <summary>
        /// Get/Set MuteList Data
        /// </summary>
        public List<MuteList> MuteListData { get; set; } = new List<MuteList>();

        /// <summary>
        /// Get/Set OnlineSettingList Feature
        /// </summary>
        public bool OnlineSettingList { get; set; } = true;
        /// <summary>
        /// Get/Set OnlineSettingList Data
        /// </summary>
        public List<OnlineSetting> OnlineSettingListData { get; set; } = new List<OnlineSetting>();

        /// <summary>
        /// Get/Set SwearInfractionList Feature
        /// </summary>
        public bool SwearInfractionList { get; set; } = false;
        /// <summary>
        /// Get/Set SwearInfractionList Data
        /// </summary>
        public List<SwearInfractionList> SwearInfractionListData { get; set; } = new List<SwearInfractionList>();

        #region Swear Infraction Feature
        /// <summary>
        /// Get/Set SwearInfraction Filter List
        /// </summary>
        public List<SwearInfractionFilterList> SwearInfractionFilterListData { get; set; } = new List<SwearInfractionFilterList>();

        private int _SwearInfractionCap = 5;
        /// <summary>
        /// Get/Set SwearInfraction Cap
        /// </summary>
        public int SwearInfractionCap
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

        private int _SwearInfractionReset = 1;
        /// <summary>
        /// Get/Set SwearInfraction Reset time
        /// </summary>
        public int SwearInfractionReset
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
        private int _SpamResetDuration = 30;
        /// <summary>
        /// Get/Set Spam Reset Duration
        /// </summary>
        public int SpamResetDuration
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
        public bool LoggerInfo { get; set; } = true;

        /// <summary>
        /// Get/Set Logger Warning Message
        /// </summary>
        public bool LoggerWarning { get; set; } = true;

        /// <summary>
        /// Get/Set Logger Debugger Message
        /// </summary>
        public bool LoggerDebug { get; set; } = false;

        /// <summary>
        /// Get/Set Logger Chat Message
        /// </summary>
        public bool LoggerChat { get; set; } = true;

        /// <summary>
        /// Get/Set Logger Private Message
        /// </summary>
        public bool LoggerPM { get; set; } = true;

        /// <summary>
        /// Get/Set Logger Server Message
        /// </summary>
        public bool LoggerServer { get; set; } = true;

        /// <summary>
        /// Get/Set Logger Trade Message
        /// </summary>
        public bool LoggerTrade { get; set; } = true;

        /// <summary>
        /// Get/Set Logger PvP Message
        /// </summary>
        public bool LoggerPvP { get; set; } = true;

        /// <summary>
        /// Get/Set Logger Command Message
        /// </summary>
        public bool LoggerCommand { get; set; } = true;
        #endregion Server Client Logger

        #region Token
        /// <summary>
        /// Get/Set Token Defination
        /// </summary>
        public Dictionary<string, string> TokenDefination { get; set; } = new Dictionary<string, string>();
        #endregion Token

        #region MapFile
        /// <summary>
        /// Get/Set Map File List Data
        /// </summary>
        public List<MapFileList> MapFileListData { get; set; } = new List<MapFileList>();
        #endregion MapFile
        #endregion Property

        /// <summary>
        /// New Setting - Setup
        /// </summary>
        public void Setup()
        {
            // Initialize Tokens
            TokenDefination.Add("SERVER_FULL", "This server is currently full of players.");
            TokenDefination.Add("SERVER_WRONGGAMEMODE", "This server require you to play the following gamemode: {0}.");
            TokenDefination.Add("SERVER_BLACKLISTED", "You have been banned from server. Reason: {0} | Ban duration: {1}.");
            TokenDefination.Add("SERVER_IPBLACKLISTED", "You have been ip banned from server. Reason: {0} | Ban duration: {1}.");
            TokenDefination.Add("SERVER_GAMEJOLT", "{0} ({1}) {2}");
            TokenDefination.Add("SERVER_NOGAMEJOLT", "{0} {1}");
            TokenDefination.Add("SERVER_CHATGAMEJOLT", "<{0} ({1})>: {2}");
            TokenDefination.Add("SERVER_CHATNOGAMEJOLT", "<{0}>: {1}");
            TokenDefination.Add("SERVER_PLAYERLEFT", "You have left the server.");
            TokenDefination.Add("SERVER_DISALLOW", "You do not have required permission to join the server. Please try again later.");
            TokenDefination.Add("SERVER_CLONE", "You are still in the server. Please try again later.");
            TokenDefination.Add("SERVER_MUTED", "You have been muted in the server. Reason: {0} | Ban duration: {1}.");
            TokenDefination.Add("SERVER_MUTEDTEMP", "You have been muted by that player. Reason: {0} | Ban duration: {1}.");
            TokenDefination.Add("SERVER_SWEARWARNING", "Please avoid swearing where necessary. Triggered word: {0} | You have {1} infraction point. {2} infraction point will get a timeout.");
            TokenDefination.Add("SERVER_SWEAR", "Please avoid swearing where necessary. Triggered word: {0}");
            TokenDefination.Add("SERVER_SPAM", "Please be unique :) don't send the same message again in quick succession.");
            Core.Logger.Add("Setting.cs: Setting initiated.", Logger.LogTypes.Info);
        }

        /// <summary>
        /// Load Setting File
        /// </summary>
        public bool Load()
        {
            try
            {
                #region application_settings.json
                if (File.Exists(ApplicationDirectory + "\\application_settings.json"))
                {
                    using (JsonTextReader Reader = new JsonTextReader(new StringReader(File.ReadAllText(ApplicationDirectory + "\\application_settings.json"))))
                    {
                        Reader.DateParseHandling = DateParseHandling.DateTime;
                        Reader.FloatParseHandling = FloatParseHandling.Double;

                        int StartObjectDepth = -1;
                        string ObjectPropertyName = null;
                        string PropertyName = null;
                        string TempPropertyName = null;
                        List<string> SeasonMonth = new List<string>();
                        List<string> WeatherSeason = new List<string>();

                        while (Reader.Read())
                        {
                            if (Reader.TokenType == JsonToken.StartObject)
                            {
                                StartObjectDepth++;
                                if (TempPropertyName != null && TempPropertyName != ObjectPropertyName)
                                {
                                    ObjectPropertyName = TempPropertyName;
                                    TempPropertyName = null;
                                }
                            }
                            else if (Reader.TokenType == JsonToken.EndObject)
                            {
                                if (StartObjectDepth == 3 && string.Equals(ObjectPropertyName, "SeasonMonth", StringComparison.OrdinalIgnoreCase))
                                {
                                    string TempValue = null;
                                    foreach (string item in SeasonMonth)
                                    {
                                        TempValue += item + "|";
                                    }
                                    TempValue = TempValue.Remove(TempValue.LastIndexOf("|"));
                                    this.SeasonMonth.SeasonData = TempValue;
                                }
                                else if (StartObjectDepth == 3 && string.Equals(ObjectPropertyName, "WeatherSeason", StringComparison.OrdinalIgnoreCase))
                                {
                                    string TempValue = null;
                                    foreach (string item in WeatherSeason)
                                    {
                                        TempValue += item + "|";
                                    }
                                    TempValue = TempValue.Remove(TempValue.LastIndexOf("|"));
                                    this.WeatherSeason.WeatherData = TempValue;
                                }
                                StartObjectDepth--;
                            }

                            if (Reader.TokenType == JsonToken.PropertyName)
                            {
                                TempPropertyName = Reader.Value.ToString();
                            }
                            else if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                            {
                                PropertyName = TempPropertyName;
                                TempPropertyName = null;
                            }

                            #region Pokémon 3D Server Client Setting File
                            if (StartObjectDepth == 1 && string.Equals(ObjectPropertyName, "Pokémon 3D Server Client Setting File", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "CheckForUpdate", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            CheckForUpdate = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Pokémon 3D Server Client Setting File.CheckForUpdate\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "GeneratePublicIP", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            GeneratePublicIP = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Pokémon 3D Server Client Setting File.GeneratePublicIP\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion Pokémon 3D Server Client Setting File
                            #region Main Server Property
                            else if (StartObjectDepth == 1 && string.Equals(ObjectPropertyName, "Main Server Property", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "IPAddress", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            IPAddress = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Main Server Property.IPAddress\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Port", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            Port = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Main Server Property.Port\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "ServerName", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            ServerName = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Main Server Property.ServerName\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "ServerMessage", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            ServerMessage = Reader.Value.ToString();
                                        }
                                        else if (Reader.TokenType == JsonToken.Null)
                                        {
                                            ServerMessage = null;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Main Server Property.ServerMessage\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "WelcomeMessage", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            WelcomeMessage = Reader.Value.ToString();
                                        }
                                        else if (Reader.TokenType == JsonToken.Null)
                                        {
                                            WelcomeMessage = null;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Main Server Property.WelcomeMessage\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "GameMode", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            for (int i = 0; i < Reader.Value.ToString().SplitCount(); i++)
                                            {
                                                if (!GameMode.Contains(Reader.Value.ToString()))
                                                {
                                                    GameMode.Add(Reader.Value.ToString().GetSplit(i, ","));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Main Server Property.GameMode\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "MaxPlayers", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            MaxPlayers = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Main Server Property.MaxPlayers\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "OfflineMode", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            OfflineMode = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Main Server Property.OfflineMode\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion Main Server Property
                            #region World
                            else if (StartObjectDepth == 2 && string.Equals(ObjectPropertyName, "World", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "Season", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            Season = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.World.Season\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Weather", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            Weather = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.World.Weather\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "DoDayCycle", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            DoDayCycle = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.World.DoDayCycle\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion World
                            #region SeasonMonth
                            else if (StartObjectDepth == 3 && string.Equals(ObjectPropertyName, "SeasonMonth", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "January", StringComparison.OrdinalIgnoreCase) || string.Equals(PropertyName, "February", StringComparison.OrdinalIgnoreCase) || string.Equals(PropertyName, "March", StringComparison.OrdinalIgnoreCase) || string.Equals(PropertyName, "April", StringComparison.OrdinalIgnoreCase) || string.Equals(PropertyName, "May", StringComparison.OrdinalIgnoreCase) || string.Equals(PropertyName, "June", StringComparison.OrdinalIgnoreCase) || string.Equals(PropertyName, "July", StringComparison.OrdinalIgnoreCase) || string.Equals(PropertyName, "August", StringComparison.OrdinalIgnoreCase) || string.Equals(PropertyName, "September", StringComparison.OrdinalIgnoreCase) || string.Equals(PropertyName, "October", StringComparison.OrdinalIgnoreCase) || string.Equals(PropertyName, "November", StringComparison.OrdinalIgnoreCase) || string.Equals(PropertyName, "December", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            SeasonMonth.Add(Reader.Value.ToString());
                                        }
                                        else
                                        {
                                            SeasonMonth.Add("-2");
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.World.SeasonMonth\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion SeasonMonth
                            #region WeatherSeason
                            else if (StartObjectDepth == 3 && string.Equals(ObjectPropertyName, "WeatherSeason", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "Winter", StringComparison.OrdinalIgnoreCase) || string.Equals(PropertyName, "Spring", StringComparison.OrdinalIgnoreCase) || string.Equals(PropertyName, "Summer", StringComparison.OrdinalIgnoreCase) || string.Equals(PropertyName, "Fall", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            WeatherSeason.Add(Reader.Value.ToString());
                                        }
                                        else
                                        {
                                            WeatherSeason.Add("-2");
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.World.WeatherSeason\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion WeatherSeason
                            #region World
                            else if (StartObjectDepth == 2 && string.Equals(ObjectPropertyName, "WeatherSeason", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "DefaultWorldCountry", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            DefaultWorldCountry = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.World.DefaultWorldCountry\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion World
                            #region FailSafe Features
                            else if (StartObjectDepth == 2 && string.Equals(ObjectPropertyName, "FailSafe Features", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "NoPingKickTime", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            NoPingKickTime = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.FailSafe Features.NoPingKickTime\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "AFKKickTime", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            AFKKickTime = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.FailSafe Features.AFKKickTime\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "AutoRestartTime", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            AutoRestartTime = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.FailSafe Features.AutoRestartTime\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion FailSafe Features
                            #region Features
                            else if (StartObjectDepth == 2 && string.Equals(ObjectPropertyName, "Features", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "BlackList", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            BlackList = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.Features.BlackList\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "IPBlackList", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            IPBlackList = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.Features.IPBlackList\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "WhiteList", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            WhiteList = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.Features.WhiteList\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "OperatorList", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            OperatorList = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.Features.OperatorList\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "MuteList", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            MuteList = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.Features.MuteList\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "OnlineSettingList", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            OnlineSettingList = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.Features.OnlineSettingList\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "SwearInfractionList", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            SwearInfractionList = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.Features.SwearInfractionList\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion Features
                            #region Swear Infraction Feature
                            else if (StartObjectDepth == 3 && string.Equals(ObjectPropertyName, "Swear Infraction Feature", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "SwearInfractionCap", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            SwearInfractionCap = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.World.Swear Infraction Feature.SwearInfractionCap\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "SwearInfractionReset", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            SwearInfractionReset = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.World.Swear Infraction Feature.SwearInfractionReset\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion Swear Infraction Feature
                            #region Spam Feature
                            else if (StartObjectDepth == 3 && string.Equals(ObjectPropertyName, "Spam Feature", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "SpamResetDuration", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            SpamResetDuration = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Advanced Server Property.World.Spam Feature.SpamResetDuration\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion Spam Feature
                            #region Server Client Logger
                            else if (StartObjectDepth == 1 && string.Equals(ObjectPropertyName, "Server Client Logger", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "LoggerInfo", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            LoggerInfo = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Server Client Logger.LoggerInfo\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "LoggerWarning", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            LoggerWarning = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Server Client Logger.LoggerWarning\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "LoggerDebug", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            LoggerDebug = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Server Client Logger.LoggerDebug\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "LoggerChat", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            LoggerChat = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Server Client Logger.LoggerChat\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "LoggerPM", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            LoggerPM = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Server Client Logger.LoggerPM\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "LoggerServer", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            LoggerServer = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Server Client Logger.LoggerServer\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "LoggerTrade", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            LoggerTrade = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Server Client Logger.LoggerTrade\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "LoggerPvP", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            LoggerPvP = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Server Client Logger.LoggerPvP\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "LoggerCommand", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            LoggerCommand = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Server Client Logger.LoggerCommand\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion Server Client Logger
                        }
                    }
                }
                else
                {
                    return false;
                }
                #endregion application_settings.json

                #region Data\BlackList.json
                if (File.Exists(ApplicationDirectory + "\\Data\\BlackList.json"))
                {
                    using (JsonTextReader Reader = new JsonTextReader(new StringReader(File.ReadAllText(ApplicationDirectory + "\\Data\\BlackList.json"))))
                    {
                        Reader.DateParseHandling = DateParseHandling.DateTime;
                        Reader.FloatParseHandling = FloatParseHandling.Double;

                        int StartObjectDepth = -1;
                        string PropertyName = null;
                        string TempPropertyName = null;

                        string Name = null;
                        int GameJoltID = -1;
                        string Reason = null;
                        DateTime StartTime = DateTime.Now;
                        int Duration = -1;

                        while (Reader.Read())
                        {
                            if (Reader.TokenType == JsonToken.StartObject)
                            {
                                StartObjectDepth++;
                            }
                            else if (Reader.TokenType == JsonToken.EndObject)
                            {
                                if (StartObjectDepth == 1)
                                {
                                    BlackListData.Add(new BlackList(Name, GameJoltID, Reason, StartTime, Duration));
                                    Name = null;
                                    GameJoltID = -1;
                                    Reason = null;
                                    StartTime = DateTime.Now;
                                    Duration = -1;
                                }
                                StartObjectDepth--;
                            }

                            if (Reader.TokenType == JsonToken.PropertyName)
                            {
                                TempPropertyName = Reader.Value.ToString();
                            }
                            else if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                            {
                                PropertyName = TempPropertyName;
                                TempPropertyName = null;
                            }

                            if (StartObjectDepth == 1)
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "Name", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            Name = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"BlackList.Name\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            GameJoltID = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"BlackList.GameJoltID\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Reason", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            Reason = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"BlackList.Reason\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "StartTime", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Date)
                                        {
                                            StartTime = (DateTime)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"BlackList.StartTime\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Duration", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            Duration = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"BlackList.Duration\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion Data\BlackList.json

                #region Data\IPBlackList.json
                if (File.Exists(ApplicationDirectory + "\\Data\\IPBlackList.json"))
                {
                    using (JsonTextReader Reader = new JsonTextReader(new StringReader(File.ReadAllText(ApplicationDirectory + "\\Data\\IPBlackList.json"))))
                    {
                        Reader.DateParseHandling = DateParseHandling.DateTime;
                        Reader.FloatParseHandling = FloatParseHandling.Double;

                        int StartObjectDepth = -1;
                        string PropertyName = null;
                        string TempPropertyName = null;

                        string IPAddress = null;
                        string Reason = null;
                        DateTime StartTime = DateTime.Now;
                        int Duration = -1;

                        while (Reader.Read())
                        {
                            if (Reader.TokenType == JsonToken.StartObject)
                            {
                                StartObjectDepth++;
                            }
                            else if (Reader.TokenType == JsonToken.EndObject)
                            {
                                if (StartObjectDepth == 1)
                                {
                                    IPBlackListData.Add(new IPBlackList(IPAddress, Reason, StartTime, Duration));
                                    IPAddress = null;
                                    Reason = null;
                                    StartTime = DateTime.Now;
                                    Duration = -1;
                                }
                                StartObjectDepth--;
                            }

                            if (Reader.TokenType == JsonToken.PropertyName)
                            {
                                TempPropertyName = Reader.Value.ToString();
                            }
                            else if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                            {
                                PropertyName = TempPropertyName;
                                TempPropertyName = null;
                            }

                            if (StartObjectDepth == 1)
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "IPAddress", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            IPAddress = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"IPBlackList.IPAddress\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Reason", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            Reason = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"IPBlackList.Reason\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "StartTime", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Date)
                                        {
                                            StartTime = (DateTime)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"IPBlackList.StartTime\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Duration", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            Duration = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"IPBlackList.Duration\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion Data\IPBlackList.json

                #region Data\MapFileList.json
                #endregion Data\MapFileList.json

                #region Data\MuteList.json
                if (File.Exists(ApplicationDirectory + "\\Data\\MuteList.json"))
                {
                    using (JsonTextReader Reader = new JsonTextReader(new StringReader(File.ReadAllText(ApplicationDirectory + "\\Data\\MuteList.json"))))
                    {
                        Reader.DateParseHandling = DateParseHandling.DateTime;
                        Reader.FloatParseHandling = FloatParseHandling.Double;

                        int StartObjectDepth = -1;
                        string PropertyName = null;
                        string TempPropertyName = null;

                        string Name = null;
                        int GameJoltID = -1;
                        string Reason = null;
                        DateTime StartTime = DateTime.Now;
                        int Duration = -1;

                        while (Reader.Read())
                        {
                            if (Reader.TokenType == JsonToken.StartObject)
                            {
                                StartObjectDepth++;
                            }
                            else if (Reader.TokenType == JsonToken.EndObject)
                            {
                                if (StartObjectDepth == 1)
                                {
                                    MuteListData.Add(new MuteList(Name, GameJoltID, Reason, StartTime, Duration));
                                    Name = null;
                                    GameJoltID = -1;
                                    Reason = null;
                                    StartTime = DateTime.Now;
                                    Duration = -1;
                                }
                                StartObjectDepth--;
                            }

                            if (Reader.TokenType == JsonToken.PropertyName)
                            {
                                TempPropertyName = Reader.Value.ToString();
                            }
                            else if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                            {
                                PropertyName = TempPropertyName;
                                TempPropertyName = null;
                            }

                            if (StartObjectDepth == 1)
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "Name", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            Name = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"MuteList.Name\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            GameJoltID = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"MuteList.GameJoltID\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Reason", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            Reason = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"MuteList.Reason\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "StartTime", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Date)
                                        {
                                            StartTime = (DateTime)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"MuteList.StartTime\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Duration", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            Duration = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"MuteList.Duration\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion Data\MuteList.json

                #region Data\OperatorList.json
                if (File.Exists(ApplicationDirectory + "\\Data\\OperatorList.json"))
                {
                    using (JsonTextReader Reader = new JsonTextReader(new StringReader(File.ReadAllText(ApplicationDirectory + "\\Data\\OperatorList.json"))))
                    {
                        Reader.DateParseHandling = DateParseHandling.DateTime;
                        Reader.FloatParseHandling = FloatParseHandling.Double;

                        int StartObjectDepth = -1;
                        string PropertyName = null;
                        string TempPropertyName = null;

                        string Name = null;
                        int GameJoltID = -1;
                        string Reason = null;
                        int OperatorLevel = (int)Player.OperatorTypes.Player;

                        while (Reader.Read())
                        {
                            if (Reader.TokenType == JsonToken.StartObject)
                            {
                                StartObjectDepth++;
                            }
                            else if (Reader.TokenType == JsonToken.EndObject)
                            {
                                if (StartObjectDepth == 1)
                                {
                                    OperatorListData.Add(new OperatorList(Name, GameJoltID, Reason, OperatorLevel));
                                    Name = null;
                                    GameJoltID = -1;
                                    Reason = null;
                                    OperatorLevel = (int)Player.OperatorTypes.Player;
                                }
                                StartObjectDepth--;
                            }

                            if (Reader.TokenType == JsonToken.PropertyName)
                            {
                                TempPropertyName = Reader.Value.ToString();
                            }
                            else if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                            {
                                PropertyName = TempPropertyName;
                                TempPropertyName = null;
                            }

                            if (StartObjectDepth == 1)
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "Name", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            Name = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"OperatorList.Name\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            GameJoltID = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"OperatorList.GameJoltID\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Reason", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            Reason = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"OperatorList.Reason\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "OperatorLevel", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            OperatorLevel = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"OperatorList.OperatorLevel\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion Data\OperatorList.json

                #region Data\SwearInfractionFilterList.json
                #endregion Data\SwearInfractionFilterList.json

                #region Data\SwearInfractionList.json
                if (File.Exists(ApplicationDirectory + "\\Data\\SwearInfractionList.json"))
                {
                    using (JsonTextReader Reader = new JsonTextReader(new StringReader(File.ReadAllText(ApplicationDirectory + "\\Data\\SwearInfractionList.json"))))
                    {
                        Reader.DateParseHandling = DateParseHandling.DateTime;
                        Reader.FloatParseHandling = FloatParseHandling.Double;

                        int StartObjectDepth = -1;
                        string PropertyName = null;
                        string TempPropertyName = null;

                        string Name = null;
                        int GameJoltID = -1;
                        int Points = 0;
                        int Muted = 0;
                        DateTime StartTime = DateTime.Now;

                        while (Reader.Read())
                        {
                            if (Reader.TokenType == JsonToken.StartObject)
                            {
                                StartObjectDepth++;
                            }
                            else if (Reader.TokenType == JsonToken.EndObject)
                            {
                                if (StartObjectDepth == 1)
                                {
                                    SwearInfractionListData.Add(new SwearInfractionList(Name, GameJoltID, Points, Muted, StartTime));
                                    Name = null;
                                    GameJoltID = -1;
                                    Points = 0;
                                    Muted = 0;
                                    StartTime = DateTime.Now;
                                }
                                StartObjectDepth--;
                            }

                            if (Reader.TokenType == JsonToken.PropertyName)
                            {
                                TempPropertyName = Reader.Value.ToString();
                            }
                            else if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                            {
                                PropertyName = TempPropertyName;
                                TempPropertyName = null;
                            }

                            if (StartObjectDepth == 1)
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "Name", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            Name = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"SwearInfractionList.Name\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            GameJoltID = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"SwearInfractionList.GameJoltID\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Points", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            Points = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"SwearInfractionList.Points\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Muted", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            Muted = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"SwearInfractionList.Muted\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "StartTime", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Date)
                                        {
                                            StartTime = (DateTime)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"SwearInfractionList.StartTime\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion Data\SwearInfractionList.json

                #region Data\WhiteList.json
                if (File.Exists(ApplicationDirectory + "\\Data\\WhiteList.json"))
                {
                    using (JsonTextReader Reader = new JsonTextReader(new StringReader(File.ReadAllText(ApplicationDirectory + "\\Data\\WhiteList.json"))))
                    {
                        Reader.DateParseHandling = DateParseHandling.DateTime;
                        Reader.FloatParseHandling = FloatParseHandling.Double;

                        int StartObjectDepth = -1;
                        string PropertyName = null;
                        string TempPropertyName = null;

                        string Name = null;
                        int GameJoltID = -1;
                        string Reason = null;

                        while (Reader.Read())
                        {
                            if (Reader.TokenType == JsonToken.StartObject)
                            {
                                StartObjectDepth++;
                            }
                            else if (Reader.TokenType == JsonToken.EndObject)
                            {
                                if (StartObjectDepth == 1)
                                {
                                    WhiteListData.Add(new WhiteList(Name, GameJoltID, Reason));
                                    Name = null;
                                    GameJoltID = -1;
                                    Reason = null;
                                }
                                StartObjectDepth--;
                            }

                            if (Reader.TokenType == JsonToken.PropertyName)
                            {
                                TempPropertyName = Reader.Value.ToString();
                            }
                            else if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                            {
                                PropertyName = TempPropertyName;
                                TempPropertyName = null;
                            }

                            if (StartObjectDepth == 1)
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "Name", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            Name = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"WhiteList.Name\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            GameJoltID = Reader.Value.ToString().Toint();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"WhiteList.GameJoltID\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Reason", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            Reason = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"WhiteList.Reason\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion Data\WhiteList.json

                #region Data\Token.json
                if (File.Exists(ApplicationDirectory + "\\Data\\Token.json"))
                {
                    using (JsonTextReader Reader = new JsonTextReader(new StringReader(File.ReadAllText(ApplicationDirectory + "\\Data\\Token.json"))))
                    {
                        Reader.DateParseHandling = DateParseHandling.DateTime;
                        Reader.FloatParseHandling = FloatParseHandling.Double;

                        int StartObjectDepth = -1;
                        string PropertyName = null;
                        string TempPropertyName = null;

                        string Name = null;
                        string Description = null;

                        while (Reader.Read())
                        {
                            if (Reader.TokenType == JsonToken.StartObject)
                            {
                                StartObjectDepth++;
                            }
                            else if (Reader.TokenType == JsonToken.EndObject)
                            {
                                if (StartObjectDepth == 1)
                                {
                                    if (TokenDefination.ContainsKey(Name))
                                    {
                                        TokenDefination[Name] = Description;
                                    }
                                    else
                                    {
                                        TokenDefination.Add(Name, Description);
                                    }
                                    Name = null;
                                    Description = null;
                                }
                                StartObjectDepth--;
                            }

                            if (Reader.TokenType == JsonToken.PropertyName)
                            {
                                TempPropertyName = Reader.Value.ToString();
                            }
                            else if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                            {
                                PropertyName = TempPropertyName;
                                TempPropertyName = null;
                            }

                            if (StartObjectDepth == 1)
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "Name", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            Name = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Token.Name\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Description", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            Description = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Add("Settings.cs: \"Token.Description\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion Data\Token.json

                Core.Logger.Add("Setting.cs: Loaded Setting.", Logger.LogTypes.Info);
                return true;
            }
            catch (Exception ex)
            {
                ex.CatchError();
                Core.Logger.Add("Setting.cs: Load Setting failed.", Logger.LogTypes.Info);
                return false;
            }
        }

        /// <summary>
        /// Save Setting File
        /// </summary>
        public void Save()
        {
            try
            {
                if (!Directory.Exists(ApplicationDirectory + "\\Data"))
                {
                    Directory.CreateDirectory(ApplicationDirectory + "\\Data");
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

                #region Data\BlackList.json
                string List = null;
                if (BlackListData.Count > 0)
                {
                    for (int i = 0; i < BlackListData.Count; i++)
                    {
                        List += string.Format(@"        {{
            ""Name"": ""{0}"",
            ""GameJoltID"": {1},
            ""Reason"": ""{2}"",
            ""StartTime"": ""{3}"",
            ""Duration"": {4}
        }},
",
BlackListData[i].Name,
BlackListData[i].GameJoltID.ToString(),
BlackListData[i].Reason,
BlackListData[i].StartTime.ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffK"),
BlackListData[i].Duration.ToString());
                    }

                    List = List.Remove(List.LastIndexOf(","));
                }

                File.WriteAllText(ApplicationDirectory + @"\Data\BlackList.json", string.Format(@"{{
    ""BlackListData"":
    [
{0}
    ]
}}", List), Encoding.Unicode);
                #endregion Data\BlackList.json

                #region Data\IPBlackList.json
                List = null;
                if (IPBlackListData.Count > 0)
                {
                    for (int i = 0; i < IPBlackListData.Count; i++)
                    {
                        List += string.Format(@"        {{
            ""IPAddress"": ""{0}"",
            ""Reason"": ""{1}"",
            ""StartTime"": ""{2}"",
            ""Duration"": {3}
        }},
",
IPBlackListData[i].IPAddress,
IPBlackListData[i].Reason,
IPBlackListData[i].StartTime.ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffK"),
IPBlackListData[i].Duration.ToString());
                    }

                    List = List.Remove(List.LastIndexOf(","));
                }

                File.WriteAllText(ApplicationDirectory + @"\Data\IPBlackList.json", string.Format(@"{{
    ""IPBlackListData"":
    [
{0}
    ]
}}", List), Encoding.Unicode);
                #endregion Data\IPBlackList.json

                #region Data\MapFileList.json
                #endregion Data\MapFileList.json

                #region Data\MuteList.json
                List = null;
                if (MuteListData.Count > 0)
                {
                    for (int i = 0; i < MuteListData.Count; i++)
                    {
                        List += string.Format(@"        {{
            ""Name"": ""{0}"",
            ""GameJoltID"": {1},
            ""Reason"": ""{2}"",
            ""StartTime"": ""{3}"",
            ""Duration"": {4}
        }},
",
MuteListData[i].Name,
MuteListData[i].GameJoltID.ToString(),
MuteListData[i].Reason,
MuteListData[i].StartTime.ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffK"),
MuteListData[i].Duration.ToString());
                    }

                    List = List.Remove(List.LastIndexOf(","));
                }

                File.WriteAllText(ApplicationDirectory + @"\Data\MuteList.json", string.Format(@"{{
    ""MuteListData"":
    [
{0}
    ]
}}", List), Encoding.Unicode);
                #endregion Data\MuteList.json

                #region Data\OperatorList.json
                List = null;
                if (OperatorListData.Count > 0)
                {
                    for (int i = 0; i < OperatorListData.Count; i++)
                    {
                        List += string.Format(@"        {{
            ""Name"": ""{0}"",
            ""GameJoltID"": {1},
            ""Reason"": ""{2}"",
            ""OperatorLevel"": {3}
        }},
",
OperatorListData[i].Name,
OperatorListData[i].GameJoltID.ToString(),
OperatorListData[i].Reason,
OperatorListData[i].OperatorLevel.ToString());
                    }

                    List = List.Remove(List.LastIndexOf(","));
                }

                File.WriteAllText(ApplicationDirectory + @"\Data\OperatorList.json", string.Format(@"{{
    ""OperatorListData"":
    [
{0}
    ]
}}", List), Encoding.Unicode);
                #endregion Data\OperatorList.json

                #region Data\SwearInfractionFilterList.json
                #endregion Data\SwearInfractionFilterList.json

                #region Data\SwearInfractionList.json
                List = null;
                if (SwearInfractionListData.Count > 0)
                {
                    for (int i = 0; i < SwearInfractionListData.Count; i++)
                    {
                        List += string.Format(@"        {{
            ""Name"": ""{0}"",
            ""GameJoltID"": {1},
            ""Points"": {2},
            ""Muted"": {3},
            ""StartTime"": ""{4}""
        }},
",
SwearInfractionListData[i].Name,
SwearInfractionListData[i].GameJoltID.ToString(),
SwearInfractionListData[i].Points.ToString(),
SwearInfractionListData[i].Muted.ToString(),
SwearInfractionListData[i].StartTime.ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffK"));
                    }

                    List = List.Remove(List.LastIndexOf(","));
                }

                File.WriteAllText(ApplicationDirectory + @"\Data\SwearInfractionList.json", string.Format(@"{{
    ""SwearInfractionListData"":
    [
{0}
    ]
}}", List), Encoding.Unicode);
                #endregion Data\SwearInfractionList.json

                #region Data\WhiteList.json
                List = null;
                if (WhiteListData.Count > 0)
                {
                    for (int i = 0; i < WhiteListData.Count; i++)
                    {
                        List += string.Format(@"        {{
            ""Name"": ""{0}"",
            ""GameJoltID"": {1},
            ""Reason"": ""{2}""
        }},
",
WhiteListData[i].Name,
WhiteListData[i].GameJoltID.ToString(),
WhiteListData[i].Reason);
                    }

                    List = List.Remove(List.LastIndexOf(","));
                }

                File.WriteAllText(ApplicationDirectory + @"\Data\WhiteList.json", string.Format(@"{{
    ""WhiteListData"":
    [
{0}
    ]
}}", List), Encoding.Unicode);
                #endregion Data\WhiteList.json

                #region Data\Token.json
                List = null;
                if (TokenDefination.Count > 0)
                {
                    foreach (KeyValuePair<string, string> Data in TokenDefination)
                    {
                        List += string.Format(@"        {{
            ""Name"": ""{0}"",
            ""Description"": ""{1}""
        }},
",
Data.Key,
Data.Value);
                    }

                    List = List.Remove(List.LastIndexOf(","));
                }

                File.WriteAllText(ApplicationDirectory + @"\Data\Token.json", string.Format(@"{{
    ""Token"":
    [
{0}
    ]
}}", List), Encoding.Unicode);
                #endregion Data\Token.json

                Core.Logger.Add("Setting.cs: Saved Setting.", Logger.LogTypes.Info);
            }
            catch (Exception ex)
            {
                ex.CatchError();
                Core.Logger.Add("Setting.cs: Save Setting failed.", Logger.LogTypes.Info);
            }
        }

        /// <summary>
        /// Check for setting file.
        /// </summary>
        /// <param name="Files">File Name with extension.</param>
        public bool HaveSettingFile(string Files)
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
        public string Token(string Key, params string[] Variable)
        {
            string ReturnValue = null;

            if (TokenDefination.ContainsKey(Key))
            {
                ReturnValue = string.Format(TokenDefination[Key], Variable);
            }

            return ReturnValue;
        }

        #region Data List Functions
        #region BlackList

        #endregion BlackList
        #endregion Data List Functions
    }
}