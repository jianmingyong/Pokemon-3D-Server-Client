﻿using System;
using Pokemon_3D_Server_Core.SCON_Client_Listener.Servers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Commands;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
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
        /// Get SCON Listener
        /// </summary>
        public static SCONListener SCONListener { get; } = new SCONListener();

        /// <summary>
        /// Get Comamnd List.
        /// </summary>
        public static CommandCollection Command { get; } = new CommandCollection();

        /// <summary>
        /// Get Logger.
        /// </summary>
        public static LoggerCollection Logger { get; } = new LoggerCollection();

        /// <summary>
        /// Get Player Collection.
        /// </summary>
        public static PlayerCollection Player { get; } = new PlayerCollection();

        /// <summary>
        /// Get Pokemon 3D Listener
        /// </summary>
        public static Listener Listener { get; } = new Listener();

        /// <summary>
        /// Get Setting.
        /// </summary>
        public static Setting Setting { get; } = new Setting();

        /// <summary>
        /// Get World.
        /// </summary>
        public static World World { get; } = new World();

        /// <summary>
        /// Get Updater.
        /// </summary>
        public static Updater Updater { get; } = new Updater();

        /// <summary>
        /// Server Main Entry Point - Initialize as many things as possible here.
        /// Order is important here, any additional initialization should be place at the bottom.
        /// </summary>
        public static void Start(string Directory)
        {
            try
            {
                Setting.ApplicationDirectory = Directory;

                // Initialize Logger.
                Logger.Start();

                // Initialize Setting.
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

                // Initialize Listener.
                Listener.Start();

                // Initialize SCONListener.
                if (Setting.SCONEnable)
                {
                    SCONListener.Start();
                }

                // Initialize Updater
                if (Setting.CheckForUpdate)
                {
                    Updater.Update();
                }

                // Initialize Command.
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
            Listener.Dispose();
            SCONListener.Dispose();
            Logger.Dispose();
        }
    }
}