using System;
using Pokemon_3D_Server_Core;
using Pokemon_3D_Server_Core.Server_Client_Listener.Events;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;

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

            Core.Start(Environment.CurrentDirectory);

            Console.Read();
        }

        private static void ClientEvent_Update(ClientEvent.Types Type, object Args)
        {
            try
            {
                if (Type == ClientEvent.Types.Logger)
                {
                    Console.WriteLine(Args);
                }
                else if (Type == ClientEvent.Types.Restart)
                {
                    
                }
                else if (Type == ClientEvent.Types.Stop)
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
