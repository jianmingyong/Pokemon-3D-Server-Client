using System;
using System.Net;
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
        public string _OutputMessage;

        /// <summary>
        /// Message to Output.
        /// </summary>
        public string OutputMessage
        {
            get
            {
                return _OutputMessage;
            }
            set
            {
                _OutputMessage = value;
            }
        }

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
            string Logger;
            if (Client != null)
            {
                Logger = ((IPEndPoint)Client.Client.RemoteEndPoint).Address.ToString() + ": " + Message;
            }
            else
            {
                Logger = Message;
            }

            switch (LogType)
            {
                case LogType.Info:
                    OutputMessage = DateTime.Now + " [Info] " + Logger;
                    break;
                case LogType.Warning:
                    OutputMessage = DateTime.Now + " [Warning] " + Logger;
                    break;
                case LogType.Debug:
                    OutputMessage = DateTime.Now + " [Debug] " + Logger;
                    break;
                case LogType.Chat:
                    OutputMessage = DateTime.Now + " [Chat] " + Logger;
                    break;
                case LogType.PM:
                    OutputMessage = DateTime.Now + " [PM] " + Logger;
                    break;
                case LogType.Server:
                    OutputMessage = DateTime.Now + " [Server] " + Logger;
                    break;
                case LogType.Trade:
                    OutputMessage = DateTime.Now + " [Trade] " + Logger;
                    break;
                case LogType.PvP:
                    OutputMessage = DateTime.Now + " [PvP] " + Logger;
                    break;
                case LogType.Command:
                    OutputMessage = DateTime.Now + " [Command] " + Logger;
                    break;
                default:
                    OutputMessage = DateTime.Now + " [Info] " + Logger;
                    break;
            }
        }
    }
}
