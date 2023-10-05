using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data;
using Pokemon_3D_Server_Core.Server_Client_Listener.Worlds;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Settings
{
    /// <summary>
    /// Class containing Settings
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// Get/Set Application Directory.
        /// </summary>
        public string ApplicationDirectory { get; private set; }

        #region Main Application Setting
        /// <summary>
        /// Get Startup Time.
        /// </summary>
        public DateTime StartTime { get; } = DateTime.Now;

        private string _ApplicationVersion { get; set; }
        /// <summary>
        /// Get Application Version.
        /// </summary>
        public string ApplicationVersion { get; } = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        /// <summary>
        /// Get Protocol Version.
        /// </summary>
        public string ProtocolVersion { get; } = "0.5";

        /// <summary>
        /// Get/Set Check For Update.
        /// </summary>
        public bool CheckForUpdate { get; set; } = true;

        /// <summary>
        /// Get/Set Generate Public IP.
        /// </summary>
        public bool GeneratePublicIP { get; set; } = true;

        /// <summary>
        /// Get/Set Main Entry Point.
        /// </summary>
        public MainEntryPointType MainEntryPoint { get; set; } = MainEntryPointType.jianmingyong_Server;

        /// <summary>
        /// Main Entry Point Type.
        /// </summary>
        public enum MainEntryPointType
        {
            /// <summary>
            /// Main Entry Point Type: jianmingyong Server
            /// </summary>
            jianmingyong_Server,

            /// <summary>
            /// Main Entry Point Type: Rcon
            /// </summary>
            Rcon,
        }
        #endregion Main Application Setting

        #region Main Server Property
        /// <summary>
        /// Get/Set IP Address
        /// </summary>
        public IPAddress _IPAddress = Functions.GetPublicIP() == null ? null : System.Net.IPAddress.Parse(Functions.GetPublicIP());
        /// <summary>
        /// Get/Set IP Address
        /// </summary>
        public string IPAddress
        {
            get
            {
                return _IPAddress == null ? "" : _IPAddress.ToString();
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _IPAddress = GeneratePublicIP ? Functions.GetPublicIP() == null ? null : System.Net.IPAddress.Parse(Functions.GetPublicIP()) : null;
                }
                else
                {
                    _IPAddress = System.Net.IPAddress.Parse(value);
                }
            }
        }

        private int _Port = 15124;
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
                _Port = value.Clamp(0, 65535);
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

        #region GameMode
        /// <summary>
        /// Get/Set GameMode
        /// </summary>
        public List<string> GameMode { get; set; } = new List<string> { };

        /// <summary>
        /// Get/Set GM_Pokemon3D
        /// </summary>
        public bool GM_Pokemon3D { get; set; } = true;

        /// <summary>
        /// Get/Set GM_1YearLater3D
        /// </summary>
        public bool GM_1YearLater3D { get; set; } = false;

        /// <summary>
        /// Get/Set GM_DarkfireMode
        /// </summary>
        public bool GM_DarkfireMode { get; set; } = false;

        /// <summary>
        /// Get/Set GM_German
        /// </summary>
        public bool GM_German { get; set; } = false;

        /// <summary>
        /// Get/Set GM_PokemonGoldSilverRandomLocke
        /// </summary>
        public bool GM_PokemonGoldSilverRandomLocke { get; set; } = false;

        /// <summary>
        /// Get/Set GM_PokemonLostSilver
        /// </summary>
        public bool GM_PokemonLostSilver { get; set; } = false;

        /// <summary>
        /// Get/Set GM_PokemonSilversSoul
        /// </summary>
        public bool GM_PokemonSilversSoul { get; set; } = false;

        /// <summary>
        /// Get/Set GM_PokemonUniversal3D
        /// </summary>
        public bool GM_PokemonUniversal3D { get; set; } = false;

        /// <summary>
        /// Get/Set GM_Others
        /// </summary>
        public string GM_Others { get; set; } = "";
        #endregion GameMode

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
                _Season = value.Clamp(-3, 3);
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
                _Weather = value.Clamp(-4, 9);
            }
        }

        /// <summary>
        /// Get/Set Do DayCycle
        /// </summary>
        public bool DoDayCycle { get; set; } = true;

        /// <summary>
        /// Get/Set Time Offset
        /// </summary>
        public int TimeOffset { get; set; } = 0;

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

        #region Network Ping System
        private int _NoPingKickTime = 30;
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
        #endregion Network Ping System

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

        /// <summary>
        /// Get/Set SwearInfraction Filter List
        /// </summary>
        public List<SwearInfractionFilterList> SwearInfractionFilterListData { get; set; } = new List<SwearInfractionFilterList>();

        #region Swear Infraction Feature
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

        #region Chat Feature
        /// <summary>
        /// Get/Set Allow Chat in server
        /// </summary>
        public bool AllowChatInServer { get; set; } = true;

        /// <summary>
        /// Get/Set Allow Chat Channels
        /// </summary>
        public bool AllowChatChannels { get; set; } = false;

        /// <summary>
        /// Get/Set Custom Chat Channels.
        /// </summary>
        public List<string> CustomChannels { get; set; } = new List<string> { "German Lounge" };

        private int _SpamResetDuration = -1;
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
        #endregion Chat Feature

        #region PvP Feature
        /// <summary>
        /// Get/Set Allow PvP
        /// </summary>
        public bool AllowPvP { get; set; } = true;

        /// <summary>
        /// Get/Set Allow PvP Validation
        /// </summary>
        public bool AllowPvPValidation { get; set; } = true;
        #endregion PvP Feature

        #region Trade Feature
        /// <summary>
        /// Get/Set Allow Trade
        /// </summary>
        public bool AllowTrade { get; set; } = true;
        #endregion Trade Feature
        #endregion Feature
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

        #region RCON Server Property
        /// <summary>
        /// Get/Set RCON Enable?
        /// </summary>
        public bool RCONEnable { get; set; } = true;

        private int _RCONPort = 15125;
        /// <summary>
        /// Get/Set RCON Port.
        /// </summary>
        public int RCONPort
        {
            get
            {
                return _RCONPort;
            }
            set
            {
                _RCONPort = value.Clamp(0, 65535);
            }
        }

        /// <summary>
        /// Get/Set RCON Password.
        /// </summary>
        public string RCONPassword { get; set; } = "Password";
        #endregion RCON Server Property

        /// <summary>
        /// Get/Set Token Defination
        /// </summary>
        public Dictionary<string, string> TokenDefination { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Get/Set Map File List Data
        /// </summary>
        public List<MapFileList> MapFileListData { get; set; } = new List<MapFileList>();

        /// <summary>
        /// New Setting.
        /// </summary>
        /// <param name="Directory">Start Directory.</param>
        public Setting(string Directory)
        {
            ApplicationDirectory = Directory;

            // Initialize Tokens
            #region Player Name Text
            TokenDefination.Add("SERVER_GAMEJOLT", "{0} ({1}) {2}");
            TokenDefination.Add("SERVER_NOGAMEJOLT", "{0} {1}");
            TokenDefination.Add("SERVER_CHATGAMEJOLT", "<{0} ({1})>: {2}");
            TokenDefination.Add("SERVER_CHATNOGAMEJOLT", "<{0}>: {1}");
            TokenDefination.Add("SERVER_COMMANDGAMEJOLT", "[Command] {0} ({1}) {2}");
            TokenDefination.Add("SERVER_COMMANDNOGAMEJOLT", "[Command] {0} {1}");
            #endregion Player Name Text

            #region Player Join Messages
            TokenDefination.Add("SERVER_FULL", "This server is currently full of players.");
            TokenDefination.Add("SERVER_OFFLINEMODE", "This server do not allow offline save.");
            TokenDefination.Add("SERVER_WRONGGAMEMODE", "This server require you to play the following gamemode: {0}.");
            TokenDefination.Add("SERVER_DISALLOW", "You do not have required permission to join the server. Please try again later.");
            TokenDefination.Add("SERVER_CLONE", "You are still in the server. Please try again later.");
            #endregion Player Join Messages

            #region Player Left Messages
            TokenDefination.Add("SERVER_AFK", "You have been afking for too long.");
            TokenDefination.Add("SERVER_PLAYERLEFT", "You have left the server.");
            TokenDefination.Add("SERVER_NOPING", "You have a slow connection or you have disconnected from internet for too long.");
            TokenDefination.Add("SERVER_KICKED", "You have been kicked in the server with the following reason: {0}");
            #endregion Player Left Messages

            #region Client Events
            TokenDefination.Add("SERVER_CLOSE", "This server have been shut down or lost its connection. Sorry for the inconveniences caused.");
            TokenDefination.Add("SERVER_RESTART", "This server is restarting. Sorry for the inconveniences caused.");
            TokenDefination.Add("SERVER_RESTARTWARNING", "The server is scheduled to restart in {0}. Please enjoy your stay.");
            TokenDefination.Add("SERVER_TRADEPVPFAIL", "The server is scheduled to restart in {0}. For your personal safety, starting a new trading and PvP is disabled.");
            #endregion Client Events

            #region Ban / Mute
            TokenDefination.Add("SERVER_BLACKLISTED", "You have been banned from server. Reason: {0} | Ban duration: {1}.");
            TokenDefination.Add("SERVER_IPBLACKLISTED", "You have been ip banned from server. Reason: {0} | Ban duration: {1}.");
            TokenDefination.Add("SERVER_MUTED", "You have been muted in the server. Reason: {0} | Ban duration: {1}.");
            TokenDefination.Add("SERVER_MUTEDTEMP", "You have been muted by that player. Reason: {0} | Ban duration: {1}.");
            TokenDefination.Add("SERVER_SWEAR", "Please avoid swearing where necessary. Triggered word: {0}");
            TokenDefination.Add("SERVER_SWEARWARNING", "Please avoid swearing where necessary. Triggered word: {0} | You have {1} infraction point. {2} infraction point will get a timeout.");
            #endregion Ban / Mute

            #region Using Command
            TokenDefination.Add("SERVER_COMMANDPERMISSION", "You do not have the required permission to use this command.");
            TokenDefination.Add("SERVER_PLAYERNOTEXIST", "The requested player does not exist in the server. Please try again.");
            TokenDefination.Add("SERVER_KICKSELF", "You are trying to kick yourself. For your personal safety, we will not kick you :)");
            TokenDefination.Add("SERVER_NOTOPERATOR", "The requested player is not an operator.");
            #endregion Using Command

            TokenDefination.Add("SERVER_LOGINTIME", "You have played in the server for {0} hour(s). We encourage your stay but also encourage you to take a small break :)");
            TokenDefination.Add("SERVER_SPAM", "Please be unique :) don't send the same message again in quick succession.");
            TokenDefination.Add("RCON_CONNECTFAILED", "Unable to connect to the requested server. Please try again.");
            TokenDefination.Add("SERVER_PVPVALIDATION", "You are unable to use this party due to the following reason: {0}");
            TokenDefination.Add("SERVER_PVPDISALLOW", "This server do not allow user to PvP. Sorry for the inconveniences caused.");
            TokenDefination.Add("SERVER_TRADEDISALLOW", "This server do not allow user to Trade. Sorry for the inconveniences caused.");
            TokenDefination.Add("SERVER_NOCHAT", "This server do not allow user to chat. Sorry for the inconveniences caused.");
            TokenDefination.Add("SERVER_CURRENTCHATCHANNEL", "You are now at {0} Chat Channel. For more info, type \" /help chatchannel \".");
            TokenDefination.Add("SERVER_ERROR", "Package Data Error: {0}. Unable to verify.");

            OperatorListData.Add(new OperatorList("jianmingyong", 116016, "I am the god of time.", (int)Player.OperatorTypes.Creator));
            OperatorListData.Add(new OperatorList("jianmingyong1998", 222452, "I am the god of space.", (int)Player.OperatorTypes.Creator));
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
                                    this.SeasonMonth = new SeasonMonth(TempValue);
                                }
                                else if (StartObjectDepth == 3 && string.Equals(ObjectPropertyName, "WeatherSeason", StringComparison.OrdinalIgnoreCase))
                                {
                                    string TempValue = null;
                                    foreach (string item in WeatherSeason)
                                    {
                                        TempValue += item + "|";
                                    }
                                    TempValue = TempValue.Remove(TempValue.LastIndexOf("|"));
                                    this.WeatherSeason = new WeatherSeason(TempValue);
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

                            #region Main Application Setting
                            if (StartObjectDepth == 1 && string.Equals(ObjectPropertyName, "Main Application Setting", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "ApplicationVersion", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            _ApplicationVersion = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Main Application Setting.ApplicationVersion\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "CheckForUpdate", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            CheckForUpdate = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Main Application Setting.CheckForUpdate\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Main Application Setting.GeneratePublicIP\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "MainEntryPoint", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            if (Reader.Value.ToString().ToInt() == (int)MainEntryPointType.jianmingyong_Server)
                                            {
                                                MainEntryPoint = MainEntryPointType.jianmingyong_Server;
                                            }
                                            else if (Reader.Value.ToString().ToInt() == (int)MainEntryPointType.Rcon)
                                            {
                                                MainEntryPoint = MainEntryPointType.Rcon;
                                            }
                                            else
                                            {
                                                MainEntryPoint = MainEntryPointType.jianmingyong_Server;
                                            }
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Main Application Setting.MainEntryPoint\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion Main Application Setting

                            #region Main Server Property
                            else if (StartObjectDepth == 1 && string.Equals(ObjectPropertyName, "Main Server Property", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "IPAddress", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            IPAddress = GeneratePublicIP ? Functions.GetPublicIP() : Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Main Server Property.IPAddress\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Port", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            Port = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Main Server Property.Port\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Main Server Property.ServerName\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Main Server Property.ServerMessage\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Main Server Property.WelcomeMessage\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion Main Server Property

                            #region GameMode
                            else if (StartObjectDepth == 2 && string.Equals(ObjectPropertyName, "GameMode", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {

                                    if (string.Equals(PropertyName, "Kolben", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            GM_Pokemon3D = (bool)Reader.Value;

                                            if (GM_Pokemon3D)
                                            {
                                                GameMode.Add("Kolben");
                                            }
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Main Server Property.GameMode.Kolben\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "1 Year Later 3D", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            GM_1YearLater3D = (bool)Reader.Value;

                                            if (GM_1YearLater3D)
                                            {
                                                GameMode.Add("1 Year Later 3D");
                                            }
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Main Server Property.GameMode.1 Year Later 3D\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Darkfire Mode", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            GM_DarkfireMode = (bool)Reader.Value;

                                            if (GM_DarkfireMode)
                                            {
                                                GameMode.Add("Darkfire Mode");
                                            }
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Main Server Property.GameMode.Darkfire Mode\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "German", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            GM_German = (bool)Reader.Value;

                                            if (GM_German)
                                            {
                                                GameMode.Add("German");
                                            }
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Main Server Property.GameMode.German\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Pokemon Gold&Silver - RandomLocke", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            GM_PokemonGoldSilverRandomLocke = (bool)Reader.Value;

                                            if (GM_PokemonGoldSilverRandomLocke)
                                            {
                                                GameMode.Add("Pokemon Gold&Silver - RandomLocke");
                                            }
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Main Server Property.GameMode.Pokemon Gold&Silver - RandomLocke\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Pokemon Lost Silver", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            GM_PokemonLostSilver = (bool)Reader.Value;

                                            if (GM_PokemonLostSilver)
                                            {
                                                GameMode.Add("Pokemon Lost Silver");
                                            }
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Main Server Property.GameMode.Pokemon Lost Silver\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Pokemon Silver's Soul", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            GM_PokemonSilversSoul = (bool)Reader.Value;

                                            if (GM_PokemonSilversSoul)
                                            {
                                                GameMode.Add("Pokemon Silver's Soul");
                                            }
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Main Server Property.GameMode.Pokemon Silver's Soul\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Pokemon Universal 3D", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            GM_PokemonUniversal3D = (bool)Reader.Value;

                                            if (GM_PokemonUniversal3D)
                                            {
                                                GameMode.Add("Pokemon Universal 3D");
                                            }
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Main Server Property.GameMode.Pokemon Universal 3D\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Others", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            GM_Others = Reader.Value.ToString();

                                            for (int i = 0; i < GM_Others.SplitCount(","); i++)
                                            {
                                                GameMode.Add(GM_Others.Split(',')[i].Trim());
                                            }
                                        }
                                        else if (Reader.TokenType == JsonToken.Null)
                                        {
                                            GM_Others = "";
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Main Server Property.GameMode.Others\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion GameMode

                            #region Main Server Property
                            else if (StartObjectDepth == 1 && string.Equals(ObjectPropertyName, "GameMode", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {

                                    if (string.Equals(PropertyName, "MaxPlayers", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            MaxPlayers = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Main Server Property.MaxPlayers\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Main Server Property.OfflineMode\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Season = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Advanced Server Property.World.Season\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Weather", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            Weather = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Advanced Server Property.World.Weather\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Advanced Server Property.World.DoDayCycle\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "TimeOffset", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            TimeOffset = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Advanced Server Property.World.TimeOffset\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Advanced Server Property.World.SeasonMonth\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Advanced Server Property.World.WeatherSeason\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Advanced Server Property.World.DefaultWorldCountry\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion World

                            #region Network Ping System
                            else if (StartObjectDepth == 2 && string.Equals(ObjectPropertyName, "Network Ping System", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "NoPingKickTime", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            NoPingKickTime = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Advanced Server Property.Network Ping System.NoPingKickTime\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "AFKKickTime", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            AFKKickTime = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Advanced Server Property.Network Ping System.AFKKickTime\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "AutoRestartTime", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            AutoRestartTime = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Advanced Server Property.Network Ping System.AutoRestartTime\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion Network Ping System

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
                                            Core.Logger.Log("\"Advanced Server Property.Features.BlackList\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Advanced Server Property.Features.IPBlackList\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Advanced Server Property.Features.WhiteList\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Advanced Server Property.Features.OperatorList\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Advanced Server Property.Features.MuteList\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Advanced Server Property.Features.OnlineSettingList\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Advanced Server Property.Features.SwearInfractionList\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            SwearInfractionCap = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Advanced Server Property.World.Swear Infraction Feature.SwearInfractionCap\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "SwearInfractionReset", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            SwearInfractionReset = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Advanced Server Property.World.Swear Infraction Feature.SwearInfractionReset\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion Swear Infraction Feature

                            #region Chat Feature
                            else if (StartObjectDepth == 3 && string.Equals(ObjectPropertyName, "Chat Feature", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "AllowChatInServer", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            AllowChatChannels = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Advanced Server Property.World.Chat Feature.AllowChatInServer\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "AllowChatChannels", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            AllowChatChannels = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Advanced Server Property.World.Chat Feature.AllowChatChannels\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "CustomChannels", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            CustomChannels = Reader.Value.ToString().Split(',').ToList();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Advanced Server Property.World.Chat Feature.CustomChannels\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "SpamResetDuration", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            SpamResetDuration = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Advanced Server Property.World.Chat Feature.SpamResetDuration\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion Chat Feature

                            #region PvP Feature
                            else if (StartObjectDepth == 3 && string.Equals(ObjectPropertyName, "PvP Feature", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "AllowPvP", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            AllowPvP = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Advanced Server Property.World.PvP Feature.AllowPvP\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "AllowPvPValidation", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            AllowPvPValidation = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Advanced Server Property.World.PvP Feature.AllowPvPValidation\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion PvP Feature

                            #region Trade Feature
                            else if (StartObjectDepth == 3 && string.Equals(ObjectPropertyName, "Trade Feature", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "AllowTrade", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            AllowTrade = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Advanced Server Property.World.Trade Feature.AllowTrade\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion Trade Feature

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
                                            Core.Logger.Log("\"Server Client Logger.LoggerInfo\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Server Client Logger.LoggerWarning\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "LoggerDebug", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
#if DEBUG
                                            LoggerDebug = true;
#else
                                            LoggerDebug = false;
#endif
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"Server Client Logger.LoggerDebug\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Server Client Logger.LoggerChat\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Server Client Logger.LoggerPM\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Server Client Logger.LoggerServer\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Server Client Logger.LoggerTrade\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Server Client Logger.LoggerPvP\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Server Client Logger.LoggerCommand\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            #endregion Server Client Logger

                            #region RCON Server Property
                            else if (StartObjectDepth == 1 && string.Equals(ObjectPropertyName, "RCON Server Property", StringComparison.OrdinalIgnoreCase))
                            {
                                if (Reader.TokenType == JsonToken.Boolean || Reader.TokenType == JsonToken.Bytes || Reader.TokenType == JsonToken.Date || Reader.TokenType == JsonToken.Float || Reader.TokenType == JsonToken.Integer || Reader.TokenType == JsonToken.Null || Reader.TokenType == JsonToken.String)
                                {
                                    if (string.Equals(PropertyName, "RCONEnable", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            RCONEnable = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"RCON Server Property.RCONEnable\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "RCONPort", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            RCONPort = Reader.Value.ToString().ToUshort();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"RCON Server Property.RCONPort\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "RCONPassword", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            RCONPassword = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"RCON Server Property.RCONPassword\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }

                            #endregion RCON Server Property
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
                                            Core.Logger.Log("\"BlackList.Name\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            GameJoltID = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"BlackList.GameJoltID\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"BlackList.Reason\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"BlackList.StartTime\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Duration", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            Duration = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"BlackList.Duration\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"IPBlackList.IPAddress\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"IPBlackList.Reason\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"IPBlackList.StartTime\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Duration", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            Duration = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"IPBlackList.Duration\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"MuteList.Name\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            GameJoltID = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"MuteList.GameJoltID\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"MuteList.Reason\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"MuteList.StartTime\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Duration", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            Duration = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"MuteList.Duration\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                    if (!(GameJoltID == 116016 || GameJoltID == 222452))
                                    {
                                        OperatorListData.Add(new OperatorList(Name, GameJoltID, Reason, OperatorLevel));
                                    }
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
                                            Core.Logger.Log("\"OperatorList.Name\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            GameJoltID = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"OperatorList.GameJoltID\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"OperatorList.Reason\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "OperatorLevel", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            OperatorLevel = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"OperatorList.OperatorLevel\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion Data\OperatorList.json

                #region Data\SwearInfractionFilterList.json
                if (!File.Exists(ApplicationDirectory + "\\Data\\SwearInfractionFilterList.json"))
                {
                    using (WebClient Client = new WebClient() { CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache) })
                    {
                        Client.DownloadFile("https://github.com/jianmingyong/Pokemon-3D-Server-Client/raw/master/Pokemon.3D.Server.Core/Resource/SwearInfractionFilterListData.json", ApplicationDirectory + "\\Data\\SwearInfractionFilterList.json");
                    }
                }

                if (File.Exists(ApplicationDirectory + "\\Data\\SwearInfractionFilterList.json"))
                {
                    using (JsonTextReader Reader = new JsonTextReader(new StringReader(File.ReadAllText(ApplicationDirectory + "\\Data\\SwearInfractionFilterList.json"))))
                    {
                        Reader.DateParseHandling = DateParseHandling.DateTime;
                        Reader.FloatParseHandling = FloatParseHandling.Double;

                        int StartObjectDepth = -1;
                        string PropertyName = null;
                        string TempPropertyName = null;

                        string Word = null;
                        bool CaseSensitive = false;

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
                                    SwearInfractionFilterListData.Add(new SwearInfractionFilterList(Word, CaseSensitive));
                                    Word = null;
                                    CaseSensitive = false;
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
                                    if (string.Equals(PropertyName, "Word", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.String)
                                        {
                                            Word = Reader.Value.ToString();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"SwearInfractionFilterListData.Word\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "CaseSensitive", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Boolean)
                                        {
                                            CaseSensitive = (bool)Reader.Value;
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"SwearInfractionFilterListData.CaseSensitive\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
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
                                            Core.Logger.Log("\"SwearInfractionList.Name\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            GameJoltID = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"SwearInfractionList.GameJoltID\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Points", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            Points = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"SwearInfractionList.Points\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "Muted", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            Muted = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"SwearInfractionList.Muted\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"SwearInfractionList.StartTime\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"WhiteList.Name\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                    else if (string.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Reader.TokenType == JsonToken.Integer)
                                        {
                                            GameJoltID = Reader.Value.ToString().ToInt();
                                        }
                                        else
                                        {
                                            Core.Logger.Log("\"WhiteList.GameJoltID\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"WhiteList.Reason\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Token.Name\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("\"Token.Description\" does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion Data\Token.json

                #region Overwrite Setting Per Version
                if (_ApplicationVersion == "0.54.1.36")
                {
                    TokenDefination["SERVER_AFK"] = "You have been afk for too long.";
                }
                #endregion Overwrite Setting Per Version

                Core.Logger.Log("Setting loaded.", Logger.LogTypes.Info);
                return true;
            }
            catch (Exception ex)
            {
                ex.CatchError();
                Core.Logger.Log("Setting load failed.", Logger.LogTypes.Info);
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

    /*
        Warning: The syntax for each setting is case sensitive.
        String: ""Text inside a quote you may use escape char like \\""
        Integer: 0123456789
        Boolean: true false

        Please use the GUI Setting Editor in the server console if you are unsure how to change setting here.
    */

    ""Main Application Setting"":
    {{
        ""StartTime"": ""{0}"",
        ""ApplicationVersion"": ""{1}"",
        ""ProtocolVersion"": ""{2}"",

        /*
            CheckForUpdate: To allow or disallow the application to check for update upon launch.
            Required Syntax: Boolean.
        */
        ""CheckForUpdate"": {3},

        /*
            GeneratePublicIP: To allow or disallow the application to update the IP address upon launch.
            Required Syntax: Boolean.
        */
        ""GeneratePublicIP"": {4},

        /*
            MainEntryPoint: The main entry point of the server console.
            Required Syntax: Integer.
            jianmingyong Server Instance = 0 | RCON = 1
        */
        ""MainEntryPoint"": {5},
    }},

    ""Main Server Property"":
    {{
        /*
            IPAddress: Public/External IP address of your server.
            Required Syntax: Valid IPv4 address.
        */
        ""IPAddress"": ""{6}"",

        /*
            Port: The port to use on your server.
            Required Syntax: Integer between 0 to 65535 inclusive.
            Port cannot be the same as SCON and RCON.
        */
        ""Port"": {7},

        /*
            ServerName: The server name to be display to public.
            Required Syntax: String.
        */
        ""ServerName"": ""{8}"",

        /*
            ServerMessage: The server message to display when a player select a server.
            Required Syntax: String.
            ""ServerMessage"": null, => If you do not want to display the server message.
        */
        ""ServerMessage"": {9},

        /*
            WelcomeMessage: The server message to display when a player joins a server.
            Required Syntax: String.
            ""WelcomeMessage"": null, => If you do not want to display the welcome message.
        */
        ""WelcomeMessage"": {10},

        ""GameMode"":
        {{
            /*
                Default GameMode: To allow or disallow default GameMode to join the server.
                Required Syntax: Boolean.
            */
            ""Kolben"": {11},

            /*
                Approved GameMode by staff: To allow or disallow custom GameMode to join the server.
                Required Syntax: Boolean.
            */
            ""1 Year Later 3D"": {12},
            ""Darkfire Mode"": {13},
            ""German"": {14},
            ""Pokemon Gold&Silver - RandomLocke"": {15},
            ""Pokemon Lost Silver"": {16},
            ""Pokemon Silver's Soul"": {17},
            ""Pokemon Universal 3D"": {18},

            /*
                Other GameMode: To support other GameMode.
                Required Syntax: String.
                You may insert multiple GameMode by using a "","" (comma) after each GameMode name.
            */
            ""Others"": {19}
        }},

        /*
            MaxPlayers: The maximum amount of player in the server that can join.
            Required Syntax: Integer. -1 = Unlimited Players. (Technically not unlimited but the bigggest amount the game can handle.)
        */
        ""MaxPlayers"": {20},

        /*
            OfflineMode: To allow or disallow offline save player joins the server.
            - It will be allowed if other GameMode other than default server is allowed to join the server.
            Required Syntax: Boolean.
        */
        ""OfflineMode"": {21}
    }},

    ""Advanced Server Property"":
    {{
        ""World"":
        {{
            /*
                Season: To set server default season.
                Required Syntax: Integer.
                Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2 | SeasonMonth = -3
            */
            ""Season"": {22},

            /*
                Weather: To set server default weather.
                Required Syntax: Integer.
                Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 | WeatherSeason = -3 | Real World Weather = -4
                Real world weather is not implemented. Do not use!
            */
            ""Weather"": {23},

            /*
                DoDayCycle: To allow or disallow the server to update day and night cycle.
			    Required Syntax: Boolean.
            */
			""DoDayCycle"": {24},

            /*
                Time Offset: Offset the time in the server.
                Required Syntax: Integer.
                The time offset is counted by seconds. 60 = 1 minute time difference from your local time.
            */
            ""TimeOffset"": {25},

            /*
                SeasonMonth: To set the season based on local date. Must set Season = -3
			    Required Syntax: Integer.
                Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2
			    You may insert more than one season by separating it with a "","" (comma).
            */
            ""SeasonMonth"":
            {{
                ""January"": ""{26}"",
                ""February"": ""{27}"",
                ""March"": ""{28}"",
                ""April"": ""{29}"",
                ""May"": ""{30}"",
                ""June"": ""{31}"",
                ""July"": ""{32}"",
                ""August"": ""{33}"",
                ""September"": ""{34}"",
                ""October"": ""{35}"",
                ""November"": ""{36}"",
                ""December"": ""{37}""
            }},

            /*
                WeatherSeason: To set the weather based on server season. Must set Weather = -3
			    Required Syntax: Integer.
                Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 | Real World Weather = -4
			    You may insert more than one season by separating it with a "","" (comma).
                Real world weather is not implemented. Do not use!
            */
            ""WeatherSeason"":
            {{
                ""Winter"": ""{38}"",
                ""Spring"": ""{39}"",
                ""Summer"": ""{40}"",
                ""Fall"": ""{41}""
            }},

            /*
                DefaultWorldCountry: To set the default country for real world weather.
                Required Syntax: String.
                Valid Country name / City name. No fancy character. Use Default A-Z a-z letter.
            */
            ""DefaultWorldCountry"": ""{42}""
        }},

        ""Network Ping System"":
        {{
            /*
                NoPingKickTime: To kick player out if there are no valid ping for n amount of seconds.
                Required Syntax: Integer. -1 to disable it.
            */
            ""NoPingKickTime"": {43},

            /*
                AFKKickTime: To kick player out if there are no valid activity for n amount of seconds.
			    Required Syntax: Integer. -1 to disable it.
            */
            ""AFKKickTime"": {44},
        
            /*
                AutoRestartTime: To automatically restart the server after n seconds. Disable PvP and trade features for the last 5 minutes of the countdown.
			    Required Syntax: Integer. -1 to disable it.
            */
            ""AutoRestartTime"": {45}
        }},

        ""Features"":
        {{
            /*
                BlackList: To allow or disallow using Blacklist feature.
			    Required Syntax: Boolean.
            */
            ""BlackList"": {46},

            /*
                IPBlackList: To allow or disallow using IPBlacklist feature.
			    Required Syntax: Boolean.
            */
            ""IPBlackList"": {47},

            /*
                WhiteList: To allow or disallow using whitelist feature.
			    Required Syntax: Boolean.
            */
            ""WhiteList"": {48},

            /*
                OperatorList: To allow or disallow using operator feature.
			    Required Syntax: Boolean.
            */
            ""OperatorList"": {49},

            /*
                MuteList: To allow or disallow using mute feature.
			    Required Syntax: Boolean.
            */
            ""MuteList"": {50},

            /*
                OnlineSettingList: To allow or disallow using Online Setting feature.
			    Required Syntax: Boolean.
            */
            ""OnlineSettingList"": {51},

            /*
                SwearInfractionList: To allow or disallow using swear infraction feature.
			    Required Syntax: Boolean.
            */
            ""SwearInfractionList"": {52},

            ""Swear Infraction Feature"":
            {{
                /*
                    SwearInfractionCap: Amount of infraction points before the first mute.
				    Required Syntax: Integer. -1 to disable.
                */
                ""SwearInfractionCap"": {53},

                /*
                    SwearInfractionReset: Amount of days before it expire the infraction count.
				    Required Syntax: Integer. -1 to disable.
                */
                ""SwearInfractionReset"": {54}
            }},

            ""Chat Feature"":
            {{
                /*
                    AllowChatInServer: To allow or disallow player to chat in the server.
                    Required Syntax: Boolean.
                */
                ""AllowChatInServer"": {55},

                /*
                    AllowChatChannels: To allow or disallow player to use chat channels in the server.
                    Required Syntax: Boolean.
                */
                ""AllowChatChannels"": {56},

                /*
                    CustomChannels: List of custom channels for the server.
                    Required Syntax: String.
                    ""CustomChannels"": null, => If there are no custom channels.
                */
                ""CustomChannels"": {57},

                /*
                    SpamResetDuration: Amount of seconds for the user to send the same word again.
				    Required Syntax: Integer. -1 to disable.
                */
                ""SpamResetDuration"": {58}
            }},

            ""PvP Feature"":
            {{
                /*
                    AllowPvP: To allow or disallow player to PvP in the server.
                    Required Syntax: Boolean.
                */
                ""AllowPvP"": {59},

                /*
                    AllowPvPValidation: To allow or disallow PvP Validation system.
                    Required Syntax: Boolean.
                    Online player can change all they want. Offline player do not have the power to change any.
                */
                ""AllowPvPValidation"": {60}
            }},

            ""Trade Feature"":
            {{
                /*
                    AllowTrade: To allow or disallow player to Trade in the server.
                    Required Syntax: Boolean.
                */
                ""AllowTrade"": {61}
            }}
        }}
    }},

    ""Server Client Logger"":
    {{
        /*
            LoggerInfo: To log server information.
            Required Syntax: Boolean.
        */
        ""LoggerInfo"": {62},

        /*
            LoggerWarning: To log server warning including ex exception.
            Required Syntax: Boolean.
        */
        ""LoggerWarning"": {63},

        ""LoggerDebug"": {64},

        /*
            LoggerChat:  To log server chat message.
            Required Syntax: Boolean.
        */
        ""LoggerChat"": {65},

        /*
            LoggerPM: To log server private chat message. (Actual Private Message content is not logged)
            Required Syntax: Boolean.
        */
        ""LoggerPM"": {66},

        /*
            LoggerServer: To log server message.
            Required Syntax: Boolean.
        */
        ""LoggerServer"": {67},

        /*
            LoggerTrade: To log trade request. (Actual Trade Request content is not logged)
            Required Syntax: Boolean.
        */
        ""LoggerTrade"": {68},

        /*
            LoggerPvP: To log pvp request. (Actual PvP Request content is not logged)
            Required Syntax: Boolean.
        */
        ""LoggerPvP"": {69},

        /*
            LoggerCommand: To log server command usage. (Debug Commands are not logged)
            Required Syntax: Boolean.
        */
        ""LoggerCommand"": {70}
    }},

    ""RCON Server Property"":
    {{
        /*
            RCONEnable: Enable RCON
            Required Syntax: Boolean.
        */
        ""RCONEnable"": {71},

        /*
            RCONPort: The port for RCON Listener.
            Required Syntax: Integer between 0 to 65535 inclusive.
        */
        ""RCONPort"": {72},

        /*
            RCONPassword: The password for the RCON to connect.
		    Required Syntax: String. Please do not insert password that contains your personal infomation.
        */
        ""RCONPassword"": ""{73}""
    }}
}}",
StartTime.ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffK"),
ApplicationVersion,
ProtocolVersion,
CheckForUpdate.ToString().ToLower(),
GeneratePublicIP.ToString().ToLower(),
(int)MainEntryPoint,
IPAddress,
Port.ToString(),
ServerName,
string.IsNullOrWhiteSpace(ServerMessage) ? "null" : @"""" + ServerMessage + @"""",
string.IsNullOrWhiteSpace(WelcomeMessage) ? "null" : @"""" + WelcomeMessage + @"""",
GM_Pokemon3D.ToString().ToLower(),
GM_1YearLater3D.ToString().ToLower(),
GM_DarkfireMode.ToString().ToLower(),
GM_German.ToString().ToLower(),
GM_PokemonGoldSilverRandomLocke.ToString().ToLower(),
GM_PokemonLostSilver.ToString().ToLower(),
GM_PokemonSilversSoul.ToString().ToLower(),
GM_PokemonUniversal3D.ToString().ToLower(),
string.IsNullOrWhiteSpace(GM_Others) ? "null" : @"""" + GM_Others + @"""",
MaxPlayers.ToString(),
OfflineMode.ToString().ToLower(),
Season.ToString(),
Weather.ToString(),
DoDayCycle.ToString().ToLower(),
TimeOffset.ToString(),
SeasonMonth.SeasonData.GetSplit(0),
SeasonMonth.SeasonData.GetSplit(1),
SeasonMonth.SeasonData.GetSplit(2),
SeasonMonth.SeasonData.GetSplit(3),
SeasonMonth.SeasonData.GetSplit(4),
SeasonMonth.SeasonData.GetSplit(5),
SeasonMonth.SeasonData.GetSplit(6),
SeasonMonth.SeasonData.GetSplit(7),
SeasonMonth.SeasonData.GetSplit(8),
SeasonMonth.SeasonData.GetSplit(9),
SeasonMonth.SeasonData.GetSplit(10),
SeasonMonth.SeasonData.GetSplit(11),
WeatherSeason.WeatherData.GetSplit(0),
WeatherSeason.WeatherData.GetSplit(1),
WeatherSeason.WeatherData.GetSplit(2),
WeatherSeason.WeatherData.GetSplit(3),
DefaultWorldCountry,
NoPingKickTime.ToString(),
AFKKickTime.ToString(),
AutoRestartTime.ToString(),
BlackList.ToString().ToLower(),
IPBlackList.ToString().ToLower(),
WhiteList.ToString().ToLower(),
OperatorList.ToString().ToLower(),
MuteList.ToString().ToLower(),
OnlineSettingList.ToString().ToLower(),
SwearInfractionList.ToString().ToLower(),
SwearInfractionCap.ToString(),
SwearInfractionReset.ToString(),
AllowChatInServer.ToString().ToLower(),
AllowChatChannels.ToString().ToLower(),
CustomChannels.Count == 0 ? "null" : @"""" + string.Join(",", CustomChannels) + @"""",
SpamResetDuration.ToString(),
AllowPvP.ToString().ToLower(),
AllowPvPValidation.ToString().ToLower(),
AllowTrade.ToString().ToLower(),
LoggerInfo.ToString().ToLower(),
LoggerWarning.ToString().ToLower(),
LoggerDebug.ToString().ToLower(),
LoggerChat.ToString().ToLower(),
LoggerPM.ToString().ToLower(),
LoggerServer.ToString().ToLower(),
LoggerTrade.ToString().ToLower(),
LoggerPvP.ToString().ToLower(),
LoggerCommand.ToString().ToLower(),
RCONEnable.ToString().ToLower(),
RCONPort.ToString(),
RCONPassword), Encoding.UTF8);
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
}}", List), Encoding.UTF8);
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
}}", List), Encoding.UTF8);
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
}}", List), Encoding.UTF8);
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
}}", List), Encoding.UTF8);
                #endregion Data\OperatorList.json

                #region Data\SwearInfractionFilterList.json
                List = null;
                if (SwearInfractionFilterListData.Count > 0)
                {
                    for (int i = 0; i < SwearInfractionFilterListData.Count; i++)
                    {
                        List += string.Format(@"        {{
            ""Word"": ""{0}"",
            ""CaseSensitive"": {1}
        }},
",
SwearInfractionFilterListData[i].Word,
SwearInfractionFilterListData[i].CaseSensitive.ToString().ToLower());
                    }

                    List = List.Remove(List.LastIndexOf(","));
                }

                File.WriteAllText(ApplicationDirectory + @"\Data\SwearInfractionFilterList.json", string.Format(@"{{
    ""SwearInfractionFilterListData"":
    [
{0}
    ]
}}", List), Encoding.UTF8);
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
}}", List), Encoding.UTF8);
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
}}", List), Encoding.UTF8);
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
Data.Value.Replace(@"\", @"\\").Replace(@"/", @"\/").Replace(@"""", @"\"""));
                    }

                    List = List.Remove(List.LastIndexOf(","));
                }

                File.WriteAllText(ApplicationDirectory + @"\Data\Token.json", string.Format(@"{{
    ""Token"":
    [
{0}
    ]
}}", List), Encoding.UTF8);
                #endregion Data\Token.json

                Core.Logger.Log("Setting saved.", Logger.LogTypes.Info);
            }
            catch (Exception ex)
            {
                ex.CatchError();
                Core.Logger.Log("Setting save failed.", Logger.LogTypes.Info);
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
                if (File.Exists(ApplicationDirectory + "\\" + Files))
                {
                    if (string.IsNullOrWhiteSpace(File.ReadAllText(ApplicationDirectory + "\\" + Files)))
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
    }
}