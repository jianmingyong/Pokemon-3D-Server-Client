using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Worlds;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data
{
    /// <summary>
    /// Class containing OnlineSetting data
    /// </summary>
    public class OnlineSetting : CommonData
    {
        /// <summary>
        /// Get Last Updated
        /// </summary>
        public DateTime LastUpdated { get; } = DateTime.Now;

        /// <summary>
        /// Get/Set World Season
        /// </summary>
        public int Season { get; set; } = (int)World.SeasonType.Nothing;

        /// <summary>
        /// Get/Set World Weather
        /// </summary>
        public int Weather { get; set; } = (int)World.WeatherType.Nothing;

        /// <summary>
        /// Get/Set Current World Season
        /// </summary>
        public int CurrentWorldSeason { get; set; }

        /// <summary>
        /// Get/Set Current World Weather
        /// </summary>
        public int CurrentWorldWeather { get; set; }

        /// <summary>
        /// Get/Set Last World Update Time
        /// </summary>
        public DateTime LastWorldUpdate { get; set; }

        /// <summary>
        /// Get/Set MuteList data
        /// </summary>
        public List<MuteList> MuteListData { get; set; } = new List<MuteList>();

        /// <summary>
        /// New OnlineSetting
        /// </summary>
        /// <param name="Name">Player Name</param>
        /// <param name="GameJoltID">Player GameJolt ID</param>
        public OnlineSetting(string Name, int GameJoltID)
        {
            this.Name = Name;
            this.GameJoltID = GameJoltID;
            Load();
        }

        private void Load()
        {
            if (HaveSettingFile(GameJoltID))
            {
                try
                {
                    using (JsonTextReader Reader = new JsonTextReader(new StringReader(File.ReadAllText(Core.Setting.ApplicationDirectory + "\\Data\\OnlineSetting\\" + GameJoltID.ToString() + ".json"))))
                    {
                        Reader.DateParseHandling = DateParseHandling.DateTime;
                        Reader.FloatParseHandling = FloatParseHandling.Double;

                        int StartObjectDepth = -1;
                        string ObjectPropertyName = null;
                        string PropertyName = null;
                        string TempPropertyName = null;

                        string Name = null;
                        int GameJoltID = -1;
                        string Reason = null;
                        DateTime StartTime = DateTime.Now;
                        int Duration = 0;

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
                                if (StartObjectDepth == 1 && string.Equals(ObjectPropertyName, "MuteListData", StringComparison.OrdinalIgnoreCase))
                                {
                                    MuteListData.Add(new MuteList(Name, GameJoltID, Reason, StartTime, Duration));
                                    Name = null;
                                    GameJoltID = -1;
                                    Reason = null;
                                    StartTime = DateTime.Now;
                                    Duration = 0;
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

                            if (StartObjectDepth == 1 && string.Equals(ObjectPropertyName, "World Property", StringComparison.OrdinalIgnoreCase))
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
                                            Core.Logger.Log("OnlineSetting.cs: Player " + this.GameJoltID.ToString() + ": World Property.Season does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("OnlineSetting.cs: Player " + this.GameJoltID.ToString() + ": World Property.Weather does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                            else if (StartObjectDepth == 1 && string.Equals(ObjectPropertyName, "MuteListData", StringComparison.OrdinalIgnoreCase))
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
                                            Core.Logger.Log("OnlineSetting.cs: Player " + this.GameJoltID.ToString() + ": MuteListData.Name does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("OnlineSetting.cs: Player " + this.GameJoltID.ToString() + ": MuteListData.GameJoltID does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("OnlineSetting.cs: Player " + this.GameJoltID.ToString() + ": MuteListData.Reason does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("OnlineSetting.cs: Player " + this.GameJoltID.ToString() + ": MuteListData.StartTime does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
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
                                            Core.Logger.Log("OnlineSetting.cs: Player " + this.GameJoltID.ToString() + ": MuteListData.Duration does not match the require type. Default value will be used.", Logger.LogTypes.Warning);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.CatchError();
                }
            }
        }

        /// <summary>
        /// Save Player Online Setting
        /// </summary>
        public void Save()
        {
            try
            {
                if (!Directory.Exists(Core.Setting.ApplicationDirectory + "\\Data\\UserSetting"))
                {
                    Directory.CreateDirectory(Core.Setting.ApplicationDirectory + "\\Data\\UserSetting");
                }

                string ReturnString = "";
                if (MuteListData.Count > 0)
                {
                    foreach (MuteList Data in MuteListData)
                    {
                        ReturnString += string.Format(@"        {{
""Name"": ""{0}"",
""GameJoltID"": {1},
""Reason"": ""{2}"",
""StartTime"": ""{3}"",
""MuteDuration"": {4}
        }},",
        Data.Name,
        Data.GameJoltID.ToString(),
        Data.Reason,
        Data.StartTime.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffK"),
        Data.Duration.ToString());
                    }
                }

                File.WriteAllText(Core.Setting.ApplicationDirectory + "\\Data\\UserSetting\\" + GameJoltID.ToString() + ".json", string.Format(@"{{
    ""Pokémon 3D Server Client Setting File"":
    {{
        ""Name"": ""{0}"",
        ""GameJoltID"": {1},
        ""LastUpdate"": ""{2}""
    }},

    ""World Property"":
    {{
        ""Season"": {3},
        ""Weather"": {4}
    }}

    ""MuteListData"":
    [
{5}
    ]
}}",
Name,
GameJoltID.ToString(),
LastUpdated.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffK"),
Season.ToString(),
Weather.ToString(),
ReturnString), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }

        private bool HaveSettingFile(int GameJoltID)
        {
            try
            {
                if (File.Exists(Core.Setting.ApplicationDirectory + "\\Data\\OnlineSetting\\" + GameJoltID.ToString() + ".json"))
                {
                    string FileContent = File.ReadAllText(Core.Setting.ApplicationDirectory + "\\Data\\OnlineSetting\\" + GameJoltID.ToString() + ".json");
                    if (string.IsNullOrWhiteSpace(FileContent))
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
    }
}