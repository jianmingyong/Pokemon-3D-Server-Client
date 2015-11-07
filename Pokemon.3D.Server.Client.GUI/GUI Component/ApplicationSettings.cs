using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Aragas.Core.Data;
using Pokemon_3D_Server_Core;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Worlds;

namespace Pokemon_3D_Server_Client_GUI
{
    /// <summary>
    /// Class containing Application Setting GUI Component.
    /// </summary>
    public partial class ApplicationSettings : Form
    {
        private List<Settings> Setting = new List<Settings>();

        /// <summary>
        /// GUI Component Start Point.
        /// </summary>
        public ApplicationSettings()
        {
            InitializeComponent();
        }

        private void ShowSetting()
        {
            Setting.Add(new Settings("CheckForUpdate", Core.Setting.CheckForUpdate.ToString().ToLower(), "To allow application to check for update upon launch.\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("GeneratePublicIP", Core.Setting.GeneratePublicIP.ToString().ToLower(), "To allow application to update IP address upon launch.\nSyntax: Boolean: true, false"));

            Setting.Add(new Settings("IPAddress", Core.Setting.IPAddress, "Public IP address of your server.\nSyntax: Valid IPv4 address."));
            Setting.Add(new Settings("Port", Core.Setting.Port.ToString(), "The port to use on your server.\nSyntax: Integer: Between 0 to 65535 inclusive."));
            Setting.Add(new Settings("ServerName", Core.Setting.ServerName, "The server name to display to public.\nSyntax: String."));
            Setting.Add(new Settings("ServerMessage", Core.Setting.ServerMessage, "The server message to display when a player select a server.\nSyntax: String."));
            Setting.Add(new Settings("WelcomeMessage", Core.Setting.WelcomeMessage, "The server message to display when a player joins a server.\nSyntax: String."));
            Setting.Add(new Settings("GameMode", string.Join(",", Core.Setting.GameMode), "The GameMode that player should play in order to join the server.\nSyntax: String. You may insert multiple gamemode by adding a comma seperator on each gamemode name."));
            Setting.Add(new Settings("MaxPlayers", Core.Setting.MaxPlayers.ToString(), "The maximum amount of player in the server that can join.\nSyntax: Integer: -1: Unlimited."));
            Setting.Add(new Settings("OfflineMode", Core.Setting.OfflineMode.ToString().ToLower(), "The ability for offline profile player to join the server.\nSyntax: Boolean: true, false"));

            Setting.Add(new Settings("SCONEnable", Core.Setting.SCONEnable.ToString().ToLower(), "Enable SCON.\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("SCONPort", Core.Setting.SCONPort.ToString(), "The port for SCON Listener. Please be unique and don't be same as Pokemon Listener Port.\nSyntax: Integer: Between 0 to 65535 inclusive."));
            Setting.Add(new Settings("SCONPassword", Core.Setting._SCONPassword, "The password for the SCON to connect.\nSyntax: String. Please do not insert password that contains your personal infomation."));

            Setting.Add(new Settings("Season", Core.Setting.Season.ToString(), "To set server default season.\nSyntax: Integer: Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2 | SeasonMonth = -3"));
            Setting.Add(new Settings("Weather", Core.Setting.Weather.ToString(), "To set server default weather.\nSyntax: Integer: Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 | WeatherSeason = -3 | Real World Weather = -4"));
            Setting.Add(new Settings("DoDayCycle", Core.Setting.DoDayCycle.ToString().ToLower(), "To allow the server to update day cycle.\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("SeasonMonth", Core.Setting.SeasonMonth.SeasonData, "To set the season based on local date. Must set Season = -3\nSyntax: Integer: Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2\nYou may insert more than one season by separating it with a comma."));
            Setting.Add(new Settings("WeatherSeason", Core.Setting.WeatherSeason.WeatherData, "To set the weather based on server season. Must set Weather = -3\nSyntax: Integer: Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 | Real World Weather = -4\nYou may insert more than one weather by separating it with a comma."));
            Setting.Add(new Settings("DefaultWorldCountry", Core.Setting.DefaultWorldCountry, "To set the default country for real world weather.\nSyntax: String. Valid Country name / City name. No fancy character. Use Default A-Z a-z letter."));

            Setting.Add(new Settings("NoPingKickTime", Core.Setting.NoPingKickTime.ToString(), "To kick player out if there are no valid ping for n amount of seconds.\nSyntax: Integer: -1 to disable it."));
            Setting.Add(new Settings("AFKKickTime", Core.Setting.AFKKickTime.ToString(), "To kick player out if there are no valid activity for n amount of seconds.\nSyntax: Integer: -1 to disable it."));
            Setting.Add(new Settings("AutoRestartTime", Core.Setting.AutoRestartTime.ToString(), "To automatically restart the server after n seconds. Disable PvP and trade features for the last 5 minutes of the countdown.\nSyntax: Integer: -1 to disable it."));

            Setting.Add(new Settings("BlackList", Core.Setting.BlackList.ToString().ToLower(), "To allow using blacklist feature.\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("IPBlackList", Core.Setting.IPBlackList.ToString().ToLower(), "To allow using ipblacklist feature.\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("WhiteList", Core.Setting.WhiteList.ToString().ToLower(), "To allow using whitelist feature.\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("OperatorList", Core.Setting.OperatorList.ToString().ToLower(), "To allow using operator feature.\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("MuteList", Core.Setting.MuteList.ToString().ToLower(), "To allow using mute feature.\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("OnlineSettingList", Core.Setting.OnlineSettingList.ToString().ToLower(), "To allow using online setting feature.\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("SwearInfractionList", Core.Setting.SwearInfractionList.ToString().ToLower(), "To allow using swear infraction feature.\nSyntax: Boolean: true, false"));

            Setting.Add(new Settings("SwearInfractionCap", Core.Setting.SwearInfractionCap.ToString(), "Amount of infraction points before the first mute.\nSyntax: Integer: -1 to disable."));
            Setting.Add(new Settings("SwearInfractionReset", Core.Setting.SwearInfractionReset.ToString(), "Amount of days before it expire the infraction count.\nSyntax: Integer: -1 to disable."));

            Setting.Add(new Settings("CustomChannels", string.Join(",", Core.Setting.CustomChannels), "List of custom channels for the server.\nSyntax: String."));
            Setting.Add(new Settings("SpamResetDuration", Core.Setting.SpamResetDuration.ToString(), "Amount of seconds for the user to send the same word again.\nSyntax: Integer: -1 to disable."));

            Setting.Add(new Settings("LoggerInfo", Core.Setting.LoggerInfo.ToString().ToLower(), "To log server information.\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("LoggerWarning", Core.Setting.LoggerWarning.ToString().ToLower(), "To log server warning including ex exception.\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("LoggerDebug", Core.Setting.LoggerDebug.ToString().ToLower(), "To log server package data (Lag might happen if turn on).\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("LoggerChat", Core.Setting.LoggerChat.ToString().ToLower(), "To log server chat message.\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("LoggerPM", Core.Setting.LoggerPM.ToString().ToLower(), "To log server private chat message. (Actual Private Message content is not logged)\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("LoggerServer", Core.Setting.LoggerServer.ToString().ToLower(), "To log server message.\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("LoggerTrade", Core.Setting.LoggerTrade.ToString().ToLower(), "To log trade request. (Actual Trade Request content is not logged)\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("LoggerPvP", Core.Setting.LoggerPvP.ToString().ToLower(), "To log pvp request. (Actual PvP Request content is not logged)\nSyntax: Boolean: true, false"));
            Setting.Add(new Settings("LoggerCommand", Core.Setting.LoggerCommand.ToString().ToLower(), "To log server command usage. (Debug Commands are not logged)\nSyntax: Boolean: true, false"));

            ObjectListView1.AddObjects(Setting);
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Setting.Count; i++)
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
                    case "SCONEnable":
                        try
                        {
                            Core.Setting.SCONEnable = bool.Parse(((Settings)ObjectListView1.GetModelObject(i)).Value);
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "SCONPort":
                        try
                        {
                            Core.Setting.SCONPort = ((Settings)ObjectListView1.GetModelObject(i)).Value.ToUshort();
                        }
                        catch (Exception ex)
                        {
                            ex.CatchError();
                        }
                        break;
                    case "SCONPassword":
                        try
                        {
                            Core.Setting._SCONPassword = ((Settings)ObjectListView1.GetModelObject(i)).Value;
                            Core.Setting.SCONPassword = new PasswordStorage(((Settings)ObjectListView1.GetModelObject(i)).Value);
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
