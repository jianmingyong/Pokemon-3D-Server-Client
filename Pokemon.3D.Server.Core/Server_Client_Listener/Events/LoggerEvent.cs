using System;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Events
{
    /// <summary>
    /// Class containing Logger Event
    /// </summary>
    public class LoggerEvent
    {
        /// <summary>
        /// Delegate for Listen Logger Event Handler.
        /// </summary>
        public delegate void LoggerEventHandler(string Args);

        /// <summary>
        /// Event to invoke for GUI element.
        /// </summary>
        public static event LoggerEventHandler Update;

        /// <summary>
        /// Invoke Event.
        /// </summary>
        /// <param name="Args">Argument for the event.</param>
        public static void Invoke(string Args)
        {
            try
            {
                Update(Args);
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }
    }
}