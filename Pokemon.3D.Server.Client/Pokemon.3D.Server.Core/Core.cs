using Pokemon_3D_Server_Core.Commands;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Network;
using Pokemon_3D_Server_Core.Packages;
using Pokemon_3D_Server_Core.Players;
using Pokemon_3D_Server_Core.Rcon.Players;
using Pokemon_3D_Server_Core.Settings;
using Pokemon_3D_Server_Core.Worlds;

namespace Pokemon_3D_Server_Core
{
    /// <summary>
    /// Class containing all important features.
    /// </summary>
    public class Core
    {
        /// <summary>
        /// Get/Set Comamnd List.
        /// </summary>
        public static CommandCollection Command { get; set; } = new CommandCollection();

        /// <summary>
        /// Get/Set Logger.
        /// </summary>
        public static LoggerCollection Logger { get; set; } = new LoggerCollection();

        /// <summary>
        /// Get/Set Server.
        /// </summary>
        public static ServerClient Server { get; set; } = new ServerClient();

        /// <summary>
        /// Get/Set Package Handler.
        /// </summary>
        public static PackageHandler Package { get; set; } = new PackageHandler();

        /// <summary>
        /// Get/Set Player Collection.
        /// </summary>
        public static PlayerCollection Player { get; set; } = new PlayerCollection();

        /// <summary>
        /// Get/Set Rcon Player Collection.
        /// </summary>
        public static RconPlayerCollection RconPlayer { get; set; } = new RconPlayerCollection();

        /// <summary>
        /// Get/Set Setting.
        /// </summary>
        public static Setting Setting { get; set; } = new Setting();

        /// <summary>
        /// Get/Set World.
        /// </summary>
        public static World World { get; set; } = new World();
    }
}
