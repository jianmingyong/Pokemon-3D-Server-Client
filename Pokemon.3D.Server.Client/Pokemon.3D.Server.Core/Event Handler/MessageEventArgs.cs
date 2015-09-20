using System;

namespace Pokemon_3D_Server_Core.Loggers
{
    /// <summary>
    /// Event Handler for Logger.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Message to Output.
        /// </summary>
        public string OutputMessage { get; set; }

        /// <summary>
        /// New MessageEventArgs.
        /// </summary>
        /// <param name="Logger">Logger.</param>
        public MessageEventArgs(Logger Logger)
        {
            OutputMessage = Logger.ToString();
        }
    }
}
