using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Global
{
    /// <summary>
    /// Public Class Settings
    /// </summary>
    public class Settings
    {

        /// <summary>
        /// Get/Set Application Directory.
        /// </summary>
        [JsonIgnore]
        public static string ApplicationDirectory { get; set; }

        /// <summary>
        /// Get/Set Startup Time.
        /// </summary>
        public static DateTime StartTime { get; set; }

        /// <summary>
        /// Get Application Version.
        /// </summary>
        public static readonly string ApplicationVersion = Environment.Version.ToString();

        /// <summary>
        /// Get Protocol Version.
        /// </summary>
        public static readonly string ProtocolVersion = "0.5";

        /// <summary>
        /// Get/Set Check For Update.
        /// </summary>
        public static bool CheckForUpdate { get; set; }

        /// <summary>
        /// Get/Set Generate Public IP.
        /// </summary>
        public static bool GeneratePublicIP { get; set; }
    }
}
