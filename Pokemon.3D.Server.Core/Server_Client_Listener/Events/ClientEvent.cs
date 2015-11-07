using System;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Events
{
    /// <summary>
    /// Class containing Client Event
    /// </summary>
    public class ClientEvent
    {
        /// <summary>
        /// Delegate for Client Event Handler.
        /// </summary>
        public delegate void ClientEventHandler(Types Type);

        /// <summary>
        /// Event to invoke for GUI element.
        /// </summary>
        public static event ClientEventHandler Update;

        /// <summary>
        /// List of Client Event Type.
        /// </summary>
        public enum Types
        {
            /// <summary>
            /// Restart Client { Args = null }
            /// </summary>
            Restart,

            /// <summary>
            /// Stop Client { Args = null }
            /// </summary>
            Stop,

            /// <summary>
            /// Update Client { Args = null }
            /// </summary>
            Update
        }

        /// <summary>
        /// Invoke Event.
        /// </summary>
        /// <param name="Type">Type of event to invoke.</param>
        public static void Invoke(Types Type)
        {
            try
            {
                Update(Type);
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }
    }
}