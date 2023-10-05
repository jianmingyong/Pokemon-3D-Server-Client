using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Pokemon_3D_Server_Core;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Worlds;
using Pokemon_3D_Server_Core.Server_Client_Listener.Settings;

namespace Pokemon_3D_Server_Client_GUI
{
    /// <summary>
    /// Class containing Application Setting GUI Component.
    /// </summary>
    public partial class ApplicationSettings : Form
    {
        private List<Settings> SettingToDisplay = new List<Settings>();

        /// <summary>
        /// GUI Component Start Point.
        /// </summary>
        public ApplicationSettings()
        {
            InitializeComponent();
        }

        private void ShowSetting()
        {
            SettingToDisplay = new List<Settings>
            {
                new Settings
                (
                    "CheckForUpdate",
                    Core.Setting.CheckForUpdate.ToString().ToLower(),
                    "To allow or disallow the application to check for update upon launch.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "GeneratePublicIP",
                    Core.Setting.GeneratePublicIP.ToString().ToLower(),
                    "To allow or disallow the application to update the IP address upon launch.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "MainEntryPoint",
                    ((int)Core.Setting.MainEntryPoint).ToString(),
                    "The main entry point of the server console.\nRequired Syntax: Integer.\njianmingyong Server Instance = 0 | RCON = 1"
                ),

                new Settings
                (
                    "IPAddress",
                    Core.Setting.IPAddress,
                    "Public/External IP address of your server.\nRequired Syntax: Valid IPv4 address."
                ),

                new Settings
                (
                    "Port",
                    Core.Setting.Port.ToString(),
                    "The port to use on your server.\nRequired Syntax: Integer between 0 to 65535 inclusive.\nPort cannot be the same as SCON and RCON."
                ),

                new Settings
                (
                    "ServerName",
                    Core.Setting.ServerName,
                    "The server name to be display to public.\nRequired Syntax: String."
                ),

                new Settings
                (
                    "ServerMessage",
                    Core.Setting.ServerMessage,
                    "The server message to display when a player select a server.\nRequired Syntax: String."
                ),

                new Settings
                (
                    "WelcomeMessage",
                    Core.Setting.WelcomeMessage,
                    "The server message to display when a player joins a server.\nRequired Syntax: String."
                ),

                new Settings
                (
                    "GameMode",
                    string.Join(", ",Core.Setting.GameMode),
                    "The GameMode player should play in order to join the server.\nRequired Syntax: String.\nYou may insert multiple gamemode by adding a comma seperator on each gamemode name."
                ),

                new Settings
                (
                    "MaxPlayers",
                    Core.Setting.MaxPlayers.ToString(),
                    "The maximum amount of player in the server that can join.\nRequired Syntax: Integer.\n-1 = Unlimited Players. (Technically not unlimited but the bigggest amount the game can handle.)"
                ),

                new Settings
                (
                    "OfflineMode",
                    Core.Setting.OfflineMode.ToString().ToLower(),
                    "To allow or disallow offline save player joins the server.\nRequired Syntax: Boolean.\nIt will be allowed if other GameMode other than default server is allowed to join the server."
                ),

                new Settings
                (
                    "Season",
                    Core.Setting.Season.ToString(),
                    "To set server default season.\nRequired Syntax: Integer.\nWinter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2 | SeasonMonth = -3"
                ),

                new Settings
                (
                    "Weather",
                    Core.Setting.Weather.ToString(),
                    "To set server default weather.\nRequired Syntax: Integer.\nClear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 | WeatherSeason = -3"
                ),

                new Settings
                (
                    "DoDayCycle",
                    Core.Setting.DoDayCycle.ToString().ToLower(),
                    "To allow or disallow the server to update day and night cycle.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "TimeOffset",
                    Core.Setting.TimeOffset.ToString(),
                    "Offset the time in the server.\nRequired Syntax: Integer.\nThe time offset is counted by seconds. 60 = 1 minute time difference from your local time."
                ),

                new Settings
                (
                    "SeasonMonth",
                    Core.Setting.SeasonMonth.SeasonData,
                    "To set the season based on local date. Must set Season = -3\nRequired Syntax: Integer.\nWinter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2"
                ),

                new Settings
                (
                    "WeatherSeason",
                    Core.Setting.WeatherSeason.WeatherData,
                    "To set the weather based on server season. Must set Weather = -3\nRequired Syntax: Integer.\nClear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2"
                ),

                new Settings
                (
                    "DefaultWorldCountry",
                    Core.Setting.DefaultWorldCountry,
                    "To set the default country for real world weather.\nRequired Syntax: String.\nValid Country name / City name. No fancy character. Use Default A-Z a-z letter."
                ),

                new Settings
                (
                    "NoPingKickTime",
                    Core.Setting.NoPingKickTime.ToString(),
                    "To kick player out if there are no valid ping for n amount of seconds.\nRequired Syntax: Integer.\n-1 to disable it."
                ),

                new Settings
                (
                    "AFKKickTime",
                    Core.Setting.AFKKickTime.ToString(),
                    "To kick player out if there are no valid activity for n amount of seconds.\nRequired Syntax: Integer.\n-1 to disable it."
                ),

                new Settings
                (
                    "AutoRestartTime",
                    Core.Setting.AutoRestartTime.ToString(),
                    "To automatically restart the server after n seconds. Disable PvP and trade features for the last 5 minutes of the countdown.\nRequired Syntax: Integer.\n-1 to disable it."
                ),

                new Settings
                (
                    "BlackList",
                    Core.Setting.BlackList.ToString().ToLower(),
                    "To allow or disallow using Blacklist feature.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "IPBlackList",
                    Core.Setting.IPBlackList.ToString().ToLower(),
                    "To allow or disallow using IPBlacklist feature.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "WhiteList",
                    Core.Setting.WhiteList.ToString().ToLower(),
                    "To allow or disallow using WhiteList feature.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "OperatorList",
                    Core.Setting.OperatorList.ToString().ToLower(),
                    "To allow or disallow using OperatorList feature.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "MuteList",
                    Core.Setting.MuteList.ToString().ToLower(),
                    "To allow or disallow using MuteList feature.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "OnlineSettingList",
                    Core.Setting.OnlineSettingList.ToString().ToLower(),
                    "To allow or disallow using OnlineSettingList feature.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "SwearInfractionList",
                    Core.Setting.SwearInfractionList.ToString().ToLower(),
                    "To allow or disallow using SwearInfractionList feature.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "SwearInfractionCap",
                    Core.Setting.SwearInfractionCap.ToString(),
                    "Amount of infraction points before the first mute.\nRequired Syntax: Integer.\n-1 to disable."
                ),

                new Settings
                (
                    "SwearInfractionReset",
                    Core.Setting.SwearInfractionReset.ToString(),
                    "Amount of days before it expire the infraction count.\nRequired Syntax: Integer.\n-1 to disable."
                ),

                new Settings
                (
                    "AllowChatInServer",
                    Core.Setting.AllowChatInServer.ToString().ToLower(),
                    "To allow or disallow player to chat in the server.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "AllowChatChannels",
                    Core.Setting.AllowChatChannels.ToString().ToLower(),
                    "To allow or disallow player to use chat channels in the server.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "CustomChannels",
                    string.Join(", ",Core.Setting.CustomChannels),
                    "List of custom channels for the server.\nRequired Syntax: String."
                ),

                new Settings
                (
                    "SpamResetDuration",
                    Core.Setting.SpamResetDuration.ToString(),
                    "Amount of seconds for the user to send the same word again.\nRequired Syntax: Integer.\n-1 to disable."
                ),

                new Settings
                (
                    "AllowPvP",
                    Core.Setting.AllowPvP.ToString().ToLower(),
                    "To allow or disallow player to PvP in the server.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "AllowPvPValidation",
                    Core.Setting.AllowPvPValidation.ToString().ToLower(),
                    "To allow or disallow PvP Validation system.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "AllowTrade",
                    Core.Setting.AllowTrade.ToString().ToLower(),
                    "To allow or disallow player to Trade in the server.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "LoggerInfo",
                    Core.Setting.LoggerInfo.ToString().ToLower(),
                    "To log server information.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "LoggerWarning",
                    Core.Setting.LoggerWarning.ToString().ToLower(),
                    "To log server warning including ex exception.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "LoggerChat",
                    Core.Setting.LoggerChat.ToString().ToLower(),
                    "To log server chat message.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "LoggerPM",
                    Core.Setting.LoggerPM.ToString().ToLower(),
                    "To log server private chat message. (Actual Private Message content is not logged)\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "LoggerServer",
                    Core.Setting.LoggerServer.ToString().ToLower(),
                    "To log server message.\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "LoggerTrade",
                    Core.Setting.LoggerTrade.ToString().ToLower(),
                    "To log trade request. (Actual Trade Request content is not logged)\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "LoggerPvP",
                    Core.Setting.LoggerPvP.ToString().ToLower(),
                    "To log pvp request. (Actual PvP Request content is not logged)\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "LoggerCommand",
                    Core.Setting.LoggerCommand.ToString().ToLower(),
                    "To log server command usage. (Debug Commands are not logged)\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "RCONEnable",
                    Core.Setting.RCONEnable.ToString().ToLower(),
                    "Enable RCON\nRequired Syntax: Boolean."
                ),

                new Settings
                (
                    "RCONPort",
                    Core.Setting.RCONPort.ToString(),
                    "The port for RCON Listener.\nRequired Syntax: Integer between 0 to 65535 inclusive."
                ),

                new Settings
                (
                    "RCONPassword",
                    Core.Setting.RCONPassword,
                    "The password for the RCON to connect.\nRequired Syntax: String."
                ),
            };

            ObjectListView1.AddObjects(SettingToDisplay);
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SettingToDisplay.Count; i++)
            {
                switch (((Settings)ObjectListView1.GetModelObject(i)).Setting)
                {
                    case "CheckForUpdate":
                        try
                        {
                            Core.Setting.CheckForUpdate = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "GeneratePublicIP":
                        try
                        {
                            Core.Setting.GeneratePublicIP = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "MainEntryPoint":
                        try
                        {
                            if (((Settings)ObjectListView1.GetModelObject(i)).Value.ToInt() == (int)Setting.MainEntryPointType.jianmingyong_Server)
                            {
                                Core.Setting.MainEntryPoint = Setting.MainEntryPointType.jianmingyong_Server;
                            }
                            else if (((Settings)ObjectListView1.GetModelObject(i)).Value.ToInt() == (int)Setting.MainEntryPointType.Rcon)
                            {
                                Core.Setting.MainEntryPoint = Setting.MainEntryPointType.Rcon;
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "IPAddress":
                        try
                        {
                            Core.Setting.IPAddress = ((Settings)ObjectListView1.GetModelObject(i)).Value;
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "Port":
                        try
                        {
                            Core.Setting.Port = ((Settings)ObjectListView1.GetModelObject(i)).Value.ToInt();
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "ServerName":
                        try
                        {
                            Core.Setting.ServerName = ((Settings)ObjectListView1.GetModelObject(i)).Value;
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "ServerMessage":
                        try
                        {
                            Core.Setting.ServerMessage = ((Settings)ObjectListView1.GetModelObject(i)).Value;
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "WelcomeMessage":
                        try
                        {
                            Core.Setting.WelcomeMessage = ((Settings)ObjectListView1.GetModelObject(i)).Value;
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "GameMode":
                        try
                        {
                            Core.Setting.GameMode = ((Settings)ObjectListView1.GetModelObject(i)).Value.Split(',').ToList();

                            Core.Setting.GM_Pokemon3D = false;
                            Core.Setting.GM_1YearLater3D = false;
                            Core.Setting.GM_DarkfireMode = false;
                            Core.Setting.GM_German = false;
                            Core.Setting.GM_PokemonGoldSilverRandomLocke = false;
                            Core.Setting.GM_PokemonLostSilver = false;
                            Core.Setting.GM_PokemonSilversSoul = false;
                            Core.Setting.GM_PokemonUniversal3D = false;

                            List<string> Others = new List<string>();

                            for (int a = 0; a < Core.Setting.GameMode.Count; a++)
                            {
                                if (string.Equals(Core.Setting.GameMode[a], "Pokemon 3D", StringComparison.OrdinalIgnoreCase))
                                {
                                    Core.Setting.GM_Pokemon3D = true;
                                }
                                else if (string.Equals(Core.Setting.GameMode[a], "1 Year Later 3D", StringComparison.OrdinalIgnoreCase))
                                {
                                    Core.Setting.GM_1YearLater3D = true;
                                }
                                else if (string.Equals(Core.Setting.GameMode[a], "Darkfire Mode", StringComparison.OrdinalIgnoreCase))
                                {
                                    Core.Setting.GM_DarkfireMode = true;
                                }
                                else if (string.Equals(Core.Setting.GameMode[a], "German", StringComparison.OrdinalIgnoreCase))
                                {
                                    Core.Setting.GM_German = true;
                                }
                                else if (string.Equals(Core.Setting.GameMode[a], "Pokemon Gold&Silver - RandomLocke", StringComparison.OrdinalIgnoreCase))
                                {
                                    Core.Setting.GM_PokemonGoldSilverRandomLocke = true;
                                }
                                else if (string.Equals(Core.Setting.GameMode[a], "Pokemon Lost Silver", StringComparison.OrdinalIgnoreCase))
                                {
                                    Core.Setting.GM_PokemonLostSilver = true;
                                }
                                else if (string.Equals(Core.Setting.GameMode[a], "Pokemon Silver's Soul", StringComparison.OrdinalIgnoreCase))
                                {
                                    Core.Setting.GM_PokemonSilversSoul = true;
                                }
                                else if (string.Equals(Core.Setting.GameMode[a], "Pokemon Universal 3D", StringComparison.OrdinalIgnoreCase))
                                {
                                    Core.Setting.GM_PokemonUniversal3D = true;
                                }
                                else
                                {
                                    Others.Add(Core.Setting.GameMode[a]);
                                }
                            }

                            if (Others.Count > 0)
                            {
                                Core.Setting.GM_Others = string.Join(", ", Others);
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "MaxPlayers":
                        try
                        {
                            Core.Setting.MaxPlayers = ((Settings)ObjectListView1.GetModelObject(i)).Value.ToInt();
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "OfflineMode":
                        try
                        {
                            Core.Setting.OfflineMode = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    
                    case "Season":
                        try
                        {
                            Core.Setting.Season = ((Settings)ObjectListView1.GetModelObject(i)).Value.ToInt();
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "Weather":
                        try
                        {
                            Core.Setting.Weather = ((Settings)ObjectListView1.GetModelObject(i)).Value.ToInt();
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "DoDayCycle":
                        try
                        {
                            Core.Setting.DoDayCycle = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "TimeOffset":
                        try
                        {
                            Core.Setting.TimeOffset = ((Settings)ObjectListView1.GetModelObject(i)).Value.ToInt();
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "SeasonMonth":
                        try
                        {
                            Core.Setting.SeasonMonth = new SeasonMonth(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "WeatherSeason":
                        try
                        {
                            Core.Setting.WeatherSeason = new WeatherSeason(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "DefaultWorldCountry":
                        try
                        {
                            Core.Setting.DefaultWorldCountry = ((Settings)ObjectListView1.GetModelObject(i)).Value;
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "NoPingKickTime":
                        try
                        {
                            Core.Setting.NoPingKickTime = ((Settings)ObjectListView1.GetModelObject(i)).Value.ToInt();
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "AFKKickTime":
                        try
                        {
                            Core.Setting.AFKKickTime = ((Settings)ObjectListView1.GetModelObject(i)).Value.ToInt();
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "AutoRestartTime":
                        try
                        {
                            Core.Setting.AutoRestartTime = ((Settings)ObjectListView1.GetModelObject(i)).Value.ToInt();
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "BlackList":
                        try
                        {
                            Core.Setting.BlackList = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "IPBlackList":
                        try
                        {
                            Core.Setting.IPBlackList = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "WhiteList":
                        try
                        {
                            Core.Setting.WhiteList = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "OperatorList":
                        try
                        {
                            Core.Setting.OperatorList = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "MuteList":
                        try
                        {
                            Core.Setting.MuteList = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "OnlineSettingList":
                        try
                        {
                            Core.Setting.OnlineSettingList = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "SwearInfractionList":
                        try
                        {
                            Core.Setting.SwearInfractionList = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "SwearInfractionCap":
                        try
                        {
                            Core.Setting.SwearInfractionCap = ((Settings)ObjectListView1.GetModelObject(i)).Value.ToInt();
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "SwearInfractionReset":
                        try
                        {
                            Core.Setting.SwearInfractionReset = ((Settings)ObjectListView1.GetModelObject(i)).Value.ToInt();
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "AllowChatInServer":
                        try
                        {
                            Core.Setting.AllowChatInServer = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "AllowChatChannels":
                        try
                        {
                            Core.Setting.AllowChatChannels = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "CustomChannels":
                        try
                        {
                            Core.Setting.CustomChannels = ((Settings)ObjectListView1.GetModelObject(i)).Value.Split(',').ToList();
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "SpamResetDuration":
                        try
                        {
                            Core.Setting.SpamResetDuration = ((Settings)ObjectListView1.GetModelObject(i)).Value.ToInt();
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "AllowPvP":
                        try
                        {
                            Core.Setting.AllowPvP = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "AllowPvPValidation":
                        try
                        {
                            Core.Setting.AllowPvPValidation = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "AllowTrade":
                        try
                        {
                            Core.Setting.AllowTrade = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "LoggerInfo":
                        try
                        {
                            Core.Setting.LoggerInfo = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "LoggerWarning":
                        try
                        {
                            Core.Setting.LoggerWarning = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "LoggerDebug":
                        try
                        {
                            Core.Setting.LoggerDebug = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "LoggerChat":
                        try
                        {
                            Core.Setting.LoggerChat = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "LoggerPM":
                        try
                        {
                            Core.Setting.LoggerPM = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "LoggerServer":
                        try
                        {
                            Core.Setting.LoggerServer = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "LoggerTrade":
                        try
                        {
                            Core.Setting.LoggerTrade = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "LoggerPvP":
                        try
                        {
                            Core.Setting.LoggerPvP = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "LoggerCommand":
                        try
                        {
                            Core.Setting.LoggerCommand = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "RCONEnable":
                        try
                        {
                            Core.Setting.RCONEnable = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "RCONPort":
                        try
                        {
                            Core.Setting.RCONPort = ((Settings)ObjectListView1.GetModelObject(i)).Value.ToInt();
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "RCONPassword":
                        try
                        {
                            Core.Setting.RCONPassword = ((Settings)ObjectListView1.GetModelObject(i)).Value;
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                }
            }

            Close();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ApplicationSettings_Load(object sender, EventArgs e)
        {
            ShowSetting();
        }
    }

    /// <summary>
    /// Class containing Settings Definations.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Get/Set Setting Name.
        /// </summary>
        public string Setting { get; set; }

        /// <summary>
        /// Get/Set Setting Value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Get/Set Setting Remark.
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// New Setting
        /// </summary>
        /// <param name="Setting">Setting Name.</param>
        /// <param name="Value">Default Value.</param>
        /// <param name="Remark">Remark.</param>
        public Settings(string Setting, string Value, string Remark)
        {
            this.Setting = Setting;
            this.Value = Value;
            this.Remark = Remark;
        }
    }
}
