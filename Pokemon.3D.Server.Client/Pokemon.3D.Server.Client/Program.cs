using System;
using Pokemon_3D_Server_Core;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Settings;

namespace Pokémon_3D_Server_Client
{
    /// <summary>
    /// Class containing the Main Access point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main Access Point.
        /// </summary>
        /// <param name="args">Program Start Argument.</param>
        public static void Main(string[] args)
        {
            // Add Handler
            QueueMessage.AddMessage += QueueMessage_AddMessage;

            // Setup Settings.
            Core.Setting.Setup();

            // Change Console Title
            Console.Title = @"Pokémon 3D Server Client | Player online: 0 / " + Core.Setting.MaxPlayers;
            

            // Setup Settings
            Core.Setting.ApplicationDirectory = Environment.CurrentDirectory;
            if (Core.Setting.Load())
            {
                Core.Setting.Save();
            }
            else
            {
                Core.Setting.Save();
                Environment.Exit(0);
            }

            // Setup Server
            Core.Server.Start();

            Console.Read();
        }

        private static void QueueMessage_AddMessage(object myObject, MessageEventArgs myArgs)
        {
            Console.WriteLine(myArgs.OutputMessage);
        }
    }
}
