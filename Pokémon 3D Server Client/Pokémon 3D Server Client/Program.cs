using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global;

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
            Console.Title = @"Pokémon 3D Server Client | Player online: 0 / " + Settings.MaxPlayers;
            // Add Handler
            QueueMessage.AddMessage += QueueMessage_AddMessage;

            QueueMessage.Add("", MessageEventArgs.LogType.Info);

            Console.Read();
        }

        private static void QueueMessage_AddMessage(object myObject, MessageEventArgs myArgs)
        {
            Console.WriteLine(myArgs.OutputMessage);
        }
    }
}
