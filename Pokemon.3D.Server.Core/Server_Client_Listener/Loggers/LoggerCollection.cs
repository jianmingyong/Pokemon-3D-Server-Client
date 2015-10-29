using System;
using System.IO;
using System.Net.Sockets;
using Pokemon_3D_Server_Core.Server_Client_Listener.Events;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Loggers
{
    /// <summary>
    /// Class containing Logger Collections.
    /// </summary>
    public class LoggerCollection : IDisposable
    {
        private FileStream FileStream;
        private StreamWriter Writer;
        private static readonly object Lock = new object();

        /// <summary>
        /// Start the Logger.
        /// </summary>
        public void Start()
        {
            if (!Directory.Exists(Core.Setting.ApplicationDirectory + "\\Logger"))
            {
                Directory.CreateDirectory(Core.Setting.ApplicationDirectory + "\\Logger");
            }

            FileStream = new FileStream(Core.Setting.ApplicationDirectory + "\\Logger\\Logger_" + Core.Setting.StartTime.ToString("dd-MM-yyyy_HH.mm.ss") + ".dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Writer = new StreamWriter(FileStream) { AutoFlush = true };

            Log("Logger initialized.", Logger.LogTypes.Info);
        }

        /// <summary>
        /// Dispose the Logger.
        /// </summary>
        public void Dispose()
        {
            Writer.Dispose();
            FileStream.Dispose();
        }

        /// <summary>
        /// Log the message.
        /// </summary>
        /// <param name="Message">Message to log.</param>
        /// <param name="LogType">Log type.</param>
        /// <param name="Client">Client.</param>
        public void Log(string Message, Logger.LogTypes LogType, TcpClient Client = null)
        {
            lock (Lock)
            {
                try
                {
                    Logger Logger = new Logger(Message, LogType, Client);

                    if (Logger.CanDisplay())
                    {
                        Writer.WriteLine(Logger.ToString());
                        Writer.Flush();
                        ClientEvent.Invoke(ClientEvent.Types.Logger, Logger.ToString());
                    }
                }
                catch (Exception) { }
            }
        }   
    }
}