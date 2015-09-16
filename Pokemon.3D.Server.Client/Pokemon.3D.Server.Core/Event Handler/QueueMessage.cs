using System.Net.Sockets;

namespace Global
{
    /// <summary>
    /// Class containing Logger Functions
    /// </summary>
    public class QueueMessage
    {
        /// <summary>
        /// Delegate for Message Handler.
        /// </summary>
        public delegate void QueueMessageHandler(object myObject, MessageEventArgs myArgs);

        /// <summary>
        /// Event to invoke when queuing a new message.
        /// </summary>
        public static event QueueMessageHandler AddMessage;

        /// <summary>
        /// Queue New Message to be invoked.
        /// </summary>
        /// <param name="Message">Message to display.</param>
        /// <param name="LogType">Log Type.</param>
        /// <param name="Client">TcpClient of the player.</param>
        public static void Add(string Message, MessageEventArgs.LogType LogType, TcpClient Client = null)
        {
            AddMessage(null, new MessageEventArgs(Message, LogType, Client));
        }
    }
}
