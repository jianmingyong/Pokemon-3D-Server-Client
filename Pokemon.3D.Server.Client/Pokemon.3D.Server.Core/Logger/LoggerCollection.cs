using System.IO;
using System.Net.Sockets;
using Pokemon_3D_Server_Core.Event;

namespace Pokemon_3D_Server_Core.Loggers
{
    /// <summary>
    /// Class containing Logger Collections.
    /// </summary>
    public class LoggerCollection
    {
        private FileStream FileStream;
        private StreamReader Reader;
        private StreamWriter Writer;

        /// <summary>
        /// Setup new Logger.
        /// </summary>
        public void Setup()
        {
            if (!Directory.Exists(Core.Setting.ApplicationDirectory + "\\Logger"))
            {
                Directory.CreateDirectory(Core.Setting.ApplicationDirectory + "\\Logger");
            }

            FileStream = new FileStream(Core.Setting.ApplicationDirectory + "\\Logger\\Logger_" + Core.Setting.StartTime.ToString("dd-MM-yyyy_HH.mm.ss") + ".dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Reader = new StreamReader(FileStream);
            Writer = new StreamWriter(FileStream) { AutoFlush = true };

            Add("LoggerCollection.cs: Logger initiated.",Logger.LogTypes.Info);
        }

        /// <summary>
        /// Add the logger to the top of the collection.
        /// </summary>
        /// <param name="Message">Message to output.</param>
        /// <param name="LogType">Log Type.</param>
        /// <param name="Client">Optional: Client.</param>
        public void Add(string Message, Logger.LogTypes LogType, TcpClient Client = null)
        {
            Logger Logger = new Logger(Message, LogType, Client);
            Writer.WriteLine(Logger.ToString());
            Writer.Flush();

            ClientEvent.Invoke(Logger);
        }
    }
}
