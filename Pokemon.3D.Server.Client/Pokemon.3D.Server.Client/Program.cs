using System;
using System.IO;
using Pokemon_3D_Server_Core;
using Pokemon_3D_Server_Core.Event;
using Pokemon_3D_Server_Core.Modules;

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
            ClientEvent.Update += ClientEvent_Update;

            // Setup Settings.
            Core.Setting.Setup();

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

        private static void ClientEvent_Update(object myObject, ClientEventArgs myArgs)
        {
            try
            {
                if (myArgs.Type == ClientEvent.Types.Logger)
                {
                    Console.WriteLine(myArgs.Output);

                    if (!Directory.Exists(Core.Setting.ApplicationDirectory + "\\Logger"))
                    {
                        Directory.CreateDirectory(Core.Setting.ApplicationDirectory + "\\Logger");
                    }

                    File.AppendAllText(Core.Setting.ApplicationDirectory + "\\Logger\\Logger_" + Core.Setting.StartTime.ToString("dd-MM-yyyy_HH.mm.ss") + ".dat", myArgs.Output + Functions.vbNewLine);
                }
                else if (myArgs.Type == ClientEvent.Types.Restart)
                {
                    
                }
                else if (myArgs.Type == ClientEvent.Types.Stop)
                {
                    
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }
    }
}
