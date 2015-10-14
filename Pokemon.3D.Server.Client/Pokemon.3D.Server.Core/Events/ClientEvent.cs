using System;
using Pokemon_3D_Server_Core.Modules;

namespace Pokemon_3D_Server_Core.Events
{
    /// <summary>
    /// Class containing Client Event
    /// </summary>
    public class ClientEvent
    {
        /// <summary>
        /// Delegate for Client Event Handler.
        /// </summary>
        public delegate void ClientEventHandler(Types Type, object Args);

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
            /// Update Logger { Args = Logger }
            /// </summary>
            Logger,

            /// <summary>
            /// Restart Client { Args = null }
            /// </summary>
            Restart,

            /// <summary>
            /// Stop Client { Args = null }
            /// </summary>
            Stop,

            /// <summary>
            /// Add Player into the list { Args = Player }
            /// </summary>
            AddPlayer,

            /// <summary>
            /// Remove Player in the list { Args = Player }
            /// </summary>
            RemovePlayer,

            /// <summary>
            /// Update Player in the list { Args = Player }
            /// </summary>
            UpdatePlayer,
        }

        /// <summary>
        /// Invoke Event.
        /// </summary>
        /// <param name="Type">Type of event to invoke.</param>
        /// <param name="Args">Argument for the event.</param>
        public static void Invoke(Types Type, object Args)
        {
            try
            {
                Update(Type, Args);
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }
    }
}