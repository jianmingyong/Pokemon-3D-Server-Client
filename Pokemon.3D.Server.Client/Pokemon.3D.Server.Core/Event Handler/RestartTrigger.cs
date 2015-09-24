using System;

namespace Pokemon_3D_Server_Core.Network
{
    /// <summary>
    /// Class containing Restart Trigger Event.
    /// </summary>
    public class RestartTrigger
    {
        /// <summary>
        /// Delegate for Restart Trigger.
        /// </summary>
        public delegate void RemoteRestart(object myObject, EventArgs myArgs);

        /// <summary>
        /// Event to invoke when restarting.
        /// </summary>
        public static event RemoteRestart RestartSwitch;

        /// <summary>
        /// Restart the server.
        /// </summary>
        public static void Restart()
        {
            RestartSwitch(null, null);
        }
    }
}
