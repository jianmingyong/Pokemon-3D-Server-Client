using System;
using System.Net.Sockets;

namespace Global
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
        /// Message Log Type
        /// </summary>
        public enum LogType
        {
            /// <summary>
            /// General Log Type.
            /// </summary>
            Info,

            /// <summary>
            /// Error Log Type.
            /// </summary>
            Warning,

            /// <summary>
            /// Debug Log Type.
            /// </summary>
            Debug,

            /// <summary>
            /// Chat Log Type.
            /// </summary>
            Chat,

            /// <summary>
            /// PM Log Type.
            /// </summary>
            PM,

            /// <summary>
            /// Server Chat Log Type.
            /// </summary>
            Server,

            /// <summary>
            /// Trade Log Type.
            /// </summary>
            Trade,

            /// <summary>
            /// PvP Log Type.
            /// </summary>
            PvP,

            /// <summary>
            /// Command Log Type.
            /// </summary>
            Command,
        }

        /// <summary>
        /// Public Sub new(string Message, LogType LogType, TcpClient Client)
        /// </summary>
        /// <param name="Message">Message to invoke.</param>
        /// <param name="LogType">Log Type.</param>
        /// <param name="Client">TcpClient.</param>
        public MessageEventArgs(string Message, LogType LogType, TcpClient Client = null)
        {
            Logger Logger = new Logger(Message, LogType, Client);
            OutputMessage = Logger.ToString();
        }
    }
}
