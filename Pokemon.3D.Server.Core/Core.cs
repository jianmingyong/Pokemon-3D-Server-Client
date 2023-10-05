using System;
using Pokemon_3D_Server_Core.Server_Client_Listener.Commands;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Settings;
using Pokemon_3D_Server_Core.Shared.jianmingyong;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core
{
    /// <summary>
    /// Class containing all important features.
    /// </summary>
    public class Core
    {
        /// <summary>
        /// Get Setting.
        /// </summary>
        public static Setting Setting { get; private set; }

        /// <summary>
        /// Get Logger.
        /// </summary>
        public static LoggerCollection Logger { get; private set; }

        /// <summary>
        /// Get Updater.
        /// </summary>
        public static Updater Updater { get; private set; }

        /// <summary>
        /// Get Pokemon 3D Listener.
        /// </summary>
        public static Server_Client_Listener.Servers.Listener Listener { get; private set; }

        /// <summary>
        /// Get RCON Listener.
        /// </summary>
        public static RCON_Client_Listener.Servers.Listener RCONListener { get; private set; }

        /// <summary>
        /// Get Comamnd List.
        /// </summary>
        public static CommandCollection Command { get; private set; }

        #region Pokemon 3D Listener
        /// <summary>
        /// Get Player Collection.
        /// </summary>
        public static Server_Client_Listener.Players.PlayerCollection Player { get; } = new Server_Client_Listener.Players.PlayerCollection();

        /// <summary>
        /// Get World.
        /// </summary>
        public static Server_Client_Listener.Worlds.World World { get; } = new Server_Client_Listener.Worlds.World();
        #endregion Pokemon 3D Listener

        #region RCON Listener
        /// <summary>
        /// Get RCON Player Collection.
        /// </summary>
        public static RCON_Client_Listener.Players.PlayerCollection RCONPlayer { get; } = new RCON_Client_Listener.Players.PlayerCollection();

        /// <summary>
        /// Get RCON Upload Queue.
        /// </summary>
        public static RCON_Client_Listener.Uploader.UploaderQueue RCONUploadQueue { get; } = new RCON_Client_Listener.Uploader.UploaderQueue();
        #endregion

        #region RCON GUI Listener
        /// <summary>
        /// Get RCON Listener.
        /// </summary>
        public static RCON_GUI_Client_Listener.Servers.Listener RCONGUIListener { get; set; }

        /// <summary>
        /// Get RCON GUI Download Queue.
        /// </summary>
        public static RCON_GUI_Client_Listener.Downloader.DownloaderQueue RCONGUIDownloadQueue { get; } = new RCON_GUI_Client_Listener.Downloader.DownloaderQueue();
        #endregion RCON GUI Listener

        /// <summary>
        /// Server Main Entry Point - Initialize as many things as possible here.
        /// Order is important here, any additional initialization should be place at the bottom.
        /// </summary>
        /// <param name="Directory">Start Directory.</param>
        public static void Start(string Directory)
        {
            try
            {
                // Initialize Setting
                Setting = new Setting(Directory);

                // Initialize Logger.
                Logger = new LoggerCollection();
                Logger.Start();

                if (Setting.Load())
                {
                    Setting.Save();
                }
                else
                {
                    Setting.Save();
                    Environment.Exit(0);
                    return;
                }

                // Initialize Updater
                if (Setting.CheckForUpdate)
                {
                    Updater = new Updater();
                    Updater.Update();
                }

                if (Setting.MainEntryPoint == Setting.MainEntryPointType.jianmingyong_Server)
                {
                    // Initialize Listener.
                    Listener = new Server_Client_Listener.Servers.Listener();
                    Listener.Start();

                    // Initialize RCONListener.
                    if (Setting.RCONEnable)
                    {
                        RCONListener = new RCON_Client_Listener.Servers.Listener();
                        RCONListener.Start();
                    }
                }

                // Initialize Command.
                Command = new CommandCollection();
                Command.AddCommand();
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }

        /// <summary>
        /// Dispose all background worker for the thread to dispose.
        /// </summary>
        public static void Dispose()
        {
            if (Listener != null) Listener.Dispose();
            if (RCONListener != null) RCONListener.Dispose();
            if (Logger != null) Logger.Dispose();
        }
    }
}