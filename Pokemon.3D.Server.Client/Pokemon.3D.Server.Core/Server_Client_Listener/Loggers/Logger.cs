using System;
using System.Net;
using System.Net.Sockets;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Loggers
{
    /// <summary>
    /// Class containing Logger.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Get Log Time.
        /// </summary>
        public DateTime Time { get; } = DateTime.Now;

        /// <summary>
        /// Get/Set LogType.
        /// </summary>
        public LogTypes LogType { get; set; }

        /// <summary>
        /// Get/Set IP Address
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Get/Set Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Message Log Type.
        /// </summary>
        public enum LogTypes
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
        /// New Logger.
        /// </summary>
        /// <param name="Message">Message to invoke.</param>
        /// <param name="LogType">Log Type.</param>
        /// <param name="Client">TcpClient.</param>
        public Logger(string Message, LogTypes LogType, TcpClient Client = null)
        {
            this.Message = Message;
            this.LogType = LogType;

            try
            {
                IPAddress = Client == null ? "" : ((IPEndPoint)Client.Client.RemoteEndPoint).Address.ToString();
            }
            catch (Exception)
            {
                IPAddress = "";
            }
        }

        /// <summary>
        /// Ability to display the message into the logger.
        /// </summary>
        public bool CanDisplay()
        {
            if (LogType == LogTypes.Chat && Core.Setting.LoggerChat)
            {
                return true;
            }
            else if (LogType == LogTypes.Command && Core.Setting.LoggerCommand)
            {
                return true;
            }
            else if (LogType == LogTypes.Debug && Core.Setting.LoggerDebug)
            {
                return true;
            }
            else if (LogType == LogTypes.Info && Core.Setting.LoggerInfo)
            {
                return true;
            }
            else if (LogType == LogTypes.PM && Core.Setting.LoggerPM)
            {
                return true;
            }
            else if (LogType == LogTypes.PvP && Core.Setting.LoggerPvP)
            {
                return true;
            }
            else if (LogType == LogTypes.Server && Core.Setting.LoggerServer)
            {
                return true;
            }
            else if (LogType == LogTypes.Trade && Core.Setting.LoggerTrade)
            {
                return true;
            }
            else if (LogType == LogTypes.Warning && Core.Setting.LoggerWarning)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the full logger string.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} [{1}] {2}", IPAddress == "" ? Time.ToString() : Time.ToString() + " " + IPAddress, LogType, Message);
        }
    }
}