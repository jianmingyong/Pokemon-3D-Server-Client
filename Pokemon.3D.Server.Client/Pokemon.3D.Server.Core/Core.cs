﻿using System;
using Pokemon_3D_Server_Core.Server_Client_Listener.Commands;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using Pokemon_3D_Server_Core.Server_Client_Listener.Rcon.Players;
using Pokemon_3D_Server_Core.Server_Client_Listener.Servers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Settings;
using Pokemon_3D_Server_Core.Server_Client_Listener.Worlds;

namespace Pokemon_3D_Server_Core
{
    /// <summary>
    /// Class containing all important features.
    /// </summary>
    public class Core
    {
        /// <summary>
        /// Get Pokemon 3D Listener
        /// </summary>
        public static Listener Listener { get; set; } = new Listener();

        /// <summary>
        /// Get Logger.
        /// </summary>
        public static LoggerCollection Logger { get; set; } = new LoggerCollection();

        /// <summary>
        /// Get Setting.
        /// </summary>
        public static Setting Setting { get; set; } = new Setting();

        /// <summary>
        /// Get Player Collection.
        /// </summary>
        public static PlayerCollection Player { get; set; } = new PlayerCollection();

        /// <summary>
        /// Get/Set Comamnd List.
        /// </summary>
        public static CommandCollection Command { get; set; } = new CommandCollection();

        /// <summary>
        /// Get/Set Package Handler.
        /// </summary>
        public static PackageHandler Package { get; set; } = new PackageHandler();

        /// <summary>
        /// Get/Set World.
        /// </summary>
        public static World World { get; set; } = new World();

        /// <summary>
        /// Get/Set Rcon Player Collection.
        /// </summary>
        public static RconPlayerCollection RconPlayer { get; set; } = new RconPlayerCollection();

        /// <summary>
        /// Server Main Entry Point - Initialize as many things as possible here.
        /// </summary>
        public static void Start(string Directory)
        {
            try
            {
                // Initialize Logger.
                Logger.Start();

                // Initialize Setting.
                Setting.ApplicationDirectory = Directory;
                Setting.Setup();

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

                Setting.NoPingKickTime = 20;

                // Initialize Listener.
                Listener.Start();

                // Initialize Command.
                Command.AddCommand();
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }

        /// <summary>
        /// Dispose All Server Client Objects.
        /// </summary>
        public static void Dispose()
        {
            
        }
    }
}