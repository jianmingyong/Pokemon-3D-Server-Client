using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Global
{
    /// <summary>
    /// Class containing Networking
    /// </summary>
    public class Networking : IDisposable
    {
        /// <summary>
        /// Get/Set SteamReader
        /// </summary>
        public StreamReader Reader { get; set; }

        /// <summary>
        /// Get/Set StreamWriter
        /// </summary>
        public StreamWriter Writer { get; set; }

        /// <summary>
        /// Get/Set Client
        /// </summary>
        public TcpClient Client { get; set; }

        /// <summary>
        /// Get/Set ThreadCollection
        /// </summary>
        public List<Thread> ThreadCollection { get; set; }

        /// <summary>
        /// Get/Set TimerCollection
        /// </summary>
        public List<Timer> TimerCollection { get; set; }

        /// <summary>
        /// Get/Set Player Last Valid Ping
        /// </summary>
        public DateTime LastValidPing { get; set; }

        /// <summary>
        /// Get/Set Player Last Valid Movement
        /// </summary>
        public DateTime LastValidMovement { get; set; }

        /// <summary>
        /// Get/Set Player Login StartTime
        /// </summary>
        public DateTime LoginStartTime { get; set; }

        /// <summary>
        /// New Networking
        /// </summary>
        /// <param name="Client">Client</param>
        public Networking(TcpClient Client)
        {
            Reader = new StreamReader(Client.GetStream());
            Writer = new StreamWriter(Client.GetStream());
            this.Client = Client;
        }

        private void StartListening()
        {
            
        }

        private void ThreadStartListening()
        {
            do
            {
                try
                {
                    string ReturnMessage = Reader.ReadLine();
                    Package Package = new Package(ReturnMessage, Client);
                    QueueMessage.Add("Networking.cs: Receive: " + ReturnMessage, MessageEventArgs.LogType.Debug, Client);

                    if (Package.IsValid)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(Package.Handle));
                        LastValidPing = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    ex.CatchError();
                    return;
                }
            } while (true);
        }

        /// <summary>
        /// Dispose the networking client
        /// </summary>
        public void Dispose()
        {
            if (ThreadCollection[0].IsAlive)
            {
                ThreadCollection[0].Abort();
            }

            Reader.Dispose();
            Writer.Dispose();
            Client.Close();
        }
    }
}
