using System;
using System.Net;
using System.Net.Sockets;

namespace Global
{
    /// <summary>
    /// Class containing Logger
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Get/Set Time
        /// </summary>
        public DateTime Time { get; set; } = DateTime.Now;

        /// <summary>
        /// Get/Set LogType
        /// </summary>
        public MessageEventArgs.LogType LogType { get; set; }

        /// <summary>
        /// Get/Set Client
        /// </summary>
        public TcpClient Client { get; set; }

        /// <summary>
        /// Get/Set Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// New Logger
        /// </summary>
        /// <param name="Message">Message to invoke.</param>
        /// <param name="LogType">Log Type.</param>
        /// <param name="Client">TcpClient.</param>
        public Logger(string Message, MessageEventArgs.LogType LogType, TcpClient Client = null)
        {
            this.Message = Message;
            this.LogType = LogType;
            this.Client = Client;
        }

        /// <summary>
        /// Ability to display the message into the logger.
        /// </summary>
        public bool CanDisplay()
        {
            if (LogType == MessageEventArgs.LogType.Chat && Settings.LoggerChat)
            {
                return true;
            }
            else if (LogType == MessageEventArgs.LogType.Command && Settings.LoggerCommand)
            {
                return true;
            }
            else if (LogType == MessageEventArgs.LogType.Debug && Settings.LoggerDebug)
            {
                return true;
            }
            else if (LogType == MessageEventArgs.LogType.Info && Settings.LoggerInfo)
            {
                return true;
            }
            else if (LogType == MessageEventArgs.LogType.PM && Settings.LoggerPM)
            {
                return true;
            }
            else if (LogType == MessageEventArgs.LogType.PvP && Settings.LoggerPvP)
            {
                return true;
            }
            else if (LogType == MessageEventArgs.LogType.Server && Settings.LoggerServer)
            {
                return true;
            }
            else if (LogType == MessageEventArgs.LogType.Trade && Settings.LoggerTrade)
            {
                return true;
            }
            else if (LogType == MessageEventArgs.LogType.Warning && Settings.LoggerWarning)
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
            if (Client == null)
            {
                return string.Format("{0} [{1}] {2}", Time, LogType, Message);
            }
            else
            {
                return string.Format("{0} [{1}] {2}", Time, LogType, ((IPEndPoint)Client.Client.RemoteEndPoint).Address.ToString() + ": " + Message);
            }
        }
    }
}
