using System;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Events
{
    /// <summary>
    /// Class containing Player Event
    /// </summary>
    public class PlayerEvent
    {
        /// <summary>
        /// Delegate for Player Event Handler.
        /// </summary>
        public delegate void PlayerEventHandler(Types Type, string Args);

        /// <summary>
        /// Event to invoke for GUI element.
        /// </summary>
        public static event PlayerEventHandler Update;

        /// <summary>
        /// List of Client Event Type.
        /// </summary>
        public enum Types
        {
            /// <summary>
            /// Add Player into the list { Args = Player }
            /// </summary>
            Add,

            /// <summary>
            /// Remove Player in the list { Args = Player }
            /// </summary>
            Remove,

            /// <summary>
            /// Update Player in the list { Args = Player }
            /// </summary>
            Update,
        }

        /// <summary>
        /// Invoke Event.
        /// </summary>
        /// <param name="Type">Type of event to invoke.</param>
        /// <param name="Args">Argument for the event.</param>
        public static void Invoke(Types Type, string Args)
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