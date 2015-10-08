using System;
using Pokemon_3D_Server_Core.Loggers;

namespace Pokemon_3D_Server_Core.Event
{
    /// <summary>
    /// Class containing Client Event
    /// </summary>
    public class ClientEvent
    {
        /// <summary>
        /// Delegate for Client Event Handler.
        /// </summary>
        public delegate void ClientEventHandler(object myObject, ClientEventArgs myArgs);

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
            /// Update Logger { Output = Message }
            /// </summary>
            Logger,

            /// <summary>
            /// Restart Client { Output = null }
            /// </summary>
            Restart,

            /// <summary>
            /// Stop Client { Output = null }
            /// </summary>
            Stop,
        }

        /// <summary>
        /// Invoke Event.
        /// </summary>
        /// <param name="Type">Event to invoke.</param>
        public static void Invoke(Types Type)
        {
            try
            {
                if (Type != Types.Logger)
                {
                    Update(null, new ClientEventArgs(Type, null));
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Queue new message to be invoked.
        /// </summary>
        /// <param name="Logger">Logger.</param>
        public static void Invoke(Logger Logger)
        {
            try
            {
                if (Logger.CanDisplay())
                {
                    Update(null, new ClientEventArgs(Types.Logger, Logger.ToString()));
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
