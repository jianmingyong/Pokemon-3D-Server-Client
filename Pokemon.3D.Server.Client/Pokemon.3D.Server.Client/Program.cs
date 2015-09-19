using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global;
using Pokemon_3D_Server_Core;

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
            // Change Console Title
            Console.Title = @"Pokémon 3D Server Client | Player online: 0 / " + Core.Setting.MaxPlayers;
            // Add Handler
            //QueueMessage.AddMessage += QueueMessage_AddMessage;

            // Setup Settings
            Core.Setting.ApplicationDirectory = Environment.CurrentDirectory;
            //if (Settings.Load())
            //{
            //    Settings.Save();
            //}
            //else
            //{
            //    Settings.Save();
            //    Environment.Exit(0);
            //}

            // Setup Server
            //ServerClient.Start();

            Console.Read();
        }

        private static void QueueMessage_AddMessage(object myObject, MessageEventArgs myArgs)
        {
            Console.WriteLine(myArgs.OutputMessage);
        }
    }
}
