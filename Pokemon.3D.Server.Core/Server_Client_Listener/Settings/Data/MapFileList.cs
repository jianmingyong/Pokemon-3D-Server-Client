using System.Collections.Generic;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data
{
    /// <summary>
    /// Class containing MapFileList data
    /// </summary>
    public class MapFileList
    {
        /// <summary>
        /// Get/Set Map GameMode
        /// </summary>
        public string GameMode { get; set; }

        /// <summary>
        /// Get/Set Map Path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Get/Set Map Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// New MapFileList
        /// </summary>
        /// <param name="GameMode">GameMode for the map location</param>
        /// <param name="Path">Relative Path for the map</param>
        /// <param name="Name">Name of the map</param>
        public MapFileList(string GameMode, string Path, string Name)
        {
            this.GameMode = GameMode;
            this.Path = Path;
            this.Name = Name;
        }
    }
}