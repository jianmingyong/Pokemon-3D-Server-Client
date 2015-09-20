namespace Pokemon_3D_Server_Core.Loggers
{
    /// <summary>
    /// Class containing Logger Functions.
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
        /// <param name="Logger">Logger.</param>
        public static void Add(Logger Logger)
        {
            if (Logger.CanDisplay())
            {
                AddMessage(null, new MessageEventArgs(Logger));
            }
        }
    }
}
