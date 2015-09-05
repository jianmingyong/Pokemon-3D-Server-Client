using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Global
{
    /// <summary>
    /// Class containing Settings
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Class containing General Client Setting.
        /// </summary>
        public class Main
        {
            /// <summary>
            /// Get Startup Time.
            /// </summary>
            public static DateTime StartTime { get; set; }

            /// <summary>
            /// Get Application Version.
            /// </summary>
            public static string ApplicationVersion { get; set; }

            /// <summary>
            /// Get Protocol Version.
            /// </summary>
            public static string ProtocolVersion { get; set; }

            /// <summary>
            /// Get/Set Check For Update.
            /// </summary>
            public static bool CheckForUpdate { get; set; }

            /// <summary>
            /// Get/Set Generate Public IP.
            /// </summary>
            public static bool GeneratePublicIP { get; set; }
        }

        /// <summary>
        /// Class containing Server Property.
        /// </summary>
        public class ServerProperty
        {
            [JsonIgnore]
            private static IPAddress _IPAddress;
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

            [JsonIgnore]
            private static int _Port;
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
            public static string ServerName { get; set; }

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
            public static List<string> GameMode { get; set; }

            [JsonIgnore]
            private static int _MaxPlayers;
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
            public static bool OfflineMode { get; set; }
        }

        /// <summary>
        /// Class containing Advanced Server Property.
        /// </summary>
        public class AdvancedServerProperty
        {
            /// <summary>
            /// Class Containing World Property.
            /// </summary>
            public class World
            {
                [JsonIgnore]
                private static int _Season;
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

                [JsonIgnore]
                private static int _Weather;
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
                public static bool DoDayCycle { get; set; }


            }

            /// <summary>
            /// World Setting
            /// </summary>
            [JsonProperty(propertyName: "World", DefaultValueHandling = DefaultValueHandling.Include, Required = Required.AllowNull)]
            public static World WorldSetting { get; set; }
        }

        /// <summary>
        /// Get/Set Application Directory
        /// </summary>
        [JsonIgnore]
        public static string ApplicationDirectory { get; set; }

        /// <summary>
        /// General Setting
        /// </summary>
        [JsonProperty(propertyName: "Pokémon 3D Server Client Setting File", DefaultValueHandling = DefaultValueHandling.Include, Required = Required.AllowNull)]
        public static Main GeneralSetting { get; set; }

        /// <summary>
        /// Server Client Setting
        /// </summary>
        [JsonProperty(propertyName: "Main Server Property", DefaultValueHandling = DefaultValueHandling.Include, Required = Required.AllowNull)]
        public static ServerProperty ServerClientProperty { get; set; }

        /// <summary>
        /// Advanced Server Client Setting
        /// </summary>
        [JsonProperty(propertyName: "Advanced Server Property", DefaultValueHandling = DefaultValueHandling.Include, Required = Required.AllowNull)]
        public static AdvancedServerProperty AdvancedServerClientProperty { get; set; }
    }
}
