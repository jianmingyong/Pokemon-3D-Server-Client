using System.Collections.Generic;

namespace Global
{
    /// <summary>
    /// Class containing MapFileList data
    /// </summary>
    public class MapFileList
    {
        /// <summary>
        /// Get/Set GameMode
        /// </summary>
        public List<string> GameMode { get; set; }

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
        public MapFileList(List<string> GameMode,string Path,string Name)
        {
            this.GameMode = GameMode;
            this.Path = Path;
            this.Name = Name;
        }
    }
}
