using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Global
{
    /// <summary>
    /// Class containing Server Client
    /// </summary>
    public class ServerClient
    {
        /// <summary>
        /// Get current server status
        /// </summary>
        public static Statuses Status = Statuses.Stopped;

        /// <summary>
        /// List of running thread
        /// </summary>
        public static List<Thread> ThreadCollection = new List<Thread>();

        /// <summary>
        /// List of running timer
        /// </summary>
        public static List<Timer> TimerCollection = new List<Timer>();

        private IPEndPoint IPEndPoint;
        private TcpListener Listener;
        private TcpClient Client;
        private StreamReader Reader;
        private StreamWriter Writer;

        /// <summary>
        /// A collection of Status
        /// </summary>
        public enum Statuses
        {
            /// <summary>
            /// Server Started
            /// </summary>
            Started,

            /// <summary>
            /// Server Stopped
            /// </summary>
            Stopped,
        }

        /// <summary>
        /// Start the server
        /// </summary>
        public void Start()
        {
            // Before Running CheckList
            IPEndPoint = new IPEndPoint(IPAddress.Any, Settings.Port);
            Listener = new TcpListener(IPEndPoint);
            Listener.Start();


        }

        /// <summary>
        /// Stop the server
        /// </summary>
        public void Stop()
        {

        }

        /// <summary>
        /// Resume the server
        /// </summary>
        public void Resume()
        {

        }

        private void ThreadStartListening()
        {
            try
            {
                Status = Statuses.Started;
                do
                {
                    Client = Listener.AcceptTcpClient();
                    Reader = new StreamReader(Client.GetStream());


                } while (true);
            }
            catch (Exception)
            {

            }
        }
    }
}
