﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

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
        public static DateTime StartTime { get; private set; }

        /// <summary>
        /// Get Application Version.
        /// </summary>
        public static string ApplicationVersion { get { return Environment.Version.ToString(); } }

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
        public static IPAddress _IPAddress;
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
                SetIPAddress(value);
            }
        }
        private static async void SetIPAddress(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Task<string> GetPublicIP = Functions.GetPublicIP();
                    string IPAddress = await GetPublicIP;
                    _IPAddress = System.Net.IPAddress.Parse(IPAddress);
                }
                else
                {
                    _IPAddress = System.Net.IPAddress.Parse(value);
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
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
        public static string ServerMessage { get; set; }

        /// <summary>
        /// Get/Set Welcome Message
        /// </summary>
        public static string WelcomeMessage { get; set; }

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
        public static SeasonMonth SeasonMonth { get; set; }

        /// <summary>
        /// Get/Set WeatherSeason
        /// </summary>
        public static WeatherSeason WeatherSeason { get; set; }
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
            QueueMessage.Add("Setting.cs: Save Setting.", MessageEventArgs.LogType.Info);

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
