using System.Collections.Generic;
using System.Net.Sockets;

namespace Pokemon_3D_Server_Core.Loggers
{
    /// <summary>
    /// Class containing Logger Collections.
    /// </summary>
    public class LoggerCollection : List<Logger>
    {
        /// <summary>
        /// Add the logger to the top of the collection.
        /// </summary>
        /// <param name="Message">Message to output.</param>
        /// <param name="LogType">Log Type.</param>
        /// <param name="Client">Optional: Client.</param>
        public void Add(string Message, Logger.LogTypes LogType, TcpClient Client = null)
        {
            if (Count >= 1000)
            {
                RemoveAt(0);
            }
            QueueMessage.Add(new Logger(Message, LogType, Client));
            Add(new Logger(Message, LogType, Client));
        }
    }
}
