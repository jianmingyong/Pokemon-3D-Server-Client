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

        /// <summary>
        /// List of Player Collection
        /// </summary>
        public static PlayerCollection Player = new PlayerCollection();

        /// <summary>
        /// World
        /// </summary>
        public static World World = new World();

        private static IPEndPoint IPEndPoint;
        private static TcpListener Listener;
        private static TcpClient Client;
        private static StreamReader Reader;
        private static StreamWriter Writer;

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
        public static void Start()
        {
            try
            {
                // Before Running CheckList
                if (!My.Computer.Network.IsAvailable)
                {
                    Stop();
                    return;
                }
                IPEndPoint = new IPEndPoint(IPAddress.Any, Settings.Port);
                Listener = new TcpListener(IPEndPoint);
                Listener.Start();

                // Threading
                Thread Thread = new Thread(new ThreadStart(ThreadStartListening)) { IsBackground = true };
                Thread.Start();
                ThreadCollection.Add(Thread);

                // Timer
                Timer Timer = new Timer(new TimerCallback(StartListening), null, 0, 1000);
                TimerCollection.Add(Timer);

                // Timer 2
                Timer Timer1 = new Timer(new TimerCallback(World.Update), null, 0, 1000);
                TimerCollection.Add(Timer1);

                // Timer 3
                Timer Timer2 = new Timer(new TimerCallback(PackageHandler.Handle), null, 0, 1);

                Status = Statuses.Started;
                QueueMessage.Add("ServerClient.cs: Server Client is initalizing.", MessageEventArgs.LogType.Info);
                if (Settings.OfflineMode)
                {
                    QueueMessage.Add("ServerClient.cs: Players with offline profile can join the server.", MessageEventArgs.LogType.Info);
                }

                string GameMode = null;
                for (int i = 0; i < Settings.GameMode.Count; i++)
                {
                    GameMode += Settings.GameMode[i] + ", ";
                }
                GameMode = GameMode.Remove(GameMode.LastIndexOf(","));

                if (Functions.CheckPortOpen())
                {
                    QueueMessage.Add(string.Format(@"ServerClient.cs: Server Started. Players can join using the following address: {0}:{1} (Global), {2}:{3} (Local) and with the following GameMode: {4}.",
                        Settings.IPAddress,
                        Settings.Port,
                        Functions.GetPrivateIP(),
                        Settings.Port,
                        GameMode), MessageEventArgs.LogType.Info);
                }
                else
                {
                    QueueMessage.Add(string.Format(@"ServerClient.cs: The specific Port {0} is not opened. External/Global IP will not accept new players.", Settings.Port), MessageEventArgs.LogType.Info);
                    QueueMessage.Add(string.Format(@"ServerClient.cs: Server Started. Players can join using the following address: {0}:{1} (Local) and with the following GameMode: {2}.",
                        Functions.GetPrivateIP(),
                        Settings.Port,
                        GameMode), MessageEventArgs.LogType.Info);
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
                Stop();
            }
        }

        /// <summary>
        /// Stop the server
        /// </summary>
        public static void Stop()
        {
            try
            {
                Listener.Stop();

                for (int i = 0; i < TimerCollection.Count; i++)
                {
                    TimerCollection[i].Dispose();
                    TimerCollection.RemoveAt(0);
                }

                Status = Statuses.Stopped;
                QueueMessage.Add("ServerClient.vb: Server Client stopped.", MessageEventArgs.LogType.Info);
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }

        private static void StartListening(object obj = null)
        {
            if (ThreadCollection.Count == 0)
            {
                Start();
                QueueMessage.Add("ServerClient.vb: Server Client restarted.", MessageEventArgs.LogType.Info);
            }
        }

        private static void ThreadStartListening()
        {
            try
            {
                Status = Statuses.Started;
                do
                {
                    Client = Listener.AcceptTcpClient();
                    Reader = new StreamReader(Client.GetStream());
                    string ReturnMessage = Reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(ReturnMessage))
                    {
                        Package Package = new Package(ReturnMessage, Client);
                        QueueMessage.Add("ServerClient.cs: Receive: " + ReturnMessage, MessageEventArgs.LogType.Debug);
                        if (Package.IsValid)
                        {
                            Package.Handle();
                        }
                    }
                } while (true);
            }
            catch (SocketException)
            {
                Stop();
                ThreadCollection.RemoveAt(0);
            }
            catch (IOException)
            {
                Stop();
                ThreadCollection.RemoveAt(0);
            }
            catch (Exception ex)
            {
                ex.CatchError();
                Stop();
                ThreadCollection.RemoveAt(0);
            }
        }

        /// <summary>
        /// Sent Package Data to Player
        /// </summary>
        /// <param name="p">Package</param>
        public static void SentToPlayer(Package p)
        {
            if (Player.HasPlayer(p.Client))
            {
                Player.GetPlayer(p.Client).Client.PackageToSend.Enqueue(p);
            }
            else
            {
                try
                {
                    Writer = new StreamWriter(p.Client.GetStream()) { AutoFlush = true };
                    Writer.WriteLine(p.ToString());
                    Writer.Flush();
                    QueueMessage.Add("ServerClient.cs: Sent: " + p.ToString(), MessageEventArgs.LogType.Debug, Client);
                }
                catch (Exception)
                {
                    QueueMessage.Add("ServerClient.cs: StreamWriter failed to send package data.", MessageEventArgs.LogType.Debug, Client);
                }
            }
        }

        /// <summary>
        /// Sent Package Data to All Player
        /// </summary>
        /// <param name="p">Package</param>
        public static void SendToAllPlayer(Package p)
        {
            for (int i = 0; i < Player.Count; i++)
            {
                if (p.Client == null || Player[i].Client.Client != p.Client)
                {
                    Player[i].Client.PackageToSend.Enqueue(p);
                }
            }
        }
    }
}
